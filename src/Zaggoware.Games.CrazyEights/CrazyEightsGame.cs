namespace Zaggoware.Games.CrazyEights
{
    using System;
    using System.Linq;

    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.Common.Events;

    public class CrazyEightsGame : TurnBasedCardGameBase<CrazyEightsGameRules, CrazyEightsPlayer>
    {
        private bool _enoughCardsPlayed;
        private bool _enoughCardsDrawn;
        private int _mandatoryDrawCount;
        private bool _shouldSkipNextTurn;
        private bool _shouldStackOrMandatoryDraw;
        private ColorChangerState? _colorChangerState;
        private bool _shouldChangeColor;

        public CrazyEightsGame(CrazyEightsGameRules rules)
            : base(rules)
        {
        }

        public event EventHandler<CardDrawnEventArgs<CrazyEightsPlayer>>? CardDrawn;

        public event EventHandler<CardPlayedEventArgs<CrazyEightsPlayer>>? CardPlayed;

        public event EventHandler<CardColorChangedEventArgs<CrazyEightsPlayer>>? ColorChanged;

        public event EventHandler<CardSuitChangedEventArgs<CrazyEightsPlayer>>? SuitChanged;

        public event EventHandler<CrazyEightsPlayer>? PlayerFinished;

        public override bool CanChangeTurnDirection =>
            Rules.ReversingCards.Any(c => c == DiscardPile.LastOrDefault().Rank);

        public CardStack DiscardPile { get; } = new CardStack();

        public CardSuit? NextSuit => _colorChangerState?.Suit;

        public bool ShouldStackOrMandatoryDraw => _shouldStackOrMandatoryDraw;

        public CardStack Stockpile { get; } = new CardStack();

        public bool ChangeColor(CardColor color)
        {
            if (!CanChangeColorOrSuit(color, null))
            {
                return false;
            }

            _colorChangerState = new ColorChangerState
            {
                Color = color
            };
            _shouldChangeColor = false;

            OnColorChanged(new CardColorChangedEventArgs<CrazyEightsPlayer>(CurrentPlayer!, color));
            return true;
        }

        public bool ChangeSuit(CardSuit suit)
        {
            if (!CanChangeColorOrSuit(null, suit))
            {
                return false;
            }

            _colorChangerState = new ColorChangerState
            {
                Suit = suit
            };
            _shouldChangeColor = false;

            OnSuitChanged(new CardSuitChangedEventArgs<CrazyEightsPlayer>(CurrentPlayer!, suit));
            return true;
        }

        public PlayingCard? DrawCard()
        {
            if (CurrentPlayer == null)
            {
                return null;
            }

            var card = DrawCardFromStockpileInternal();
            CurrentPlayer.Hand.Add(card);

            // If the player has a mandatory cad being played, they should draw the required amount.
            if (_shouldStackOrMandatoryDraw)
            {
                _mandatoryDrawCount--;
                if (_mandatoryDrawCount > 0)
                {
                    return card;
                }

                _shouldStackOrMandatoryDraw = false;
            }

            _enoughCardsDrawn = true;

            OnCardDrawn(new CardDrawnEventArgs<CrazyEightsPlayer>(CurrentPlayer!, card));
            return card;
        }

        public bool PlayCard(PlayingCard card)
        {
            if (!IsStarted || IsPaused || !IsTurnStarted || CurrentPlayer == null || _enoughCardsPlayed)
            {
                return false;
            }

            var canPlayCard = CanPlayCard(card);
            if (!canPlayCard)
            {
                return false;
            }

            _colorChangerState = null;

            DiscardPile.AddCard(card);
            CurrentPlayer.Hand.Remove(card);

            OnCardPlayed(new CardPlayedEventArgs<CrazyEightsPlayer>(CurrentPlayer, card));

            //// From this point on, the playing rules have been checked and passed.
            //// TODO: Split into separate methods.

            if (Rules.MandatoryDrawCards.ContainsKey(card.Rank))
            {
                // A mandatory draw card is played or countered. Add the mandatory draw amount for the next player.
                _shouldStackOrMandatoryDraw = true;
                _mandatoryDrawCount += Rules.MandatoryDrawCards[card.Rank];
                _enoughCardsPlayed = true;

                // Continue executing as it's possible that the mandatory draw card is also another special card.
            }

            if (Rules.ColorChangerCards.Contains(card.Rank))
            {
                _shouldChangeColor = true;
                _enoughCardsPlayed = true;
            }

            // Check if the player's hand is empty and if the turn should end.
            if (CurrentPlayer.Hand.Count == 0
                && (Players.Count(p => p.Hand.Any()) <= 1
                    || !Rules.EliminationMode
                    || (Rules.EliminationMode && !_shouldChangeColor)))
            {
                // The player has no more cards in hand. Turn can be ended automatically.
                EndTurn();
                return true;
            }

            if (Rules.SingleTurnStackableCards.Contains(card.Rank))
            {
                // Single-turn stackable card has been played.
                _enoughCardsPlayed = false;
                _enoughCardsDrawn = false;

                // The current player played a single-turn stackable card.
                var suitableCard = CurrentPlayer.Hand.FirstOrDefault(c => CanPlayCard(card, c));
                if (suitableCard != PlayingCard.Unknown)
                {
                    // Suitable follow-up card found.
                    return true;
                }

                // If the player does not posses a suitable card after playing a single-turn stackable card,
                // a card will be drawn from the stockpile automatically. The player can then decide to play it or keep
                // it and end their turn. TODO: Force play drawn card if possible rule?
                DrawCard();
                return true;
            }

            if (Rules.SkipNextTurnCards.Contains(card.Rank))
            {
                // The current player played a card that skips the next player's turn.
                _shouldSkipNextTurn = true;
                _enoughCardsPlayed = true;

                // Turn can automatically be ended.
                EndTurn();
                return true;
            }

            _enoughCardsPlayed = true;

            if (Rules.ReversingCards.Contains(card.Rank))
            {
                ChangeTurnDirection();
            }

            EndTurn();
            return true;
        }

        protected override bool CanEndTurn(int turnIndex, int playerIndex, DateTime turnStartedDateTimeUtc)
        {
            return _shouldSkipNextTurn
                || (base.CanEndTurn(turnIndex, playerIndex, turnStartedDateTimeUtc)
                    && (_enoughCardsDrawn || _enoughCardsPlayed)
                    && !_shouldChangeColor);
        }

        protected override void OnInitialize(CrazyEightsGameRules rules)
        {
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            RandomizePlayingOrder();

            Stockpile.Clear();
            DiscardPile.Clear();

            foreach (var player in Players)
            {
                player.Hand.Clear();
            }

            for (var i = 0; i < Rules.NumberOfPacks; i++)
            {
                Stockpile.AddCards(new CardPack(Rules.ExcludedCards, Rules.NumberOfJokersPerPack, true));
            }

            Stockpile.Shuffle();

            for (var i = 0; i < Rules.StartingCardsPerPlayer; i++)
            {
                foreach (var player in Players)
                {
                    var card = Stockpile.DrawCard();
                    player.Hand.Add(card.GetValueOrDefault());
                }
            }

            DiscardPile.AddCard(Stockpile.DrawCard().GetValueOrDefault());
        }

        protected override void OnTurnBeginning(GameTurnEventArgs<CrazyEightsPlayer> args)
        {
            _enoughCardsPlayed = false;
            _enoughCardsDrawn = false;
            _shouldChangeColor = false;

            base.OnTurnBeginning(args);
        }

        protected override void OnTurnBegan(GameTurnEventArgs<CrazyEightsPlayer> args)
        {
            base.OnTurnBegan(args);

            if (_shouldSkipNextTurn)
            {
                // Previous player played a skip next turn card. Current turn is immediately ended.
                EndTurn();
                _shouldSkipNextTurn = false;
                return;
            }

            if (!_shouldStackOrMandatoryDraw || CanCounterMandatoryDrawCard())
            {
                return;
            }

            // The player does not posses any suitable stackable mandatory draw cards to counter the mandatory draw.
            // The player is now forced to draw the mandatory amount of cards.
            while (_shouldStackOrMandatoryDraw)
            {
                DrawCard();
            }
        }

        protected override void OnTurnEnded(GameTurnEventArgs<CrazyEightsPlayer> args)
        {
            base.OnTurnEnded(args);

            if (args.Player.Hand.Count == 0)
            {
                OnPlayerFinished(args.Player);

                if (!Rules.EliminationMode)
                {
                    // The current player finished his hands. With elimination mode disabled, the game ends.
                    // TODO: Calculate points
                    // TODO: Invoke a 'Finish' rather than directly Stop.
                    Stop();
                    return;
                }
            }

            if (Players.Count(p => p.Hand.Any()) > 1)
            {
                return;
            }

            // Only 1 player with cards lefts in elimination mode. Game ended.
            // TODO: Calculate points
            // TODO: Invoke a 'Finish' rather than directly Stop.
            Stop();
        }

        private bool CanChangeColorOrSuit(CardColor? color, CardSuit? suit)
        {
            if (!_shouldChangeColor || (color == null && suit == null))
            {
                return false;
            }

            return Rules.ColorChangerMode == ColorChangerMode.Color
                ? color != null || suit != null
                : suit != null;
        }

        private bool CanCounterMandatoryDrawCard()
        {
            if (DiscardPile.Count == 0)
            {
                return false;
            }

            var previousCard = DiscardPile.Last();
            var playerHand = CurrentPlayer!.Hand;
            return playerHand.Any(c => CanCounterMandatoryDrawCard(previousCard, c));
        }

        private bool CanCounterMandatoryDrawCard(PlayingCard previousCard, PlayingCard cardToPlay)
        {
            return Rules.MandatoryDrawCards.Any(r => r.Key == cardToPlay.Rank)
                && CanStackCard(previousCard, cardToPlay);
        }

        private bool CanEndWithCard(PlayingCard cardToPlay)
        {
            // Color changer card ending.
            var hasColorChangerCard = Rules.ColorChangerCards.Any(c => c == cardToPlay.Rank);
            if (!hasColorChangerCard || Rules.AllowColorChangerCardEnding)
            {
                return true;
            }

            // Reversing card ending.
            var hasReversingCard = Rules.ReversingCards.Any(c => c == cardToPlay.Rank);
            if (!hasReversingCard || Rules.AllowReversingCardEnding)
            {
                return true;
            }

            // Mandatory draw card ending.
            var hasMandatoryDrawCard = Rules.MandatoryDrawCards.Any(c => c.Key == cardToPlay.Rank);
            if (!hasMandatoryDrawCard || Rules.AllowMandatoryDrawCardEnding)
            {
                return true;
            }

            // Single-turn stackable card ending.
            var hasSingleTurnStackableCard = Rules.SingleTurnStackableCards.Any(c => c == cardToPlay.Rank);
            return !hasSingleTurnStackableCard || Rules.AllowSingleTurnStackableCardEnding;
        }

        private bool CanPlayCard(PlayingCard cardToPlay)
        {
            if (_enoughCardsPlayed
                || _shouldSkipNextTurn
                || CurrentPlayer == null
                || !CurrentPlayer.Hand.Contains(cardToPlay))
            {
                return false;
            }

            if (DiscardPile.Count == 0)
            {
                return true;
            }

            if (_shouldStackOrMandatoryDraw)
            {
                return CanCounterMandatoryDrawCard();
            }

            var previousCard = DiscardPile.Last();
            return CanPlayCard(previousCard, cardToPlay);
        }

        private bool CanPlayCard(PlayingCard previousCard, PlayingCard cardToPlay)
        {
            if (CurrentPlayer == null)
            {
                return false;
            }

            // TODO: Split into separate methods.
            var canPlayCard = CanStackCard(previousCard, cardToPlay);
            if (!canPlayCard)
            {
                // The card can't be played, no point in continuing.
                return false;
            }

            // If the player tries to play their last card, some extra rules may apply.
            // Check if the player has any of the special cards and if the rules allow it to end with it.
            return CurrentPlayer.Hand.Count > 1
                || CanEndWithCard(cardToPlay);
        }

        private bool CanStackCard(PlayingCard previousCard, PlayingCard cardToPlay)
        {
            var stackingMode = Rules.DefaultStackingMode;

            // Special rules apply for color changer cards.
            if (Rules.ColorChangerCards.Any(c => c == cardToPlay.Rank))
            {
                // Replace the default stacking mode to check if the color changer card can be played.
                stackingMode = Rules.ColorChangerCardStackingMode ?? Rules.DefaultStackingMode;
            }

            if (stackingMode.HasFlag(CardStackingMode.Always))
            {
                return true;
            }

            if (_colorChangerState != null)
            {
                // A color changer card was played in the previous turn.
                // Only a card of the given color or suit can be played.
                return Rules.ColorChangerMode == ColorChangerMode.Color
                    ? cardToPlay.Color == _colorChangerState.Color
                    : cardToPlay.Suit == _colorChangerState.Suit;
            }

            var isSameRank = stackingMode.HasFlag(CardStackingMode.SameRank)
                && previousCard.HasSameRank(cardToPlay);

            var isSameSuit = stackingMode.HasFlag(CardStackingMode.SameSuit)
                && (previousCard.HasSameSuit(cardToPlay)
                    || previousCard.Suit == CardSuit.Any
                    || cardToPlay.Suit == CardSuit.Any);

            var isSameColor = stackingMode.HasFlag(CardStackingMode.SameColor)
                && previousCard.HasSameColor(cardToPlay);

            return isSameRank || isSameSuit || isSameColor;
        }

        private PlayingCard DrawCardFromStockpileInternal()
        {
            // Re-stock the stockpile if it was empty.
            if (!Stockpile.Any())
            {
                RestockStockpile();
            }

            var card = Stockpile.DrawCard();

            // Check and re-stock again if the stockpile is empty.
            if (!Stockpile.Any())
            {
                RestockStockpile();
            }

            return card.GetValueOrDefault();
        }

        private void OnCardDrawn(CardDrawnEventArgs<CrazyEightsPlayer> args)
        {
            CardDrawn?.Invoke(this, args);
        }

        private void OnCardPlayed(CardPlayedEventArgs<CrazyEightsPlayer> args)
        {
            CardPlayed?.Invoke(this, args);
        }

        private void OnColorChanged(CardColorChangedEventArgs<CrazyEightsPlayer> args)
        {
            ColorChanged?.Invoke(this, args);
        }

        private void OnPlayerFinished(CrazyEightsPlayer player)
        {
            PlayerFinished?.Invoke(this, player);
        }

        private void RestockStockpile()
        {
            // Move the cards from the discard pile into the stockpile and then reshuffle.
            // The last played card is then placed back into the discard pile.
            var lastDiscardedCard = DiscardPile.DrawCard();
            if (lastDiscardedCard == null)
            {
                throw new InvalidOperationException("Both the stockpile and the discard pile are empty.");
            }

            Stockpile.AddCards(DiscardPile);
            Stockpile.Shuffle();

            DiscardPile.Clear();
            DiscardPile.AddCard(lastDiscardedCard.Value);
        }

        private void OnSuitChanged(CardSuitChangedEventArgs<CrazyEightsPlayer> args)
        {
            SuitChanged?.Invoke(this, args);
        }

        private class ColorChangerState
        {
            public CardColor? Color { get; set; }

            public CardSuit? Suit { get; set; }
        }
    }
}
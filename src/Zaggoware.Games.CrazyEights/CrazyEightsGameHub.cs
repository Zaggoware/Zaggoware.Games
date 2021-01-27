namespace Zaggoware.Games.CrazyEights
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.SignalR;

    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights.Models;
    using Zaggoware.Games.Common;
    using Zaggoware.Games.Common.Events;
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.CrazyEights.Models.Events;
    using Zaggoware.Games.Hubs;
    using Zaggoware.Games.Hubs.Extensions;

    public class CrazyEightsGameHub : TurnBasedGameHub<CrazyEightsGame, CrazyEightsGameInfoHubModel,
        CrazyEightsGameRules, CrazyEightsPlayer, CrazyEightsPlayerHubModel>
    {
        public CrazyEightsGameHub(IHubContext<CrazyEightsGameHub> context)
            : base(new GameHubContext(context.Clients, context.Groups), new CrazyEightsGameInfoModelFactory())
        {
            EventHandlers.Context ??= context;
        }

        protected override CrazyEightsGame CreateGame()
        {
            var rulesPreset = Context.GetHttpContext()?.Request.Query["rulesPreset"].ToString();
            if (string.IsNullOrEmpty(rulesPreset))
            {
                rulesPreset = "Pesten";
            }

            // TODO: Move logic to GameLobbyHub?
            var game = new CrazyEightsGame(CrazyEightsGameRules.Create(rulesPreset));
            game.Started += EventHandlers.OnGameStarted;
            game.TurnEnded += EventHandlers.OnGameTurnEnded;
            game.CardPlayed += EventHandlers.OnCardPlayed;
            game.CardDrawn += EventHandlers.OnCardDrawn;
            game.PlayerFinished += EventHandlers.OnPlayerFinished;
            game.DiscardColorChanged += EventHandlers.OnDiscardColorChanged;
            game.DiscardSuitChanged += EventHandlers.OnDiscardSuitChanged;
            return game;
        }

        protected override CrazyEightsPlayer CreatePlayer(IGameConnection connection)
        {
            return new CrazyEightsPlayer(connection);
        }

        [HubMethodName(nameof(CrazyEightsGameHubActions.ChangeDiscardColor))]
        public bool ChangeDiscardColor(CardColor color)
        {
            return IsCurrentPlayer && (CurrentGame?.ChangeDiscardColor(color) ?? false);
        }

        [HubMethodName(nameof(CrazyEightsGameHubActions.ChangeDiscardSuit))]
        public bool ChangeDiscardSuit(CardSuit suit)
        {
            return IsCurrentPlayer && (CurrentGame?.ChangeDiscardSuit(suit) ?? false);
        }

        [HubMethodName(nameof(CrazyEightsGameHubActions.DrawCard))]
        public PlayingCard? DrawCard()
        {
            return IsCurrentPlayer ? CurrentGame?.DrawCard() : null;
        }

        [HubMethodName(nameof(CrazyEightsGameHubActions.FetchPlayerHand))]
        public PlayingCard[] FetchPlayerHand()
        {
            var player = CurrentGame?.GetPlayers().SingleOrDefault(p => p.Connection.Id == Context.ConnectionId);
            if (player == null)
            {
                return Array.Empty<PlayingCard>();
            }

            return player.Hand
                .OrderBy(c => c.Suit)
                .ThenBy(c => c.Rank)
                .ToArray();
        }

        [HubMethodName(nameof(CrazyEightsGameHubActions.PlayCard))]
        public bool PlayCard(int index, CardSuit suit, CardRank rank)
        {
            if (!IsCurrentPlayer)
            {
                return false;
            }

            var game = CurrentGame;
            var card = game!.CurrentPlayer!.Hand.ElementAtOrDefault(index);
            if (card.Suit != suit || card.Rank != rank)
            {
                card = game.CurrentPlayer!.Hand.FirstOrDefault(c => c.Suit == suit && c.Rank == rank)!;
                if (card == PlayingCard.Unknown)
                {
                    return false;
                }
            }

            return game.PlayCard(card);
        }

        private static class EventHandlers
        {
            public static IHubContext<CrazyEightsGameHub>? Context { get; set; }


            public static void OnCardDrawn(object? sender, CardDrawnEventArgs<CrazyEightsPlayer> args)
            {
                // Don't return the actual card as we don't want to broadcast to everyone what the card was.
                Context?.Clients.All.InvokeEventAsync(
                    new CardDrawnHubEventModel(new CrazyEightsPlayerHubModel(args.Player)));
            }

            public static void OnCardPlayed(object? sender, CardPlayedEventArgs<CrazyEightsPlayer> args)
            {
                Context?.Clients.All.InvokeEventAsync(
                    new CardPlayedHubEventModel(new CrazyEightsPlayerHubModel(args.Player), args.Card));
            }

            public static void OnDiscardColorChanged(
                object? sender,
                DiscardColorChangedEventArgs<CrazyEightsPlayer> args)
            {
                Context?.Clients.All.InvokeEventAsync(new DiscardColorChangedHubEventModel(args.Player, args.Color));
            }

            public static void OnDiscardSuitChanged(
                object? sender,
                DiscardSuitChangedEventArgs<CrazyEightsPlayer> args)
            {
                Context?.Clients.All.InvokeEventAsync(new DiscardSuitChangedHubEventModel(args.Player, args.Suit));
            }

            public static void OnGameStarted(object? sender, EventArgs e)
            {
                // Try to immediately begin the first turn.
                (sender as CrazyEightsGame)?.BeginTurn();
            }

            public static void OnGameTurnEnded(object? sender, GameTurnEventArgs<CrazyEightsPlayer> args)
            {
                // Try to immediately begin the next turn.
                (sender as CrazyEightsGame)?.BeginTurn();
            }

            public static void OnPlayerFinished(object? sender, CrazyEightsPlayer args)
            {
                Context?.Clients.All.InvokeEventAsync(
                    new PlayeFinishedHubEventModel(new CrazyEightsPlayerHubModel(args)));
            }
        }
    }
}
namespace Zaggoware.Games.CardGames.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Zaggoware.Games.Common;

    public class CardPack : IEnumerable<PlayingCard>
    {
        private readonly List<PlayingCard> _cards = new List<PlayingCard>();

        public CardPack(int jokers = 3, bool shuffle = true)
            : this(Array.Empty<CardRank>(), jokers, shuffle)
        {
        }

        public CardPack(CardRank[] excludedRanks, int jokers, bool shuffle)
        {
            Initialize(excludedRanks, jokers);
            if (shuffle)
            {
                _cards = _cards.Randomize().ToList();
            }
        }

        public IEnumerator<PlayingCard> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void Initialize(CardRank[] excludedRanks, int jokers)
        {
            var suits = Enum.GetValues(typeof(CardSuit));
            var ranks = Enum.GetValues(typeof(CardRank));

            foreach (CardSuit suit in suits)
            {
                if (suit == CardSuit.Any)
                {
                    continue;
                }

                foreach (CardRank rank in ranks)
                {
                    if (rank == CardRank.None || rank == CardRank.Joker || excludedRanks.Contains(rank))
                    {
                        continue;
                    }

                    _cards.Add(new PlayingCard(suit, rank));
                }
            }

            if (excludedRanks.Contains(CardRank.Joker))
            {
                return;
            }

            for (var i = 0; i < jokers; i++)
            {
                _cards.Add(new PlayingCard(CardSuit.Any, CardRank.Joker));
            }
        }
    }
}
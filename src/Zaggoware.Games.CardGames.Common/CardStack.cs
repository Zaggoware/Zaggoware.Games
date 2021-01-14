namespace Zaggoware.Games.CardGames.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Zaggoware.Games.Common;

    public class CardStack : IEnumerable<PlayingCard>
    {
        private readonly ICollection<CardRank> _excludedRanks;
        private List<PlayingCard> _cards = new List<PlayingCard>();

        public CardStack(IEnumerable<CardRank>? excludedRanks = null)
        {
            _excludedRanks = excludedRanks?.ToList() ?? new List<CardRank>();
        }

        public virtual int Count => _cards.Count;

        public virtual bool FaceUp { get; set; }

        public virtual void AddCards(IEnumerable<PlayingCard> cards)
        {
            foreach (var card in cards)
            {
                AddCard(card);
            }
        }

        public virtual void AddCard(PlayingCard card)
        {
            if (card.Rank == CardRank.None || (_excludedRanks.Any() && _excludedRanks.Contains(card.Rank)))
            {
                return;
            }

            _cards.Add(card);
        }

        public virtual void Clear()
        {
            _cards.Clear();
        }

        public virtual PlayingCard? DrawCard()
        {
            if (_cards.Count == 0)
            {
                // TODO: What to do?
                return null;
            }

            var card = _cards.Last();
            _cards.RemoveAt(_cards.Count - 1);
            return card;
        }

        public virtual void Shuffle()
        {
            _cards = _cards.Randomize().ToList();
        }

        public virtual IEnumerator<PlayingCard> GetEnumerator()
        {
            return _cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

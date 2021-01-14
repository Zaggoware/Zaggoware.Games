namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public readonly struct PlayingCard : IEquatable<PlayingCard>, IComparable<PlayingCard>
    {
        public PlayingCard(CardSuit suit, CardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        public static PlayingCard Unknown => new PlayingCard(CardSuit.Any, CardRank.None);

        public CardSuit Suit { get; }

        public CardRank Rank { get; }

        public CardColor Color => Suit.GetColor();

        public static bool operator ==(PlayingCard a, PlayingCard b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PlayingCard a, PlayingCard b)
        {
            return !a.Equals(b);
        }

        public static bool operator >(PlayingCard a, PlayingCard b)
        {
            return a.Rank > b.Rank;
        }

        public static bool operator >=(PlayingCard a, PlayingCard b)
        {
            return a.Rank >= b.Rank;
        }

        public static bool operator <(PlayingCard a, PlayingCard b)
        {
            return a.Rank < b.Rank;
        }

        public static bool operator <=(PlayingCard a, PlayingCard b)
        {
            return a.Rank <= b.Rank;
        }

        public int CompareTo(PlayingCard other)
        {
            return new CardComparer().Compare(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is PlayingCard playingCard)
            {
                return Equals(playingCard);
            }

            return base.Equals(obj);
        }

        public bool Equals(PlayingCard other)
        {
            return Suit == other.Suit && Rank == other.Rank;
        }

        public override int GetHashCode()
        {
            return Suit.GetHashCode() & Rank.GetHashCode();
        }

        public bool HasSameColor(PlayingCard otherCard)
        {
            return Color == otherCard.Color;
        }

        public bool HasSameRank(PlayingCard otherCard)
        {
            return Rank == otherCard.Rank;
        }

        public bool HasSameSuit(PlayingCard otherCard)
        {
            return Suit == otherCard.Suit;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}
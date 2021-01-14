namespace Zaggoware.Games.CardGames.Common
{
    using System.Collections.Generic;

    public class CardComparer : IComparer<PlayingCard>
    {
        public int Compare(PlayingCard x, PlayingCard y)
        {
            if (x.Suit == CardSuit.Any)
            {
                if (x.Suit == y.Suit)
                {
                    return 0;
                }

                return -1;
            }

            if (y.Suit == CardSuit.Any)
            {
                return 1;
            }

            return new RankComparer().Compare(x.Rank, y.Rank);
        }
    }
}

namespace Zaggoware.Games.CardGames.Common
{
    using System.Collections.Generic;

    public class RankComparer : IComparer<CardRank>
    {
        public int Compare(CardRank x, CardRank y)
        {
            if (x < y)
            {
                return -1;
            }

            if (x > y)
            {
                return 1;
            }

            return 0;
        }
    }
}

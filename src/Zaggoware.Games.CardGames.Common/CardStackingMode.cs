namespace Zaggoware.Games.CardGames.Common
{
    using System;

    [Flags]
    public enum CardStackingMode
    {
        SameRank = 1,
        SameSuit = 2,
        SameColor = 4,
        Always = 8,
    }
}
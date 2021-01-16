namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public abstract class TurnBasedCardGameRules : CardGameRules, ITurnBasedCardGameRules
    {
        public bool CanChangeTurnDirection { get; set; } = true;

        public TimeSpan MaxTurnDuration { get; set; } = TimeSpan.Zero;

        public int MaxTurns { get; set; }
    }
}
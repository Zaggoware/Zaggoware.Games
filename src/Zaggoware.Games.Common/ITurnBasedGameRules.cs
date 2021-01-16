namespace Zaggoware.Games.Common
{
    using System;

    public interface ITurnBasedGameRules : IGameRules
    {
        bool CanChangeTurnDirection { get; set; }

        TimeSpan MaxTurnDuration { get; set; }

        int MaxTurns { get; set; }
    }
}
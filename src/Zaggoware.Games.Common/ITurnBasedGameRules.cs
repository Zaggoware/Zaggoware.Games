namespace Zaggoware.Games.Common
{
    using System;

    public interface ITurnBasedGameRules : IGameRules
    {
        TimeSpan MaxTurnDuration { get; set; }

        int MaxTurns { get; set; }
    }
}
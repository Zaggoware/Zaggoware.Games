namespace Zaggoware.Games.Common
{
    using System;

    public interface ITurnBasedGame : IGame
    {
        bool CanChangeTurnDirection { get; }

        bool IsTurnStarted { get; }

        IPlayer? CurrentPlayer { get; }

        ClockDirection TurnDirection { get; }

        int TurnIndex { get; }

        DateTime? TurnStartDateTimeUtc { get; }

        bool BeginTurn();

        bool ChangeTurnDirection();

        bool EndTurn();
    }
}
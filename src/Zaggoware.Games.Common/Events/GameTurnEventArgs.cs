namespace Zaggoware.Games.Common.Events
{
    using System;

    public class GameTurnEventArgs<TPlayer> : EventArgs
        where TPlayer : IPlayer
    {
        public GameTurnEventArgs(int turnIndex, TPlayer player, DateTime? turnStartedDateTimeUtc = null)
        {
            TurnIndex = turnIndex;
            Player = player;
            TurnStartedDateTimeUtc = turnStartedDateTimeUtc;
        }

        public int TurnIndex { get; }

        public TPlayer Player { get; }

        public DateTime? TurnStartedDateTimeUtc { get; }
    }
}
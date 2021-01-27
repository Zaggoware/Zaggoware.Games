namespace Zaggoware.Games.Hubs.Models.Events
{
    using System;

    using Zaggoware.Games.Hubs.Enums;

    public class GameTurnBeganHubEventModel : IHubEventModel
    {
        public GameTurnBeganHubEventModel(PlayerHubModel player, int turnIndex, DateTime? turnStartedDateTimeUtc)
        {
            Player = player;
            TurnIndex = turnIndex;
            TurnStartedDateTimeUtc = turnStartedDateTimeUtc;
        }

        public string EventName => TurnBasedGameHubEvents.TurnBegan.ToString();

        public PlayerHubModel Player { get; }

        public int TurnIndex { get; }

        public DateTime? TurnStartedDateTimeUtc { get; }
    }
}
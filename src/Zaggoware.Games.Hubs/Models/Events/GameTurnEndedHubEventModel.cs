﻿namespace Zaggoware.Games.Hubs.Models.Events
{
    using System;

    using Zaggoware.Games.Hubs.Enums;

    public class GameTurnEndedHubEventModel : IHubEventModel
    {
        public GameTurnEndedHubEventModel(PlayerHubModel player, int turnIndex, DateTime? turnStartedDateTimeUtc)
        {
            Player = player;
            TurnIndex = turnIndex;
            TurnStartedDateTimeUtc = turnStartedDateTimeUtc;
        }

        public string EventName => TurnBasedGameHubEvents.TurnEnded.ToString();

        public PlayerHubModel Player { get; }

        public int TurnIndex { get; }

        public DateTime? TurnStartedDateTimeUtc { get; }
    }
}
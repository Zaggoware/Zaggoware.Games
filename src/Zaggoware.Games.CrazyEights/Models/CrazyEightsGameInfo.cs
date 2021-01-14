namespace Zaggoware.Games.CrazyEights.Models
{
    using System;

    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights;
    using Zaggoware.Games.Common;

    public class CrazyEightsGameInfo
    {
        public bool Paused { get; set; }

        public bool Started { get; set; }

        public CrazyEightsPlayerInfo[] Players { get; set; } = Array.Empty<CrazyEightsPlayerInfo>();

        public IGameConnection[] Spectators { get; set; } = Array.Empty<IGameConnection>();

        public CrazyEightsGameRules Rules { get; set; } = new CrazyEightsGameRules();

        public int StockpileCount { get; set; }

        public PlayingCard[] DiscardPile { get; set; } = Array.Empty<PlayingCard>();

        public PlayingCard[] CardsInHand { get; set; } = Array.Empty<PlayingCard>();

        public int TurnIndex { get; set; }

        public string? CurrentPlayerId { get; set; }
    }
}
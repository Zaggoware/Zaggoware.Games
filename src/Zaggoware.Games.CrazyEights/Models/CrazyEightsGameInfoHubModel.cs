namespace Zaggoware.Games.CrazyEights.Models
{
    using System;

    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights;
    using Zaggoware.Games.Hubs.Models;

    public class CrazyEightsGameInfoHubModel : TurnBasedGameInfoHubModel<CrazyEightsGameRules, CrazyEightsPlayerHubModel>
    {
        public int StockpileCount { get; set; }

        public PlayingCard[] DiscardPile { get; set; } = Array.Empty<PlayingCard>();
    }
}
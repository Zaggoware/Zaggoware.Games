namespace Zaggoware.Games.CrazyEights.Models.Events
{
    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.Hubs.Models.Events;

    public class DiscardColorChangedHubEventModel : IHubEventModel
    {
        public DiscardColorChangedHubEventModel(CrazyEightsPlayer player, CardColor color)
        {
            Player = new CrazyEightsPlayerHubModel(player);
            Color = color;
        }

        public CardColor Color { get; }

        public string EventName => CrazyEightsGameHubEvents.DiscardColorChanged.ToString();

        public CrazyEightsPlayerHubModel Player { get; }
    }
}
namespace Zaggoware.Games.CrazyEights.Models.Events
{
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.Hubs.Models.Events;

    public class CardDrawnHubEventModel : IHubEventModel
    {
        public CardDrawnHubEventModel(CrazyEightsPlayerHubModel player)
        {
            Player = player;
        }

        public string EventName => CrazyEightsGameHubEvents.CardDrawn.ToString();

        public CrazyEightsPlayerHubModel Player { get; }
    }
}
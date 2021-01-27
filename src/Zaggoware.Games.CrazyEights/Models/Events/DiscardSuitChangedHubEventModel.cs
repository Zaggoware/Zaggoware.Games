namespace Zaggoware.Games.CrazyEights.Models.Events
{
    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.Hubs.Models.Events;

    public class DiscardSuitChangedHubEventModel : IHubEventModel
    {
        public DiscardSuitChangedHubEventModel(CrazyEightsPlayer player, CardSuit suit)
        {
            Player = new CrazyEightsPlayerHubModel(player);
            Suit = suit;
        }

        public string EventName => CrazyEightsGameHubEvents.DiscardSuitChanged.ToString();

        public CrazyEightsPlayerHubModel Player { get; }

        public CardSuit Suit { get; }
    }
}
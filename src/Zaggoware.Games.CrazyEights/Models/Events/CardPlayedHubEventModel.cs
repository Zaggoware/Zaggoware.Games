namespace Zaggoware.Games.CrazyEights.Models.Events
{
    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.Hubs.Models.Events;

    public class CardPlayedHubEventModel : IHubEventModel
    {
        public CardPlayedHubEventModel(CrazyEightsPlayerHubModel player, PlayingCard card)
        {
            Player = player;
            Card = card;
        }

        public PlayingCard Card { get; }

        public string EventName => CrazyEightsGameHubEvents.CardPlayed.ToString();

        public CrazyEightsPlayerHubModel Player { get; }
    }
}
namespace Zaggoware.Games.CrazyEights.Models.Events
{
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.Hubs.Models.Events;

    public class PlayeFinishedHubEventModel : IHubEventModel
    {
        public PlayeFinishedHubEventModel(CrazyEightsPlayerHubModel player)
        {
            Player = player;
        }

        public string EventName => CrazyEightsGameHubEvents.PlayerFinished.ToString();

        public CrazyEightsPlayerHubModel Player { get; }
    }
}
namespace Zaggoware.Games.Hubs.Models.Events
{
    using Zaggoware.Games.Hubs.Enums;

    public class SpectatorRemovedHubEventModel : IHubEventModel
    {
        public SpectatorRemovedHubEventModel(GameConnectionHubModel connection)
        {
            Connection = connection;
        }

        public GameConnectionHubModel Connection { get; set; }

        public string EventName => GameHubEvents.SpectatorRemoved.ToString();
    }
}
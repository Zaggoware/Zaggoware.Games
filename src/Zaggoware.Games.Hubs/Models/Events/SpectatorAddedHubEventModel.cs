namespace Zaggoware.Games.Hubs.Models.Events
{
    using Zaggoware.Games.Hubs.Enums;

    public class SpectatorAddedHubEventModel : IHubEventModel
    {
        public SpectatorAddedHubEventModel(GameConnectionHubModel connection)
        {
            Connection = connection;
        }

        public GameConnectionHubModel Connection { get; }

        public string EventName => GameHubEvents.SpectatorAdded.ToString();
    }
}
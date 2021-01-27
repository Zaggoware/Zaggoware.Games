namespace Zaggoware.Games.Hubs.Models.Events
{
    public class UserDisconnectedHubEventModel : IHubEventModel
    {
        public UserDisconnectedHubEventModel(GameConnectionHubModel connection)
        {
            Connection = connection;
        }

        public GameConnectionHubModel Connection { get; }

        // TODO: GameLobbyHubEvents enum ???
        public string EventName => "UserDisconnected";
    }
}
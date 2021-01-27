namespace Zaggoware.Games.Hubs.Models.Events
{
    public class UserConnectedHubEventModel : IHubEventModel
    {
        public UserConnectedHubEventModel(GameConnectionHubModel connection)
        {
            Connection = connection;
        }

        public GameConnectionHubModel Connection { get; }

        // TODO: GameLobbyHubEvents enum ???
        public string EventName => "UserConnected";
    }
}
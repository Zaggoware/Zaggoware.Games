namespace Zaggoware.Games.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class GameHubContext
    {
        public GameHubContext(IHubClients clients, IGroupManager groups)
        {
            Clients = clients;
            Groups = groups;
        }

        public IHubClients Clients { get; }

        public IGroupManager Groups { get; }
    }
}
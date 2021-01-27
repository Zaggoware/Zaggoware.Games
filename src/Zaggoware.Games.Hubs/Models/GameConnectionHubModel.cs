namespace Zaggoware.Games.Hubs.Models
{
    using Zaggoware.Games.Common;

    public class GameConnectionHubModel
    {
        public GameConnectionHubModel(IGameConnection connection)
        {
            Id = connection.Id;
            Name = connection.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
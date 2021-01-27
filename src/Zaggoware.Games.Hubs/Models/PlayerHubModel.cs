namespace Zaggoware.Games.Hubs.Models
{
    using Zaggoware.Games.Common;

    public class PlayerHubModel
    {
        public PlayerHubModel()
        {
        }

        public PlayerHubModel(IPlayer player)
        {
            Connection = player.Connection;
            Name = player.Name;
        }

        public IGameConnection Connection { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
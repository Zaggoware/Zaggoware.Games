namespace Zaggoware.Games.Hubs.Models.Events
{
    using Zaggoware.Games.Hubs.Enums;

    public class PlayerRemovedHubEventModel<TPlayerModel> : IHubEventModel
        where TPlayerModel : PlayerHubModel
    {
        public PlayerRemovedHubEventModel(TPlayerModel player)
        {
            Player = player;
        }

        public string EventName => GameHubEvents.PlayerRemoved.ToString();

        public TPlayerModel Player { get; }
    }
}
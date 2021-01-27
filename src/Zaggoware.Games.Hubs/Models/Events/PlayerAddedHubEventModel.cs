namespace Zaggoware.Games.Hubs.Models.Events
{
    using Zaggoware.Games.Hubs.Enums;

    public class PlayerAddedHubEventModel<TPlayerModel> : IHubEventModel
        where TPlayerModel : PlayerHubModel
    {
        public PlayerAddedHubEventModel(TPlayerModel player)
        {
            Player = player;
        }

        public string EventName => GameHubEvents.PlayerAdded.ToString();

        public TPlayerModel Player { get; }
    }
}
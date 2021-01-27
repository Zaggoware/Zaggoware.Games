namespace Zaggoware.Games.Hubs.Models.Events
{
    using Zaggoware.Games.Common;
    using Zaggoware.Games.Hubs.Enums;

    public class GameStoppedHubEventModel<TGameRules, TPlayerModel> : IHubEventModel
        where TGameRules : class, IGameRules
        where TPlayerModel : PlayerHubModel
    {
        public GameStoppedHubEventModel(GameInfoHubModel<TGameRules, TPlayerModel> gameInfo)
        {
            GameInfo = gameInfo;
        }

        public string EventName => GameHubEvents.GameStopped.ToString();

        public GameInfoHubModel<TGameRules, TPlayerModel> GameInfo { get; }
    }
}
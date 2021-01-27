namespace Zaggoware.Games.Hubs.Models.Events
{
    using Zaggoware.Games.Common;
    using Zaggoware.Games.Hubs.Enums;

    public class GameStartedHubEventModel<TGameInfoModel, TGameRules, TPlayerModel> : IHubEventModel
        where TGameInfoModel : GameInfoHubModel<TGameRules, TPlayerModel>
        where TGameRules : class, IGameRules
        where TPlayerModel : PlayerHubModel
    {
        public GameStartedHubEventModel(TGameInfoModel gameInfo)
        {
            GameInfo = gameInfo;
        }

        public string EventName => GameHubEvents.GameStarted.ToString();

        public TGameInfoModel GameInfo { get; }
    }
}
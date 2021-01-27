namespace Zaggoware.Games.Hubs.Models
{
    using Zaggoware.Games.Common;

    public abstract class GameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel>
        where TGameRules : class, IGameRules
        where TPlayer : class, IPlayer
        where TPlayerModel : PlayerHubModel
        where TGame : class, IGame
        where TGameInfoModel : GameInfoHubModel<TGameRules, TPlayerModel>
    {
        public abstract TGameInfoModel CreateGameInfoModel(TGame game);

        public abstract TPlayerModel CreatePlayerModel(TPlayer player);
    }
}
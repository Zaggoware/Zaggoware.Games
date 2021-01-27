namespace Zaggoware.Games.Hubs.Models
{
    using Zaggoware.Games.Common;

    public abstract class TurnBasedGameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel>
        : GameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel>
        where TGameRules : class, ITurnBasedGameRules
        where TPlayer : class, IPlayer
        where TPlayerModel : PlayerHubModel
        where TGame : class, ITurnBasedGame
        where TGameInfoModel : TurnBasedGameInfoHubModel<TGameRules, TPlayerModel>
    {
    }
}
namespace Zaggoware.Games.Hubs.Models
{
    using Zaggoware.Games.Common;

    public class TurnBasedGameInfoHubModel<TGameRules, TPlayerModel> : GameInfoHubModel<TGameRules, TPlayerModel>
        where TGameRules : class, ITurnBasedGameRules
        where TPlayerModel : PlayerHubModel
    {
        public int TurnIndex { get; set; }

        public TPlayerModel? CurrentPlayer { get; set; }
    }
}
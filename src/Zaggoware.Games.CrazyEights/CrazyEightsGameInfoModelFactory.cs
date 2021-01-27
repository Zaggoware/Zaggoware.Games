namespace Zaggoware.Games.CrazyEights
{
    using System.Linq;

    using Zaggoware.Games.CrazyEights.Models;
    using Zaggoware.Games.Hubs.Models;

    public class CrazyEightsGameInfoModelFactory : TurnBasedGameInfoHubModelFactory<CrazyEightsGame,
        CrazyEightsGameInfoHubModel, CrazyEightsGameRules, CrazyEightsPlayer, CrazyEightsPlayerHubModel>
    {
        public override CrazyEightsGameInfoHubModel CreateGameInfoModel(CrazyEightsGame game)
        {
            var players = game.GetPlayers();
            var gameInfo = new CrazyEightsGameInfoHubModel
            {
                IsPaused = game.IsPaused,
                IsStarted = game.IsStarted,
                Players = players
                    .Select(p => new CrazyEightsPlayerHubModel(p))
                    .ToArray(),
                Spectators = game.GetSpectators()
                    .Select(s => new GameConnectionHubModel(s))
                    .ToArray(),
                Rules = game.Rules,
                StockpileCount = game.Stockpile.Count,
                DiscardPile = game.DiscardPile.ToArray(),
                TurnIndex = game.TurnIndex,
                CurrentPlayer = game.CurrentPlayer != null
                    ? new CrazyEightsPlayerHubModel(game.CurrentPlayer)
                    : null
            };
            return gameInfo;
        }

        public override CrazyEightsPlayerHubModel CreatePlayerModel(CrazyEightsPlayer player)
        {
            return new CrazyEightsPlayerHubModel(player);
        }
    }
}
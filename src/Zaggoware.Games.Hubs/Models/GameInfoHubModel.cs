namespace Zaggoware.Games.Hubs.Models
{
    using System;

    using Zaggoware.Games.Common;

    public abstract class GameInfoHubModel<TGameRules, TPlayerModel>
        where TGameRules : class, IGameRules
        where TPlayerModel : PlayerHubModel
    {
        public bool IsPaused { get; set; }

        public bool IsStarted { get; set; }

        public TPlayerModel[] Players { get; set; } = Array.Empty<TPlayerModel>();

        public TGameRules Rules { get; set; } = null!;

        public GameConnectionHubModel[] Spectators { get; set; } = Array.Empty<GameConnectionHubModel>();
    }
}
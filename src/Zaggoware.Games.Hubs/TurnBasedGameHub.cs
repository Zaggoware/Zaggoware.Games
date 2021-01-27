namespace Zaggoware.Games.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    using Zaggoware.Games.Common;
    using Zaggoware.Games.Common.Events;
    using Zaggoware.Games.Hubs.Enums;
    using Zaggoware.Games.Hubs.Extensions;
    using Zaggoware.Games.Hubs.Models;
    using Zaggoware.Games.Hubs.Models.Events;

    public abstract class TurnBasedGameHub<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel> : GameHub<TGame,
        TGameInfoModel, TGameRules, TPlayer, TPlayerModel>
        where TGameRules : class, ITurnBasedGameRules
        where TPlayer : class, IPlayer
        where TPlayerModel : PlayerHubModel
        where TGame : TurnBasedGameBase<TGameRules, TPlayer>
        where TGameInfoModel : TurnBasedGameInfoHubModel<TGameRules, TPlayerModel>
    {
        protected TurnBasedGameHub(
            GameHubContext context,
            TurnBasedGameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel> modelFactory)
            : base(context, modelFactory)
        {
            // TODO: Find a better and cleaner way to handle this.
            EventHandlers.Context ??= context;
        }

        protected new TurnBasedGameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel>
            ModelFactory
        {
            get => (TurnBasedGameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel>)
                base.ModelFactory;
            set => base.ModelFactory = value;
        }

        protected virtual bool IsCurrentPlayer => Context.ConnectionId == CurrentGame?.CurrentPlayer?.Connection.Id;

        [HubMethodName(nameof(TurnBasedGameHubActions.BeginTurn))]
        public virtual bool BeginTurn()
        {
            return IsCurrentPlayer && (CurrentGame?.BeginTurn() ?? false);
        }

        [HubMethodName(nameof(TurnBasedGameHubActions.EndTurn))]
        public virtual bool EndTurn()
        {
            return IsCurrentPlayer && (CurrentGame?.EndTurn() ?? false);
        }

        protected override void OnGameCreated(TGame game)
        {
            base.OnGameCreated(game);

            game.TurnBegan += EventHandlers.OnGameTurnBegan;
            game.TurnEnded += EventHandlers.OnGameTurnEnded;
        }

        private class EventHandlers
        {
            public static GameHubContext? Context { get; set; }

            public static void OnGameTurnBegan(object? sender, GameTurnEventArgs<TPlayer> e)
            {
                Context?.Clients.All.InvokeEventAsync(
                    new GameTurnBeganHubEventModel(
                        new PlayerHubModel(e.Player),
                        e.TurnIndex,
                        e.TurnStartedDateTimeUtc));
            }

            public static void OnGameTurnEnded(object? sender, GameTurnEventArgs<TPlayer> e)
            {
                Context?.Clients.All.InvokeEventAsync(
                    new GameTurnEndedHubEventModel(
                        new PlayerHubModel(e.Player),
                        e.TurnIndex,
                        e.TurnStartedDateTimeUtc));
            }
        }
    }
}
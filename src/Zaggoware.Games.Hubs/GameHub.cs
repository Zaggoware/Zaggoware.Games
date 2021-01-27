namespace Zaggoware.Games.Hubs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Zaggoware.Games.Common;
    using Zaggoware.Games.Hubs.Enums;
    using Zaggoware.Games.Hubs.Extensions;
    using Zaggoware.Games.Hubs.Models;
    using Zaggoware.Games.Hubs.Models.Events;

    public abstract class GameHub<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel> : Hub
        where TGameRules : class, IGameRules
        where TPlayer : class, IPlayer
        where TPlayerModel : PlayerHubModel
        where TGame : GameBase<TGameRules, TPlayer>
        where TGameInfoModel : GameInfoHubModel<TGameRules, TPlayerModel>
    {
        private string? _gameId;
        private TGame? _game;

        protected GameHub(
            GameHubContext context,
            GameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel> modelFactory)
        {
            ModelFactory = modelFactory;

            // TODO: Find a better and cleaner way to handle this.
            EventHandlers.Context ??= context;
            EventHandlers.ModelFactory ??= modelFactory;
        }

        protected GameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel> ModelFactory
        {
            get;
            set;
        }

        protected virtual TGame? CurrentGame =>
            _game ??= GameStateManager.Instance.Games.Contains(GameId ?? string.Empty)
                ? GameStateManager.Instance.Games[GameId ?? string.Empty] as TGame
                : null;

        protected virtual string? GameId => _gameId ??= Context.GetHttpContext().Request.Query["gameId"];

        [HubMethodName(nameof(GameHubActions.ConnectUser))]
        public virtual async Task ConnectUser(string name)
        {
            // TODO: Move logic to GameLobbyHub?
            var game = CurrentGame;
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }

            bool result;
            var user = new GameUser(Context.ConnectionId, name);
            if (game.IsStarted || game.GetPlayers().Length >= game.Rules.MaxPlayers)
            {
                result = game.AddSpectator(user);
            }
            else
            {
                var player = CreatePlayer(user);
                result = game.AddPlayer(player);
            }

            if (result)
            {
                await Clients.All.InvokeEventAsync(new UserConnectedHubEventModel(new GameConnectionHubModel(user)));
            }
        }

        [HubMethodName(nameof(GameHubActions.FetchGameInfo))]
        public virtual TGameInfoModel? FetchGameInfo()
        {
            if (CurrentGame == null)
            {
                return null;
            }

            var model = ModelFactory.CreateGameInfoModel(CurrentGame);
            return model;
        }

        [HubMethodName(nameof(GameHubActions.StartGame))]
        public virtual bool StartGame()
        {
            // TODO: Return a GameHubResult<bool> object containing the game info for the calling player.
            return CurrentGame?.Start() ?? false;
        }

        public override Task OnConnectedAsync()
        {
            var game = CurrentGame;
            if (game != null)
            {
                return base.OnConnectedAsync();
            }

            // TODO: Logic in Lobby?
            game = CreateGame();
            GameStateManager.Instance.Games.Add(game);
            OnGameCreated(game);

            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            var game = CurrentGame;
            if (game == null)
            {
                return;
            }

            var player = game.GetPlayers().SingleOrDefault(p => p.Connection.Id == Context.ConnectionId);
            if (player != null)
            {
                game.RemovePlayer(player);
            }

            var spectator = game.GetSpectators().SingleOrDefault(p => p.Id == Context.ConnectionId);
            if (spectator != null)
            {
                game.RemoveSpectator(spectator);
            }

            if (player != null || spectator != null)
            {
                var connection = player?.Connection ?? spectator;
                await Clients.All.InvokeEventAsync(
                    new UserDisconnectedHubEventModel(new GameConnectionHubModel(connection!)));
            }
        }

        protected abstract TGame CreateGame();

        protected abstract TPlayer CreatePlayer(IGameConnection connection);

        protected virtual IGameConnection? GetConnection()
        {
            return GameStateManager.Instance.Connections.Contains(Context.ConnectionId)
                ? GameStateManager.Instance.Connections[Context.ConnectionId]
                : null;
        }

        protected virtual void OnGameCreated(TGame game)
        {
            game.Started += EventHandlers.OnGameStarted;
            game.Stopped += EventHandlers.OnGameStopped;
            game.PlayerAdded += EventHandlers.OnGamePlayerAdded;
            game.PlayerRemoved += EventHandlers.OnGamePlayerRemoved;
            game.SpectatorAdded += EventHandlers.OnGameSpectatorAdded;
            game.SpectatorRemoved += EventHandlers.OnGameSpectatorRemoved;
        }

        private static class EventHandlers
        {
            public static GameHubContext? Context { get; set; }

            public static GameInfoHubModelFactory<TGame, TGameInfoModel, TGameRules, TPlayer, TPlayerModel>?
                ModelFactory { get; set; }

            public static void OnGamePlayerAdded(object? sender, TPlayer args)
            {
                var playerModel = ModelFactory?.CreatePlayerModel(args);
                Context?.Clients.All.InvokeEventAsync(new PlayerAddedHubEventModel<TPlayerModel>(playerModel!));
            }

            public static void OnGamePlayerRemoved(object? sender, TPlayer args)
            {
                var playerModel = ModelFactory?.CreatePlayerModel(args);
                Context?.Clients.All.InvokeEventAsync(new PlayerRemovedHubEventModel<TPlayerModel>(playerModel!));
            }

            public static void OnGameSpectatorAdded(object? sender, IGameConnection args)
            {
                Context?.Clients.All.InvokeEventAsync(
                    new SpectatorAddedHubEventModel(new GameConnectionHubModel(args)));
            }

            public static void OnGameSpectatorRemoved(object? sender, IGameConnection args)
            {
                Context?.Clients.All.InvokeEventAsync(
                    new SpectatorRemovedHubEventModel(new GameConnectionHubModel(args)));
            }

            public static void OnGameStarted(object? sender, EventArgs args)
            {
                if (!(sender is TGame game) || ModelFactory == null)
                {
                    return;
                }

                // TODO: Remove callerConnectionId.
                var gameInfoModel = ModelFactory.CreateGameInfoModel(game);
                Context?.Clients.All.InvokeEventAsync(
                    new GameStartedHubEventModel<TGameInfoModel, TGameRules, TPlayerModel>(gameInfoModel));
            }

            public static void OnGameStopped(object? sender, EventArgs args)
            {
                if (!(sender is TGame game) || ModelFactory == null)
                {
                    return;
                }

                var model = ModelFactory.CreateGameInfoModel(game);
                Context?.Clients.All.InvokeEventAsync(new GameStoppedHubEventModel<TGameRules, TPlayerModel>(model));
            }
        }
    }
}
namespace Zaggoware.Games.CrazyEights
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.CrazyEights.Models;
    using Zaggoware.Games.Common;
    using Zaggoware.Games.Common.Events;
    using Zaggoware.Games.CrazyEights.Enums;
    using Zaggoware.Games.Hubs;
    using Zaggoware.Games.Hubs.Extensions;

    public class CrazyEightsGameHub : GameHub
    {
        public CrazyEightsGameHub(IHubContext<CrazyEightsGameHub> context)
        {
            EventHandlers.Context ??= context;
        }

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connection established.");

            var game = GetGame();
            if (game != null)
            {
                return base.OnConnectedAsync();
            }

            // TODO: Logic in Lobby.
            game = new CrazyEightsGame(CrazyEightsGameRules.Create("Pesten"));
            game.Started += EventHandlers.OnGameStarted;
            game.Stopped += EventHandlers.OnGameStopped;
            game.PlayerAdded += EventHandlers.OnGamePlayerAdded;
            game.PlayerRemoved += EventHandlers.OnGamePlayerRemoved;
            game.SpectatorAdded += EventHandlers.OnGameSpectatorAdded;
            game.SpectatorRemoved += EventHandlers.OnGameSpectatorRemoved;
            game.TurnBegan += EventHandlers.OnGameTurnBegan;
            game.TurnEnded += EventHandlers.OnGameTurnEnded;
            game.CardPlayed += EventHandlers.OnGameCardPlayed;
            game.CardDrawn += EventHandlers.OnGameCardDrawn;
            game.PlayerFinished += EventHandlers.OnGamePlayerFinished;
            game.ColorChanged += EventHandlers.OnGameColorChanged;
            game.SuitChanged += EventHandlers.OnGameSuitChanged;
            GameStateManager.Instance.Games.Add(game);

            return base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            var httpContext = Context.GetHttpContext();
            var gameId = httpContext.Request.Query["gameId"];
            if (string.IsNullOrWhiteSpace(gameId))
            {
                throw new InvalidOperationException("Game ID cannot be empty.");
            }

            var game = GetGame();
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
                var id = player?.Connection.Id ?? spectator?.Id ?? string.Empty;
                var name = player?.Name ?? spectator?.Name ?? string.Empty;

                await Clients.All.SendAsync(
                    "UserDisconnected",
                    new GameUser(id, name));
            }
        }

        public async Task ConnectUser(string name)
        {
            var game = GetGame();
            if (game == null)
            {
                throw new InvalidOperationException("Game not found.");
            }

            var user = new GameUser(Context.ConnectionId, name);
            await Clients.All.SendAsync("UserConnected", user);

            if (game.IsStarted || game.GetPlayers().Length >= game.Rules.MaxPlayers)
            {
                game.AddSpectator(user);
            }
            else
            {
                var player = new CrazyEightsPlayer(user);
                game.AddPlayer(player);
            }
        }

        public bool ChangeColor(CardColor color )
        {
            var game = GetGame();
            return IsCurrentPlayer() && (game?.ChangeColor(color) ?? false);
        }

        public bool ChangeSuit(CardSuit suit)
        {
            var game = GetGame();
            return IsCurrentPlayer() && (game?.ChangeSuit(suit) ?? false);
        }

        public PlayingCard? DrawCard()
        {
            var game = GetGame();
            return IsCurrentPlayer() ? game?.DrawCard() : null;
        }

        public bool EndTurn()
        {
            var game = GetGame();
            return IsCurrentPlayer() && (game?.EndTurn() ?? false);
        }

        public CrazyEightsGameInfo FetchGameInfo()
        {
            var game = GetGame();
            var players = game!.GetPlayers();

            var gameInfo = new CrazyEightsGameInfo
            {
                Paused = game.IsPaused,
                Started = game.IsStarted,
                Players = players
                    .Select(p => new CrazyEightsPlayerInfo(p))
                    .ToArray(),
                Spectators = game.GetSpectators(),
                Rules = game.Rules,
                StockpileCount = game.Stockpile.Count,
                DiscardPile = game.DiscardPile.ToArray(),
                TurnIndex = game.TurnIndex,
                CurrentPlayerId = game.CurrentPlayer?.Connection.Id,
                CardsInHand = players.FirstOrDefault(p => p.Connection.Id == Context.ConnectionId)?.Hand.ToArray()
                    ?? Array.Empty<PlayingCard>()
            };

            var player = players.SingleOrDefault(p => p.Connection.Id == Context.ConnectionId);
            if (player != null)
            {
                gameInfo.CardsInHand = player.Hand.ToArray();
            }

            return gameInfo;
        }

        public bool PlayCard(int index, CardSuit suit, CardRank rank)
        {
            if (!IsCurrentPlayer())
            {
                return false;
            }

            var game = GetGame();
            var card = game!.CurrentPlayer!.Hand.ElementAtOrDefault(index);
            if (card.Suit != suit || card.Rank != rank)
            {
                card = game.CurrentPlayer!.Hand.FirstOrDefault(c => c.Suit == suit && c.Rank == rank)!;
                if (card == PlayingCard.Unknown)
                {
                    return false;
                }
            }

            return game.PlayCard(card);
        }

        public bool StartGame()
        {
            var game = GetGame();
            if (game!.Start())
            {
                return game.BeginTurn();
            }

            return false;
        }

        private bool IsCurrentPlayer()
        {
            return Context.ConnectionId == GetGame()?.CurrentPlayer?.Connection.Id;
        }

        private IGameConnection? GetConnection()
        {
            return GameStateManager.Instance.Connections.Contains(Context.ConnectionId)
                ? GameStateManager.Instance.Connections[Context.ConnectionId]
                : null;
        }

        private CrazyEightsGame? GetGame()
        {
            var gameId = GetGameId();
            if (string.IsNullOrEmpty(gameId))
            {
                return null;
            }

            return GameStateManager.Instance.Games.Contains(gameId)
                ? GameStateManager.Instance.Games[gameId] as CrazyEightsGame
                : null;
        }

        private string GetGameId()
        {
            return Context.GetHttpContext().Request.Query["gameId"];
        }

        private static class EventHandlers
        {
            internal static IHubContext<CrazyEightsGameHub>? Context { get; set; }

            internal static void OnGameCardDrawn(object? sender, CardDrawnEventArgs<CrazyEightsPlayer> e)
            {
                // Don't return the actual card as we don't want to broadcast to everyone what the card was.
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.CardDrawn.ToString(),
                    new CrazyEightsPlayerInfo(e.Player));
            }

            internal static void OnGameCardPlayed(object? sender, CardPlayedEventArgs<CrazyEightsPlayer> e)
            {
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.CardPlayed.ToString(),
                    new CrazyEightsPlayerInfo(e.Player),
                    e.Card);
            }

            internal static void OnGameColorChanged(
                object? sender,
                CardColorChangedEventArgs<CrazyEightsPlayer> args)
            {
                // TODO: Automapper EventArgs -> EventModel
                Context?.Clients.All.InvokeEventAsync(
                    CrazyEightsGameEvents.ColorChanged,
                    new CrazyEightsColorChangedHubEventModel(args.Player, args.Color));
            }

            internal static void OnGameSuitChanged(
                object? sender,
                CardSuitChangedEventArgs<CrazyEightsPlayer> args)
            {
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.SuitChanged.ToString(),
                    new CrazyEightsSuitChangedHubEventModel(args.Player, args.Suit));
            }

            internal static void OnGamePlayerAdded(object? sender, CrazyEightsPlayer args)
            {
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.PlayerAdded.ToString(),
                    new CrazyEightsPlayerInfo(args));
            }

            internal static void OnGamePlayerFinished(object? sender, CrazyEightsPlayer args)
            {
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.PlayerFinished.ToString(),
                    new CrazyEightsPlayerInfo(args));
            }

            internal static void OnGamePlayerRemoved(object? sender, CrazyEightsPlayer args)
            {
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.PlayerRemoved.ToString(),
                    new CrazyEightsPlayerInfo(args));
            }

            internal static void OnGameSpectatorAdded(object? sender, IGameConnection args)
            {
                Context?.Clients.All.SendAsync(CrazyEightsGameEvents.SpectatorAdded.ToString(), args);
            }

            internal static void OnGameSpectatorRemoved(object? sender, IGameConnection e)
            {
                Context?.Clients.All.SendAsync(CrazyEightsGameEvents.SpectatorRemoved.ToString(), e);
            }

            internal static void OnGameStarted(object? sender, EventArgs e)
            {
                Context?.Clients.All.SendAsync(CrazyEightsGameEvents.GameStarted.ToString());
            }

            internal static void OnGameStopped(object? sender, EventArgs e)
            {
                Context?.Clients.All.SendAsync(CrazyEightsGameEvents.GameStopped.ToString());
            }

            internal static void OnGameTurnBegan(object? sender, GameTurnEventArgs<CrazyEightsPlayer> e)
            {
                Context?.Clients.All.SendAsync(
                    CrazyEightsGameEvents.TurnBegan.ToString(),
                    new CrazyEightsPlayerInfo(e.Player),
                    e.TurnIndex,
                    e.TurnStartedDateTimeUtc);
            }

            internal static async void OnGameTurnEnded(object? sender, GameTurnEventArgs<CrazyEightsPlayer> e)
            {
                if (Context == null)
                {
                    return;
                }

                await Context.Clients.All.SendAsync(
                    CrazyEightsGameEvents.TurnEnded.ToString(),
                    new CrazyEightsPlayerInfo(e.Player),
                    e.TurnIndex,
                    e.TurnStartedDateTimeUtc);

                // Try to immediately begin the next turn.
                (sender as ITurnBasedGame)?.BeginTurn();
            }
        }
    }
}
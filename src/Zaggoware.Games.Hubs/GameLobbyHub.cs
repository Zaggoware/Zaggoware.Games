namespace Zaggoware.Games.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Primitives;

    using Zaggoware.Games.Common;

    public class GameLobbyHub : Hub
    {
        public string CreateLobby(string name, string? password = null, string? userDisplayName = null)
        {
            // TODO: Implement identity.
            var user = new GameUser(
                Context.UserIdentifier ?? Context.ConnectionId,
                Context.User.Identity?.Name ?? userDisplayName ?? Context.ConnectionId);

            var lobby = GameLobby.Create(name, password);
            lobby.AddConnection(user);
            GameLobbyManager.Instance.AddLobby(lobby);
            return lobby.Id;
        }

        public void SetupGame(string lobbyId, string gameType, IDictionary<string, object> rules)
        {
            if (!TryGetLobby(lobbyId, Context.UserIdentifier ?? Context.ConnectionId, out var lobby))
            {
                return;
            }

            // TODO: Setup game.
        }

        private static bool TryGetLobby(string lobbyId, string connectionId, out IGameLobby? lobby)
        {
            lobby = null;
            if (!string.IsNullOrWhiteSpace(lobbyId)
                && GameLobbyManager.Instance.ContainsLobby(lobbyId!)
                && GameLobbyManager.Instance[lobbyId]!.ActiveConnections.Any(c => c.Id == connectionId))
            {
                lobby = GameLobbyManager.Instance[lobbyId];
            }

            return lobby != null;
        }
    }
}
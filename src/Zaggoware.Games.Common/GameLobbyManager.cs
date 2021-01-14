namespace Zaggoware.Games.Common
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class GameLobbyManager
    {
        private static GameLobbyManager? _instance;
        private readonly ConcurrentDictionary<string, IGameLobby> _activeLobbies
            = new ConcurrentDictionary<string, IGameLobby>();

        private GameLobbyManager()
        {
        }

        public static GameLobbyManager Instance => _instance ??= new GameLobbyManager();

        public IGameLobby? this[string lobbyId] => _activeLobbies[lobbyId];

        public IGameLobby[] ActiveLobbies => _activeLobbies.Values.ToArray();

        public void AddLobby(IGameLobby lobby)
        {
            _activeLobbies.TryAdd(lobby.Id, lobby);
        }

        public bool ContainsLobby(string lobbyId)
        {
            return _activeLobbies.ContainsKey(lobbyId);
        }

        public bool ContainsLobby(IGameLobby lobby)
        {
            return _activeLobbies.ContainsKey(lobby.Id) && _activeLobbies[lobby.Id] == lobby;
        }

        public void RemoveLobby(IGameLobby lobby)
        {
            if (!_activeLobbies.ContainsKey(lobby.Id))
            {
                return;
            }

            _activeLobbies.TryRemove(new KeyValuePair<string, IGameLobby>(lobby.Id, lobby));
        }
    }
}
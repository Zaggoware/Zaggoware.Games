namespace Zaggoware.Games.Common.Collections
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class GameCollection : IGameCollection
    {
        private readonly ConcurrentDictionary<string, IGame> _dictionary = new ConcurrentDictionary<string, IGame>();

        public IGame this[string gameId] => _dictionary[gameId];

        public void Add(IGame game)
        {
            if (!_dictionary.ContainsKey(game.Id))
            {
                _dictionary.TryAdd(game.Id, game);
            }
        }

        public bool Contains(string gameId)
        {
            return _dictionary.ContainsKey(gameId);
        }

        public bool Contains(IGame game)
        {
            return _dictionary.ContainsKey(game.Id);
        }

        public IEnumerator<IGame> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        public void Remove(IGame game)
        {
            _dictionary.TryRemove(game.Id, out _);
        }

        public void Remove(string gameId)
        {
            _dictionary.TryRemove(gameId, out _);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
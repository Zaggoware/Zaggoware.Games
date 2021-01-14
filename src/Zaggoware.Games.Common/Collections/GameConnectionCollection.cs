namespace Zaggoware.Games.Common.Collections
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class GameConnectionCollection : IGameConnectionCollection
    {
        private readonly ConcurrentDictionary<string, IGameConnection> _dictionary = new ConcurrentDictionary<string, IGameConnection>();

        public IGameConnection this[string connectionId] => _dictionary[connectionId];

        public void Add(IGameConnection connection)
        {
            if (!_dictionary.ContainsKey(connection.Id))
            {
                _dictionary.TryAdd(connection.Id, connection);
            }
        }

        public bool Contains(string connectionId)
        {
            return _dictionary.ContainsKey(connectionId);
        }

        public bool Contains(IGameConnection connection)
        {
            return _dictionary.ContainsKey(connection.Id);
        }

        public IEnumerator<IGameConnection> GetEnumerator()
        {
            return _dictionary.Values.GetEnumerator();
        }

        public void Remove(IGameConnection connection)
        {
            _dictionary.TryRemove(connection.Id, out _);
        }

        public void Remove(string connectionId)
        {
            _dictionary.TryRemove(connectionId, out _);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
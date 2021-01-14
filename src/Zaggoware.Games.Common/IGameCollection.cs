namespace Zaggoware.Games.Common
{
    using System.Collections.Generic;

    public interface IGameCollection : IEnumerable<IGame>
    {
        IGame this[string gameId] { get; }

        void Add(IGame game);

        bool Contains(string gameId);

        bool Contains(IGame game);

        void Remove(IGame game);

        void Remove(string gameId);
    }
}
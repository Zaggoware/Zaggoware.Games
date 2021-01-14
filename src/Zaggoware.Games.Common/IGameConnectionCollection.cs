namespace Zaggoware.Games.Common
{
    using System.Collections.Generic;

    public interface IGameConnectionCollection : IEnumerable<IGameConnection>
    {
        IGameConnection this[string connectionId] { get; }

        void Add(IGameConnection connection);

        bool Contains(string connectionId);
        
        bool Contains(IGameConnection connection);

        void Remove(IGameConnection connection);

        void Remove(string connectionId);
    }
}
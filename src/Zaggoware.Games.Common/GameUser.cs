namespace Zaggoware.Games.Common
{
    using System;

    public readonly struct GameUser : IGameConnection, IEquatable<IGameConnection>, IComparable<IGameConnection>
    {
        public GameUser(string id, string name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Id { get; }

        public string Name { get; }

        public int CompareTo(IGameConnection? other)
        {
            return new GameUserComparer().Compare(this, other);
        }

        public bool Equals(IGameConnection? other)
        {
            return Id.Equals(other?.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
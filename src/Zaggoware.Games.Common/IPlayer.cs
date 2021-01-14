namespace Zaggoware.Games.Common
{
    using System;

    public interface IPlayer : IEquatable<IPlayer>, IComparable<IPlayer>
    {
        IGameConnection Connection { get; }

        string Name { get; }
    }
}
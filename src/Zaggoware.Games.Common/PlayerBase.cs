namespace Zaggoware.Games.Common
{
    public abstract class PlayerBase : IPlayer
    {
        protected PlayerBase(IGameConnection connection)
        {
            Connection = connection;
        }
        
        public IGameConnection Connection { get; }

        public virtual string Name => Connection.Name;

        public virtual bool Equals(IPlayer? other)
        {
            return Connection.Equals(other?.Connection);
        }

        public virtual int CompareTo(IPlayer? other)
        {
            return new PlayerComparer().Compare(this, other);
        }
    }
}
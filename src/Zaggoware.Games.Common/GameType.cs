namespace Zaggoware.Games.Common
{
    using System;

    public abstract class GameType<TGame, TGameRules> : IGameType
        where TGame : class, IGame
        where TGameRules : class, IGameRules
    {
        protected GameType(string userFriendlyName)
            : this()
        {
            UserFriendlyName = userFriendlyName;
        }

        private GameType()
        {
            Name = typeof(TGame).Name;
            UserFriendlyName = Name;
        }

        public string Name { get; protected set; }

        public string UserFriendlyName { get; protected set; }

        protected abstract TGame CreateGame(TGameRules gameRules);

        IGame IGameType.CreateGame(IGameRules gameRules)
        {
            if (gameRules is TGameRules castedGameRules)
            {
                return CreateGame(castedGameRules);
            }

            throw new InvalidOperationException(
                $"Game rules is of the wrong type. Expected type '{typeof(TGameRules).FullName}', "
                + $"got '{gameRules.GetType().FullName}' instead.");
        }
    }
}
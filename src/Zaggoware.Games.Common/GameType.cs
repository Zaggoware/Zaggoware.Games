namespace Zaggoware.Games.Common
{
    using System;

    public abstract class GameType<TGame, TGameRules> : IGameType
        where TGame : class, IGame
        where TGameRules : class, IGameRules
    {
        public abstract string Name { get; }

        public abstract string[] DefaultRulePresets { get; }

        public override bool Equals(object? obj)
        {
            if (obj is  GameType<TGame, TGameRules> other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        protected abstract TGame CreateGame(TGameRules gameRules);

        public abstract IGameRules CreateDefaultGameRules(string? preset = null);

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

        protected virtual bool Equals(GameType<TGame, TGameRules> a, GameType<TGame, TGameRules> b)
        {
            return a.Name == b.Name;
        }
    }
}
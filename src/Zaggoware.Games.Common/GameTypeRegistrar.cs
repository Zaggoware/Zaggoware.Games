namespace Zaggoware.Games.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class GameTypeRegistrar
    {
        private static readonly List<Type> GameTypes = new List<Type>();
        private static readonly object LockObject = new object();

        public static void Register<TGame>()
            where TGame : IGame
        {
            lock (LockObject)
            {
                GameTypes.Add(typeof(TGame));
            }
        }

        public static bool Contains<TGame>()
        {
            lock (LockObject)
            {
                return GameTypes.Contains(typeof(TGame));
            }
        }

        public static bool TryGetType(string name, out Type? type)
        {
            lock (LockObject)
            {
                type = GameTypes.FirstOrDefault(g => g.Name.Equals(name));
                return type != null;
            }
        }
    }
}
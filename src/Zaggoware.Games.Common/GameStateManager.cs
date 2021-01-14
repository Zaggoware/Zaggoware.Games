namespace Zaggoware.Games.Common
{
    using System;

    using Zaggoware.Games.Common.Collections;

    public class GameStateManager
    {
        private static GameStateManager? _instance;

        private GameStateManager()
        {
        }

        public static GameStateManager Instance => _instance ??= new GameStateManager();

        public IGameCollection Games { get; } = new GameCollection();

        public IGameConnectionCollection Connections { get; } = new GameConnectionCollection();
    }
}
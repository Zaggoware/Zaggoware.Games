namespace Zaggoware.Games.Common
{
    using System;

    public interface IGame : IDisposable
    {
        string Id { get; }
        
        bool IsPaused { get; }

        bool IsStarted { get; }

        IGameRules Rules { get; }

        IGameType Type { get; }

        bool AddPlayer(IPlayer player);

        bool AddSpectator(IGameConnection spectator);

        bool ChangeRules(IGameRules newRules);

        IPlayer[] GetPlayers();

        IGameConnection[] GetSpectators();

        bool Pause();

        void RandomizePlayingOrder();

        bool RemovePlayer(IPlayer player);

        bool RemoveSpectator(IGameConnection spectator);

        bool Resume();

        bool Start();

        bool Stop();
    }
}
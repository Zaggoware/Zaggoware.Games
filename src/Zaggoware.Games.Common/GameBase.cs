namespace Zaggoware.Games.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Zaggoware.Games.Common.Events;

    public abstract class GameBase<TGameRules, TPlayer> : IGame
        where TGameRules : class, IGameRules
        where TPlayer : class, IPlayer
    {
        private bool _disposed;
        private TGameRules _rules;

        protected GameBase(TGameRules rules)
        {
            if (rules == null)
            {
                throw new ArgumentNullException(nameof(rules));
            }

            _rules = (TGameRules)rules.Copy();
            Initialize(Rules);
        }

        ~GameBase() => Dispose(false);

        public event EventHandler? Pausing;

        public event EventHandler? Paused;

        public event EventHandler<TPlayer>? PlayerAdded;

        public event EventHandler<TPlayer>? PlayerRemoved;

        public event EventHandler? Resuming;

        public event EventHandler? Resumed;

        public event EventHandler<GameRulesChangeEventArgs>? RulesChanging;

        public event EventHandler<GameRulesChangeEventArgs>? RulesChanged;

        public event EventHandler<IGameConnection>? SpectatorAdded;

        public event EventHandler<IGameConnection>? SpectatorRemoved;

        public event EventHandler? Starting;

        public event EventHandler? Started;

        public event EventHandler? Stopping;

        public event EventHandler? Stopped;

        public string Id => "1";// Guid.NewGuid().ToString();

        public bool IsPaused { get; private set; }

        public bool IsStarted { get; private set; }

        public TGameRules Rules
        {
            get => (TGameRules)_rules.Copy();
            protected set => _rules = (TGameRules)value.Copy();
        }

        protected List<TPlayer> Players { get; set; } = new List<TPlayer>();

        protected List<IGameConnection> Spectators { get; set; } = new List<IGameConnection>();

        IGameRules IGame.Rules => Rules;

        public bool AddPlayer(TPlayer player)
        {
            if (IsStarted
                || Players.Count >= Rules.MaxPlayers
                || Players.Contains(player))
            {
                return false;
            }

            Players.Add(player);
            OnPlayerAdded(player);
            return true;
        }

        public bool AddSpectator(IGameConnection spectator)
        {
            if (!Spectators.Contains(spectator))
            {
                return false;
            }

            Spectators.Add(spectator);

            OnSpectatorAdded(spectator);
            return true;
        }

        public bool ChangeRules(TGameRules newRules)
        {
            if (IsStarted)
            {
                throw new InvalidOperationException("Cannot change game rules while the game is started.");
            }

            var oldRules = (TGameRules)Rules.Copy();
            newRules = (TGameRules)newRules.Copy();

            if (!CanChangeRules(oldRules, newRules))
            {
                return false;
            }

            OnRulesChanging(new GameRulesChangeEventArgs(oldRules, newRules));

            Rules = newRules;

            OnRulesChanged(new GameRulesChangeEventArgs(oldRules, newRules));
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TPlayer[] GetPlayers()
        {
            return Players.ToArray();
        }

        public IGameConnection[] GetSpectators()
        {
            return Spectators.ToArray();
        }

        public bool Pause()
        {
            if (!IsStarted || IsPaused)
            {
                return false;
            }

            OnPausing();

            IsPaused = true;

            OnPaused();
            return IsPaused;
        }

        public void RandomizePlayingOrder()
        {
            if (IsStarted)
            {
                return;
            }

            Players = Players.Randomize().ToList();
        }

        public bool RemovePlayer(TPlayer player)
        {
            if (!Players.Contains(player))
            {
                return false;
            }

            Players.Remove(player);
            OnPlayerRemoved(player);

            if (Players.Count <= Rules.MinPlayers)
            {
                Stop();
            }

            return true;
        }

        public bool RemoveSpectator(IGameConnection spectator)
        {
            if (!Spectators.Contains(spectator))
            {
                return false;
            }

            Spectators.Remove(spectator);
            OnSpectatorRemoved(spectator);
            return true;
        }

        public bool Resume()
        {
            if (!IsStarted || !IsPaused)
            {
                return false;
            }

            OnResuming();

            IsPaused = false;

            OnResumed();
            return true;
        }

        public bool Start()
        {
            if (IsStarted
                || Players.Count < Rules.MinPlayers
                || Players.Count > Rules.MaxPlayers)
            {
                return false;
            }

            OnStarting();

            IsPaused = false;
            IsStarted = true;

            OnStarted();
            return true;
        }

        public bool Stop()
        {
            if (!IsStarted)
            {
                return false;
            }

            OnStopping();

            IsStarted = false;

            OnStopped();
            return true;
        }

        protected virtual bool CanChangeRules(TGameRules currentRules, TGameRules newRules)
        {
            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            Stop();
            Players.Clear();
            Spectators.Clear();

            _disposed = true;
        }

        protected abstract void OnInitialize(TGameRules rules);

        protected virtual void OnPaused()
        {
            Paused?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPausing()
        {
            Pausing?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPlayerAdded(TPlayer player)
        {
            PlayerAdded?.Invoke(this, player);
        }

        protected virtual void OnPlayerRemoved(TPlayer player)
        {
            PlayerRemoved?.Invoke(this, player);
        }

        protected virtual void OnResumed()
        {
            Resumed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnResuming()
        {
            Resuming?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRulesChanged(GameRulesChangeEventArgs args)
        {
            RulesChanged?.Invoke(this, args);
        }

        protected virtual void OnRulesChanging(GameRulesChangeEventArgs args)
        {
            RulesChanging?.Invoke(this, args);
        }

        protected virtual void OnSpectatorAdded(IGameConnection spectator)
        {
            SpectatorAdded?.Invoke(this, spectator);
        }

        protected virtual void OnSpectatorRemoved(IGameConnection spectator)
        {
            SpectatorRemoved?.Invoke(this, spectator);
        }

        protected virtual void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStarting()
        {
            Starting?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnStopping()
        {
            Stopping?.Invoke(this, EventArgs.Empty);
        }

        private static T EnsureType<T>(dynamic value, string paramName)
        {
            if (!(value is T typedValue))
            {
                throw new ArgumentException(
                    $"Parameter '{paramName}' is not of the correct type. Expected type '{typeof(T).FullName}', "
                    + $"got '{value.GetType().FullName}' instead.");
            }

            return typedValue;

        }

        private void Initialize(TGameRules rules)
        {
            OnInitialize(rules);
        }

        IPlayer[] IGame.GetPlayers()
        {
            return GetPlayers().ToArray<IPlayer>();
        }

        bool IGame.AddPlayer(IPlayer player)
        {
            var typedPlayer = EnsureType<TPlayer>(player, nameof(player));
            return AddPlayer(typedPlayer);
        }

        bool IGame.ChangeRules(IGameRules newRules)
        {
            var typedNewRules = EnsureType<TGameRules>(newRules, nameof(newRules));
            return ChangeRules(typedNewRules);
        }

        bool IGame.RemovePlayer(IPlayer player)
        {
            var typedPlayer = EnsureType<TPlayer>(player, nameof(player));
            return RemovePlayer(typedPlayer);
        }
    }
}

namespace Zaggoware.Games.Common
{
    using System;
    using System.Timers;

    using Zaggoware.Games.Common.Events;

    using Timer = System.Timers.Timer;

    public abstract class TurnBasedGameBase<TTurnBasedGameRules, TPlayer> : GameBase<TTurnBasedGameRules, TPlayer>, ITurnBasedGame
        where TTurnBasedGameRules : class, ITurnBasedGameRules
        where TPlayer : class, IPlayer
    {
        private readonly Timer _turnTimer = new Timer { Interval = 100 };
        private bool _turnTimerStarted;
        private int _currentPlayerIndex = -1;

        protected TurnBasedGameBase(TTurnBasedGameRules rules)
            : base(rules)
        {
            TurnTimer.Elapsed += OnTurnTimerElapsed;
        }

        public event EventHandler<GameTurnEventArgs<TPlayer>>? TurnBeginning;

        public event EventHandler<GameTurnEventArgs<TPlayer>>? TurnBegan;

        public event EventHandler<GameTurnEventArgs<TPlayer>>? TurnEnding;

        public event EventHandler<GameTurnEventArgs<TPlayer>>? TurnEnded;

        public event EventHandler<GameTurnEventArgs<TPlayer>>? TurnTimedOut;

        public TPlayer? CurrentPlayer { get; private set; }

        public bool IsTurnStarted { get; protected set; }

        public ClockDirection TurnDirection { get; protected set; } = ClockDirection.Clockwise;

        public int TurnIndex { get; protected set; } = -1;

        public DateTime? TurnStartDateTimeUtc { get; protected set; }

        protected int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            set
            {
                _currentPlayerIndex = value;
                CurrentPlayer = value >= 0 && value < Players.Count
                    ? Players[value]
                    : null;
            }
        }

        protected Timer TurnTimer => _turnTimer;

        IPlayer? ITurnBasedGame.CurrentPlayer => CurrentPlayer;

        public virtual bool BeginTurn()
        {
            var nextTurnIndex = TurnIndex + 1;
            var nextTurnPlayerIndex = GetNextPlayerIndex();

            if (!CanBeginTurn(nextTurnIndex, nextTurnPlayerIndex))
            {
                return false;
            }

            var nextTurnPlayer = Players[nextTurnPlayerIndex];
            OnTurnBeginning(new GameTurnEventArgs<TPlayer>(nextTurnIndex, nextTurnPlayer));

            TurnIndex = nextTurnIndex;
            CurrentPlayerIndex = nextTurnPlayerIndex;
            TurnStartDateTimeUtc = DateTime.UtcNow;
            IsTurnStarted = true;

            OnTurnBegan(new GameTurnEventArgs<TPlayer>(TurnIndex, CurrentPlayer!, TurnStartDateTimeUtc));

            if (Rules.MaxTurnDuration <= TimeSpan.Zero)
            {
                return true;
            }

            // Turn has a max duration. Start a timer to check if the turn has timed out.
            _turnTimerStarted = true;
            TurnTimer.Start();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TurnTimer.Elapsed -= OnTurnTimerElapsed;
                TurnTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        public virtual bool ChangeTurnDirection()
        {
            if (!Rules.CanChangeTurnDirection)
            {
                return false;
            }

            TurnDirection = (ClockDirection)((int)TurnDirection * -1);
            return true;
        }

        public virtual bool EndTurn()
        {
            if (!CanEndTurn(TurnIndex, CurrentPlayerIndex, TurnStartDateTimeUtc.GetValueOrDefault()))
            {
                return false;
            }

            var currentTurnEventArgs = new GameTurnEventArgs<TPlayer>(
                TurnIndex,
                CurrentPlayer!,
                TurnStartDateTimeUtc.GetValueOrDefault());
            OnTurnEnding(currentTurnEventArgs);

            IsTurnStarted = false;
            TurnStartDateTimeUtc = null;

            _turnTimerStarted = false;
            TurnTimer.Stop();

            OnTurnEnded(currentTurnEventArgs);
            return true;
        }

        protected virtual bool CanBeginTurn(int turnIndex, int playerIndex)
        {
            return IsStarted && !IsPaused && !IsTurnStarted;
        }

        protected virtual bool CanEndTurn(int turnIndex, int playerIndex, DateTime turnStartedDateTimeUtc)
        {
            return IsStarted && !IsPaused && IsTurnStarted;
        }

        protected virtual int GetNextPlayerIndex()
        {
            return GetNextPlayerIndex(CurrentPlayerIndex);
        }

        protected virtual int GetNextPlayerIndex(int currentIndex)
        {
            var index = currentIndex + (int)TurnDirection;
            if (TurnDirection == ClockDirection.Clockwise)
            {
                return index < Players.Count ? index : 0;
            }

            return index >= 0 ? index : Players.Count - 1;
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            TurnIndex = -1;
            CurrentPlayerIndex = -1;
            IsTurnStarted = false;
        }

        protected virtual void OnTurnTimedOut(GameTurnEventArgs<TPlayer> args)
        {
            TurnTimedOut?.Invoke(this, args);
        }

        protected virtual void OnTurnBegan(GameTurnEventArgs<TPlayer> args)
        {
            TurnBegan?.Invoke(this, args);
        }

        protected virtual void OnTurnBeginning(GameTurnEventArgs<TPlayer> args)
        {
            TurnBeginning?.Invoke(this, args);
        }

        protected virtual void OnTurnEnded(GameTurnEventArgs<TPlayer> args)
        {
            TurnEnded?.Invoke(this, args);
        }

        protected virtual void OnTurnEnding(GameTurnEventArgs<TPlayer> args)
        {
            TurnEnding?.Invoke(this, args);
        }

        private void OnTurnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsTurnStarted || !_turnTimerStarted || Rules.MaxTurnDuration <= TimeSpan.Zero)
            {
                TurnTimer.Stop();
                return;
            }

            var maxTurnDateTimeUtc = TurnStartDateTimeUtc.GetValueOrDefault().Add(Rules.MaxTurnDuration);
            if (DateTime.UtcNow <= maxTurnDateTimeUtc)
            {
                return;
            }

            TurnTimer.Stop();
            OnTurnTimedOut(new GameTurnEventArgs<TPlayer>(TurnIndex, CurrentPlayer!, TurnStartDateTimeUtc));
        }

        protected override void OnPausing()
        {
            base.OnPausing();

            if (_turnTimerStarted)
            {
                TurnTimer.Stop();
            }
        }

        protected override void OnResuming()
        {
            base.OnResuming();

            if (_turnTimerStarted)
            {
                TurnTimer.Start();
            }
        }
    }
}
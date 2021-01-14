namespace Zaggoware.Games.Common.Events
{
    using System;

    public class GameRulesChangeEventArgs : EventArgs
    {
        public GameRulesChangeEventArgs(IGameRules oldRules, IGameRules newRules)
        {
            OldRules = oldRules;
            NewRules = newRules;
        }

        public IGameRules OldRules { get; }

        public IGameRules NewRules { get; }
    }
}
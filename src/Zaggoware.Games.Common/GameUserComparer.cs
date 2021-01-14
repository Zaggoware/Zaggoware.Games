namespace Zaggoware.Games.Common
{
    using System;
    using System.Collections.Generic;

    public class GameUserComparer : IComparer<IGameConnection>
    {
        public int Compare(IGameConnection? x, IGameConnection? y)
        {
            return string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
        }
    }
}

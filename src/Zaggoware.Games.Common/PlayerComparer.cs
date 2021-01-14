namespace Zaggoware.Games.Common
{
    using System;
    using System.Collections.Generic;

    public class PlayerComparer : IComparer<IPlayer>
    {
        public int Compare(IPlayer? x, IPlayer? y)
        {
            return string.Compare(x?.Name, y?.Name, StringComparison.Ordinal);
        }
    }
}

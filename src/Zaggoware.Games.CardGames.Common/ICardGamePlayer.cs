namespace Zaggoware.Games.CardGames.Common
{
    using System.Collections.Generic;

    using Zaggoware.Games.Common;

    public interface ICardGamePlayer : IPlayer
    {
        IList<PlayingCard> Hand { get; }
    }
}
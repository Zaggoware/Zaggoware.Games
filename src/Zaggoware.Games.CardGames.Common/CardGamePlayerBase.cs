namespace Zaggoware.Games.CardGames.Common
{
    using System.Collections.Generic;

    using Zaggoware.Games.Common;

    public abstract class CardGamePlayerBase : PlayerBase, ICardGamePlayer
    {
        protected CardGamePlayerBase(IGameConnection connection)
            : base(connection)
        {
        }

        public IList<PlayingCard> Hand { get; set; } = new List<PlayingCard>();
    }
}
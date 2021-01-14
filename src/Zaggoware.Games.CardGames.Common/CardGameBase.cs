namespace Zaggoware.Games.CardGames.Common
{
    using Zaggoware.Games.Common;

    public abstract class CardGameBase<TCardGameRules, TPlayer> : GameBase<TCardGameRules, TPlayer>, ICardGame
        where TCardGameRules : class, ICardGameRules
        where TPlayer : class, ICardGamePlayer
    {
        protected CardGameBase(TCardGameRules rules)
            : base(rules)
        {
        }
    }
}

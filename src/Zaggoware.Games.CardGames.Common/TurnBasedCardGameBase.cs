namespace Zaggoware.Games.CardGames.Common
{
    using Zaggoware.Games.Common;

    public abstract class TurnBasedCardGameBase<TTurnBasedCardGameRules, TPlayer> : TurnBasedGameBase<TTurnBasedCardGameRules, TPlayer>, ICardGame
        where TTurnBasedCardGameRules : class, ITurnBasedCardGameRules
        where TPlayer : class, ICardGamePlayer
    {
        protected TurnBasedCardGameBase(TTurnBasedCardGameRules rules)
            : base(rules)
        {
        }
    }
}

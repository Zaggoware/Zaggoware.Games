namespace Zaggoware.Games.CardGames.Common
{
    using System;

    using Zaggoware.Games.Common;

    public abstract class CardGameRules : GameRulesBase, ICardGameRules
    {
        public CardRank[] ExcludedCards { get; set; } = Array.Empty<CardRank>();

        public int NumberOfJokersPerPack { get; set; }

        public int NumberOfPacks { get; set; }
    }
}
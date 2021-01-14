namespace Zaggoware.Games.CardGames.Common
{
    using Zaggoware.Games.Common;

    public interface ICardGameRules : IGameRules
    {
        public CardRank[] ExcludedCards { get; set; }

        int NumberOfPacks { get; set; }

        int NumberOfJokersPerPack { get; set; }
    }
}
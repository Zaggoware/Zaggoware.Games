namespace Zaggoware.Games.CrazyEights.Models
{
    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.Hubs.Models;

    public class CrazyEightsSuitChangedHubEventModel : IHubEventModel
    {
        public CrazyEightsSuitChangedHubEventModel(CrazyEightsPlayer player, CardSuit suit)
        {
            Player = new CrazyEightsPlayerInfo(player);
            Suit = suit;
        }

        public CrazyEightsPlayerInfo Player { get; }

        public CardSuit Suit { get; }
    }
}
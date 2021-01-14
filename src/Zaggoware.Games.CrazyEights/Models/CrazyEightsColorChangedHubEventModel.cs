namespace Zaggoware.Games.CrazyEights.Models
{
    using Zaggoware.Games.CardGames.Common;
    using Zaggoware.Games.Hubs.Models;

    public class CrazyEightsColorChangedHubEventModel : IHubEventModel
    {
        public CrazyEightsColorChangedHubEventModel(CrazyEightsPlayer player, CardColor color)
        {
            Player = new CrazyEightsPlayerInfo(player);
            Color = color;
        }

        public CrazyEightsPlayerInfo Player { get; }

        public CardColor Color { get; }
    }
}
namespace Zaggoware.Games.CrazyEights.Models
{
    using Zaggoware.Games.CrazyEights;
    using Zaggoware.Games.Hubs.Models;

    public class CrazyEightsPlayerHubModel : PlayerHubModel
    {
        public CrazyEightsPlayerHubModel(CrazyEightsPlayer player)
            : base(player)
        {
            CardsInHand = player.Hand.Count;
        }

        public int CardsInHand { get; set; }
    }
}
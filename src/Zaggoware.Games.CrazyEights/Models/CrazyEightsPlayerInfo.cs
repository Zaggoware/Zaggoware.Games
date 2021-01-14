namespace Zaggoware.Games.CrazyEights.Models
{
    using Zaggoware.Games.CrazyEights;

    public class CrazyEightsPlayerInfo
    {
        public CrazyEightsPlayerInfo(CrazyEightsPlayer player)
        {
            ConnectionId = player.Connection.Id;
            Name = player.Name;
            CardsInHand = player.Hand.Count;
        }

        public string ConnectionId { get; }

        public string Name { get; }

        public int CardsInHand { get; }
    }
}
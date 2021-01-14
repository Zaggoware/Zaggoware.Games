namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public class CardColorChangedEventArgs<TPlayer> : EventArgs
    {
        public CardColorChangedEventArgs(TPlayer player, CardColor color)
        {
            Player = player;
            Color = color;
        }

        public TPlayer Player { get; set; }

        public CardColor Color { get; set; }
    }
}
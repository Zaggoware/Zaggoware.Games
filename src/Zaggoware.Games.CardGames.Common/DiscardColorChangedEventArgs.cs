namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public class DiscardColorChangedEventArgs<TPlayer> : EventArgs
    {
        public DiscardColorChangedEventArgs(TPlayer player, CardColor color)
        {
            Player = player;
            Color = color;
        }

        public TPlayer Player { get; set; }

        public CardColor Color { get; set; }
    }
}
namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public class CardDrawnEventArgs<TPlayer> : EventArgs
    {
        public CardDrawnEventArgs(TPlayer player, PlayingCard card)
        {
            Player = player;
            Card = card;
        }

        public TPlayer Player { get; set; }

        public PlayingCard Card { get; set; }
    }
}
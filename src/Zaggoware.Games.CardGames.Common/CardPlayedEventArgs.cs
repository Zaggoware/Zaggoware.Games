namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public class CardPlayedEventArgs<TPlayer> : EventArgs
    {
        public CardPlayedEventArgs(TPlayer player, PlayingCard card)
        {
            Player = player;
            Card = card;
        }

        public TPlayer Player { get; set; }

        public PlayingCard Card { get; set; }
    }
}
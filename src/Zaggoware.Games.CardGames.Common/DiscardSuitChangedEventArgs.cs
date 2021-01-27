namespace Zaggoware.Games.CardGames.Common
{
    using System;

    public class DiscardSuitChangedEventArgs<TPlayer> : EventArgs
    {
        public DiscardSuitChangedEventArgs(TPlayer player, CardSuit suit)
        {
            Player = player;
            Suit = suit;
        }

        public TPlayer Player { get; set; }

        public CardSuit Suit { get; set; }
    }
}
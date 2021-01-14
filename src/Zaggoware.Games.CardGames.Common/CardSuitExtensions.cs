namespace Zaggoware.Games.CardGames.Common
{
    public static class CardSuitExtensions
    {
        public static CardColor GetColor(this CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Clubs:
                case CardSuit.Spades:
                    return CardColor.Black;

                case CardSuit.Diamonds:
                case CardSuit.Hearts:
                    return CardColor.Red;

                case CardSuit.Any:
                    return CardColor.Any;

                default:
                    return CardColor.Unknown;
            }
        }
    }
}
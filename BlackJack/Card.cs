namespace DeckOfCards
{
    public class Card
    {
        public int Value { get; }

        public CardSuit Suit { get; }

        public Card(int value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards
{
    public class PlayingCardDeck : IDeck
    {
        private Random Random = new Random();
        private const int ValueMin = 1;
        private const int ValueMax = 13;

        private List<Card> Cards { get; set; }

        public PlayingCardDeck()
        {
            Cards = GenerateDeck();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var card in Cards)
            {
                stringBuilder.Append(card);
            }
            return stringBuilder.ToString();
        }

        public Card RemoveTopCard()
        {
            return Remove(0);
        }

        public Card RemoveBottomCard()
        {
            return Remove(Cards.Count - 1);
        }

        public Card RemoveRandomCard()
        {
            return Remove(GenerateRandomCardIndex());
        }

        public void Shuffle()
        {
            int x = Cards.Count;

            while (x > 1)
            {
                int y = (Random.Next(0, x));

                x--;

                Card card = Cards[y];

                Cards[y] = Cards[x];
                Cards[x] = card;
            }
        }

        public void Cut()
        {
            var splitIndex = (Cards.Count / 2) - 1;
            var firstHalf = Cards.GetRange(0, splitIndex + 1);
            var secondHalf = Cards.GetRange(firstHalf.Count, Cards.Count - (splitIndex + 1));

            secondHalf.AddRange(firstHalf);

            Cards = secondHalf;
        }

        public Card Remove(int index)
        {
            var card = Cards[index];

            Cards.RemoveAt(index);

            return card;
        }

        private List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();

            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                for (int value = ValueMin; value <= ValueMax; value++)
                {
                    cards.Add(new Card(value, suit));
                }
            }

            return cards;
        }

        private int GenerateRandomCardIndex()
        {
            return Random.Next(0, Cards.Count);
        }
    }
}
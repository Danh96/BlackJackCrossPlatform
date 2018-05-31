namespace DeckOfCards
{
    public interface IDeck
    {
        Card RemoveTopCard();

        Card RemoveBottomCard();

        Card RemoveRandomCard();

        Card Remove(int index);

        void Shuffle();

        void Cut();
    }
}
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using BlackJack.Droid;
using DeckOfCards;

namespace BlackJack
{
    public class CardView : LinearLayout
    {
        TextView topLeftChar;
        TextView bottomRightChar;
        ImageView suitImage;
        LinearLayout cardLayout;

        public CardView(Context context) :
            base(context)
        {
            InflateLayout(context);
        }
        public CardView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            InflateLayout(context);
        }

        public CardView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            InflateLayout(context);
        }

        void InflateLayout(Context context)
        {
            LayoutInflater inflater = LayoutInflater.FromContext(context);
            inflater.Inflate(Resource.Layout.CardTemplate, this);

            topLeftChar = FindViewById<TextView>(Resource.Id.TopLeftChar);
            bottomRightChar = FindViewById<TextView>(Resource.Id.BottomRightChar);
            suitImage = FindViewById<ImageView>(Resource.Id.SuitImage);
            cardLayout = FindViewById<LinearLayout>(Resource.Id.LayoutCard);
        }

        public void SetCardValues(Card card)
        {
            topLeftChar.Text = GetCardValue(card);
            bottomRightChar.Text = GetCardValue(card);
            suitImage.SetImageResource(GetcardSuit(card));
        }

        public void SetDealerCardFaceDown(bool faceDown = true)
        {
            if (faceDown == true)
            {
                cardLayout.SetBackgroundResource(Resource.Drawable.CardBack);
                topLeftChar.Text = string.Empty;
                bottomRightChar.Text = string.Empty;
                suitImage.SetImageResource(Android.Resource.Color.Transparent);
            }
            else
            {
                cardLayout.SetBackgroundResource(Resource.Drawable.CardBackground);
            }
        }

        private int GetcardSuit(Card card)
        {
            int suitIconImage;

            switch (card.Suit)
            {
                case CardSuit.Clubs:
                    suitIconImage = Resource.Drawable.Clubs;
                    break;
                case CardSuit.Spades:
                    suitIconImage = Resource.Drawable.Spades;
                    break;
                case CardSuit.Diamonds:
                    suitIconImage = Resource.Drawable.Diamonds;
                    break;
                case CardSuit.Hearts:
                    suitIconImage = Resource.Drawable.Hearts;
                    break;
                default:
                    suitIconImage = 0;
                    break;
            }

            return suitIconImage;
        }

        public string GetCardValue(Card card)
        {
            string cardValue;

            switch (card.Value)
            {
                case 1:
                    cardValue = "A";
                    break;
                case 11:
                    cardValue = "J";
                    break;
                case 12:
                    cardValue = "Q";
                    break;
                case 13:
                    cardValue = "K";
                    break;
                default:
                    cardValue = card.Value.ToString();
                    break;
            }

            return cardValue;
        }
    }
}
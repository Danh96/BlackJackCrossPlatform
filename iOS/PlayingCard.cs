using Foundation;
using System;
using UIKit;
using ObjCRuntime;
using DeckOfCards;
using System.Linq;

namespace BlackJackIOS
{
	public partial class PlayingCard : UIView
    {
        public static readonly NSString Key = new NSString("PlayingCard");
        public static readonly UINib Nib;

        static PlayingCard()
        {
            Nib = UINib.FromName("PlayingCard", NSBundle.MainBundle);
        }

        protected PlayingCard(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public static PlayingCard Create(Card card)
        {
			var cardView = (PlayingCard)Nib.Instantiate(null, null).First();

			cardView.Layer.BorderWidth = 1;
			cardView.Layer.CornerRadius = 5;
			cardView.Layer.BorderColor = UIColor.Black.CGColor;

			cardView.CardSuitImage.Image = GetcardSuit(card);
			cardView.CardValueTop.Text = GetCardValue(card);
			cardView.CardValueBottom.Text = GetCardValue(card);
			cardView.CardBackgroundImage.Image = null;

			return cardView;
        }

		public static PlayingCard SetDealerCardFaceDown()
        {
            var cardView = (PlayingCard)Nib.Instantiate(null, null).First();

            cardView.Layer.BorderWidth = 1;
            cardView.Layer.CornerRadius = 5;
            cardView.Layer.BorderColor = UIColor.Black.CGColor;

			cardView.CardSuitImage.Image = null;
            cardView.CardValueTop.Text = "";
            cardView.CardValueBottom.Text = "";
			cardView.CardBackgroundImage.Image = UIImage.FromBundle("CardBack");

            return cardView;
        }

		private static UIImage GetcardSuit(Card card)
        {
			UIImage suitIconImage;

            switch (card.Suit)
            {
                case CardSuit.Clubs:
					suitIconImage = UIImage.FromBundle("Clubs");
                    break;
                case CardSuit.Spades:
					suitIconImage = UIImage.FromBundle("Spades");
                    break;
                case CardSuit.Diamonds:
					suitIconImage = UIImage.FromBundle("Diamonds");
                    break;
                case CardSuit.Hearts:
					suitIconImage = UIImage.FromBundle("Hearts");
                    break;
                default:
					suitIconImage = null;
                    break;
            }

            return suitIconImage;
        }

		public static string GetCardValue(Card card)
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
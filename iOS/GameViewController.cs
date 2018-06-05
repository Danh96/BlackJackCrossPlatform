using System.Collections.Generic;
using System.Threading;
using AudioToolbox;
using CoreGraphics;
using DeckOfCards;
using Foundation;
using UIKit;

namespace BlackJackIOS
{
	public partial class GameViewController : UIViewController
	{
        private GameFunctions gameFunctions = new GameFunctions();

        private CancellationTokenSource CancellationToken = new CancellationTokenSource();
        
		private SystemSound shuffleSound = new SystemSound(NSUrl.FromFilename("Sounds/ShuffleSound.mp3"));

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			CancellationToken.Cancel();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			CancellationToken = new CancellationTokenSource();
            SetCardsToInvisible();
            gameFunctions.ResetGame();
            SelectMatchPointsDialogPopUp();
		}

		public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
                     
			var titleView = new UIView(new CGRect (0, 0, 120, 40));
			var titleImageView = new UIImageView(UIImage.FromBundle("Logo"));
			titleImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			titleImageView.Frame = new CGRect(0, 0, titleView.Frame.Width, titleView.Frame.Height);
			titleView.AddSubview(titleImageView);
			NavigationController.NavigationBar.TopItem.TitleView = titleView;
        }

		public override void ViewDidLoad()
        {
			base.ViewDidLoad();

            gameFunctions.PropertyChanged += GameFunctions_PropertyChanged;

			SetCustomUI();

			ButtonHit.TouchUpInside += async (sender, e) =>
			{
                await gameFunctions.PlayerHit(CancellationToken.Token);
			};

			ButtonStick.TouchUpInside += async (sender, e) =>
			{
                await gameFunctions.PlayerStick(CancellationToken.Token);
			};
        }

        void GameFunctions_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(nameof(gameFunctions.PlayerScoreText), e.PropertyName))
            {
                LabelPlayerScore.Text = gameFunctions.PlayerScoreText;
            }
            else if (string.Equals(nameof(gameFunctions.DealerScoreText), e.PropertyName))
            {
                LabelDealerScore.Text = gameFunctions.DealerScoreText;
            }
            else if (string.Equals(nameof(gameFunctions.PlayersHandTotalText), e.PropertyName))
            {
                LabelPlayersHandTotal.Text = gameFunctions.PlayersHandTotalText;
            }
            else if (string.Equals(nameof(gameFunctions.DealersHandTotalText), e.PropertyName))
            {
                LabelDealersHandTotal.Text = gameFunctions.DealersHandTotalText;
            }
            else if (string.Equals(nameof(gameFunctions.ConvoText), e.PropertyName))
            {
                LabelConvoText.Text = gameFunctions.ConvoText;
            }
            else if (string.Equals(nameof(gameFunctions.ButtonsEnabled), e.PropertyName))
            {
                ButtonHit.Enabled = gameFunctions.ButtonsEnabled;
                ButtonStick.Enabled = gameFunctions.ButtonsEnabled;
            }
            else if (string.Equals(nameof(gameFunctions.GameContinues), e.PropertyName))
            {
                if (gameFunctions.GameContinues == false)
                {
                    UIStoryboard endGameStoryBoard = UIStoryboard.FromName("EndGame", NSBundle.MainBundle);
                    EndGameViewController endGameViewController = endGameStoryBoard.InstantiateViewController("EndGameViewController") as EndGameViewController;
                    endGameViewController.PlayerGameScore = gameFunctions.PlayerGameScore;
                    endGameViewController.DealerGameScore = gameFunctions.DealerGameScore;
                    NavigationController.PresentViewController(endGameViewController, true, null);
                }
                else
                {
                    StartShufflePlayer();
                    SetNewHand();
                }
            }
            else if (string.Equals(nameof(gameFunctions.PlayersHand), e.PropertyName))
            {
                PrintPlayerHand(gameFunctions.PlayersHand);
            }
            else if (string.Equals(nameof(gameFunctions.DealersHand), e.PropertyName))
            {
                PrintDealersHand(gameFunctions.DealersHand);
            }
        }

        private void SetNewHand()
        {
            SetDealersCardsFaceDown(DealerFirstCard);
            SetDealersCardsFaceDown(DealerSecondCard);
            DealerFirstCard.Alpha = 1;
            DealerSecondCard.Alpha = 1;
            DealerThirdCard.Alpha = 0;
            DealerFourthCard.Alpha = 0;
            DealerFifthCard.Alpha = 0;

            PlayerFirstCard.Alpha = 1;
            PlayerSecondCard.Alpha = 1;
            PlayerThirdCard.Alpha = 0;
            PlayerFourthCard.Alpha = 0;
            PlayerFifthCard.Alpha = 0;
        }

		private void SetCardsToInvisible()
        {
			DealerFirstCard.Alpha = 0;
			DealerSecondCard.Alpha = 0;
			DealerThirdCard.Alpha = 0;
            DealerFourthCard.Alpha = 0;
            DealerFifthCard.Alpha = 0;
			PlayerFirstCard.Alpha = 0;
			PlayerSecondCard.Alpha = 0;
            PlayerThirdCard.Alpha = 0;
            PlayerFourthCard.Alpha = 0;
            PlayerFifthCard.Alpha = 0;
        }

		private void PrintPlayerHand(List<Card> hand)
		{
			for (int i = 1; i <= hand.Count; i++)
            {
				var card = hand[i - 1];
                
				switch (i)
                {
                    case 1:
						AddCardToContainer(card, PlayerFirstCard);
                        break;
                    case 2:
						AddCardToContainer(card, PlayerSecondCard);
                        break;
                    case 3:
						AddCardToContainer(card, PlayerThirdCard);
						PlayerThirdCard.Alpha = 1;
                        break;
                    case 4:
						AddCardToContainer(card, PlayerFourthCard);
						PlayerFourthCard.Alpha = 1;
                        break;
                    case 5:
						AddCardToContainer(card, PlayerFifthCard);
						PlayerFifthCard.Alpha = 1;
                        break;
                }
            }
        }

		private void PrintDealersHand(List<Card> hand)
        {
            for (int i = 1; i <= hand.Count; i++)
            {
                var card = hand[i - 1];

                switch (i)
                {
                    case 1:
						AddCardToContainer(card, DealerFirstCard);
						DealerFirstCard.Alpha = 1;
                        break;
                    case 2:
						AddCardToContainer(card, DealerSecondCard);
						DealerSecondCard.Alpha = 1;
                        break;
                    case 3:
						AddCardToContainer(card, DealerThirdCard);
						DealerThirdCard.Alpha = 1;
                        break;
                    case 4:
						AddCardToContainer(card, DealerFourthCard);
						DealerFourthCard.Alpha = 1;
                        break;
                    case 5:
						AddCardToContainer(card, DealerFifthCard);
						DealerFifthCard.Alpha = 1;
                        break;
                }
            }
        }

        private void AddCardToContainer(Card card, UIView containerView)
        {
            var playingCard = PlayingCard.Create(card);
            playingCard.Frame = new CGRect(0, 0, containerView.Frame.Width, containerView.Frame.Height);
            containerView.AddSubview(playingCard);
        }

        private void SetDealersCardsFaceDown(UIView containerView)
        {
            var playingCard = PlayingCard.SetDealerCardFaceDown();
            playingCard.Frame = new CGRect(0, 0, containerView.Frame.Width, containerView.Frame.Height);
            containerView.AddSubview(playingCard);
        }

        private void SetCustomUI()
		{
			NavigationController.SetNavigationBarHidden(false, true);

            NavigationController.NavigationBar.TintColor = UIColor.White;
            NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(245, 0, 0);
            NavigationController.NavigationBar.Translucent = false;

            NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            NavigationController.NavigationBar.ShadowImage = new UIImage();

            ButtonStick.Layer.CornerRadius = 5;
            ButtonStick.Layer.BorderWidth = 1;
            ButtonStick.Layer.BorderColor = UIColor.White.CGColor;

            ButtonHit.Layer.CornerRadius = 5;
            ButtonHit.Layer.BorderWidth = 1;
            ButtonHit.Layer.BorderColor = UIColor.White.CGColor;

			LabelDealersHandTotal.Text = null;
			LabelPlayersHandTotal.Text = null;
			LabelConvoText.Text = null;
			LabelConvoText.Font = UIFont.BoldSystemFontOfSize(26);
		}

		private void SelectMatchPointsDialogPopUp()
		{
			UIAlertController actionSheetAlert = UIAlertController.Create("New Game", "Please select the number of points you want to play to.", UIAlertControllerStyle.Alert);

			actionSheetAlert.View.TintColor = UIColor.FromRGB(245, 0, 0);

            actionSheetAlert.AddAction(UIAlertAction.Create("3", UIAlertActionStyle.Default, (action) => gameFunctions.SetMaxMatchPoint(3)));

            actionSheetAlert.AddAction(UIAlertAction.Create("5", UIAlertActionStyle.Default, (action) => gameFunctions.SetMaxMatchPoint(5)));

            actionSheetAlert.AddAction(UIAlertAction.Create("10", UIAlertActionStyle.Default, (action) => gameFunctions.SetMaxMatchPoint(10)));

            this.PresentViewController(actionSheetAlert, true, null);
		}

		private void StartShufflePlayer()
		{
			shuffleSound.PlayAlertSound();
		}
	}
}
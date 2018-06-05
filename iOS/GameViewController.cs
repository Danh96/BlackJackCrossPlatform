using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AudioToolbox;
using CoreGraphics;
using DeckOfCards;
using Foundation;
using UIKit;

namespace BlackJackIOS
{
	public partial class GameViewController : UIViewController
	{
		private CancellationTokenSource CancellationToken;
        
		private SystemSound shuffleSound = new SystemSound(NSUrl.FromFilename("Sounds/ShuffleSound.mp3"));

        private PlayingCardDeck Deck;
        private List<Card> PlayersHand = new List<Card>();
        private List<Card> DealersHand = new List<Card>();

        private int DealersHandTotal;
        private int PlayersHandTotal;
        private int DealerGameScore;
        private int PlayerGameScore;
        private int MaxMatchPoint;

		public GameViewController (IntPtr handle) : base (handle)
		{
			
		}

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
            ResetGame();
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

			SetCustomUI();

			ButtonHit.TouchUpInside += async (sender, e) =>
			{
				PlayersHand.Add(Deck.RemoveTopCard());
                PrintPlayerHand(PlayersHand);
                PlayersHandTotal = UpdateScore(PlayersHand);
				LabelPlayersHandTotal.Text = "Players hand total: " + PlayersHandTotal.ToString();

                await CheckIfBust(CancellationToken.Token);
                await CheckIfPlayerHasFiveCardTrick(CancellationToken.Token);
			};

			ButtonStick.TouchUpInside += async (sender, e) =>
			{
				ButtonHit.Enabled = false;
                ButtonStick.Enabled = false;

                await DealersTurn(CancellationToken.Token);
			};
            
			CancellationToken = new CancellationTokenSource();
        }

		private void ResetGame()
        {
            SetCardsToInvisible();
			LabelDealersHandTotal.Text = string.Empty;
			LabelPlayersHandTotal.Text = string.Empty;
			LabelConvoText.Text = string.Empty;
            DealerGameScore = 0;
            PlayerGameScore = 0;
			LabelDealerScore.Text = "Dealers score: " + DealerGameScore.ToString();
			LabelPlayerScore.Text = "Players score: " + PlayerGameScore.ToString();
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

		private void GameStart()
        {
            Deck = new PlayingCardDeck();

            Deck.Shuffle();
			StartShufflePlayer();

            PlayersHandTotal = 0;
            PlayersHand.Clear();

            DealersHandTotal = 0;
            DealersHand.Clear();

			ButtonHit.Enabled = true;
            ButtonStick.Enabled = true;

			LabelPlayerScore.Text = "Players score: " + PlayerGameScore.ToString();
			LabelDealerScore.Text = "Dealers score: " + DealerGameScore.ToString();

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
            
            PlayersHand.Add(Deck.RemoveTopCard());
            DealersHand.Add(Deck.RemoveTopCard());
            PlayersHand.Add(Deck.RemoveTopCard());
            DealersHand.Add(Deck.RemoveTopCard());

			LabelDealersHandTotal.Text = "Dealers hand total: " + DealersHandTotal;

            PrintPlayerHand(PlayersHand);

            PlayersHandTotal = UpdateScore(PlayersHand);
			LabelPlayersHandTotal.Text = "Players hand total: " + PlayersHandTotal.ToString();

			LabelConvoText.Text = "Players turn";
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

		private int UpdateScore(List<Card> hand)
        {
            int HandTotal = 0;
            List<Card> aces = new List<Card>();

            foreach (Card c in hand)
            {
                if (c.Value == 1)
                {
                    aces.Add(c);
                }
                else if (c.Value > 10)
                {
                    HandTotal += 10;
                }
                else
                {
                    HandTotal += c.Value;
                }
            }

            return SetAceValue(aces, HandTotal);
        }

		private int SetAceValue(List<Card> aces, int total)
        {
            int HandTotal = total;

            foreach (Card c in aces)
            {
                if (HandTotal + 11 > 21)
                {
                    HandTotal++;
                }
                else
                {
                    HandTotal += 11;
                }
            }

            return HandTotal;
        }

		private async Task CheckIfBust(CancellationToken ct)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
					if (PlayersHandTotal > 21 && PlayersHandTotal != 100)
                    {
                        PlayersHandTotal = -1;
						LabelPlayersHandTotal.Text = "Players hand total: Bust!";
                        ButtonHit.Enabled = false;
                        ButtonStick.Enabled = false;
                        await DealersTurn(ct);
                    }

					if (DealersHandTotal > 21 && DealersHandTotal != 100)
                    {
                        DealersHandTotal = -1;
						LabelDealersHandTotal.Text = "Dealers hand total: Bust!";
                    }
                }
            }
            catch (System.OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }

		private async Task CheckIfPlayerHasFiveCardTrick(CancellationToken ct)
        {
            try
            {
                if (PlayersHand.Count == 5 && PlayersHandTotal != -1 && !ct.IsCancellationRequested)
                {
                    ButtonHit.Enabled = false;
                    ButtonStick.Enabled = false;
					LabelPlayersHandTotal.Text = "Players hand total: Five cards under!";
                    PlayersHandTotal = 100;
                    await DealersTurn(ct);
                }
            }
            catch (System.OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }

		private async Task DealersTurn(CancellationToken ct)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    bool dealersTurn = true;

                    await Task.Delay(1000, ct);
					LabelConvoText.Text = "Dealers turn";
                    await Task.Delay(2000, ct);

                    PrintDealersHand(DealersHand);
                    DealersHandTotal = UpdateScore(DealersHand);
					LabelDealersHandTotal.Text = "Dealers hand total: " + DealersHandTotal;

                    while (dealersTurn)
                    {
                        if (DealersHandTotal > PlayersHandTotal || DealersHandTotal == -1)
                        {
                            dealersTurn = false;
                        }
                        else if (DealersHandTotal <= 16)
                        {
                            await Task.Delay(1000);
                            DealersHand.Add(Deck.RemoveTopCard());
                            PrintDealersHand(DealersHand);
                            DealersHandTotal = UpdateScore(DealersHand);
							LabelDealersHandTotal.Text = "Dealers hand total: " + DealersHandTotal;
                            await CheckIfBust(ct);
                            CheckIfDealerHasFiveCardTrick();
                        }
                        else
                        {
                            dealersTurn = false;
                        }

                        await Task.Delay(1000, ct);
                    }

                    await UpdateGameScore(ct);
                }
            }
            catch (System.OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }

		private async Task UpdateGameScore(CancellationToken ct)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    if (PlayersHandTotal > DealersHandTotal)
                    {
                        PlayerGameScore++;
						LabelPlayerScore.Text = "Players score: " + PlayerGameScore.ToString();
						LabelConvoText.Text = "Players hand wins.";
                    }
                    else if (PlayersHandTotal < DealersHandTotal)
                    {
                        DealerGameScore++;
						LabelDealerScore.Text = "Dealers score: " + DealerGameScore.ToString();
						LabelConvoText.Text = "Dealers hand wins.";
                    }
                    else if (PlayersHandTotal == DealersHandTotal)
                    {
                        DealerGameScore++;
						LabelConvoText.Text = "Draw, points go to dealer.";
                    }

                    await Task.Delay(2000, ct);
                    await CheckIfGameContinues(ct);
                }
            }
            catch (System.OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }

		private async Task CheckIfGameContinues(CancellationToken ct)
        {
            try
            {
                if (!ct.IsCancellationRequested)
                {
                    if (PlayerGameScore == MaxMatchPoint || DealerGameScore == MaxMatchPoint)
                    {
						UIStoryboard endGameStoryBoard = UIStoryboard.FromName("EndGame", NSBundle.MainBundle);
						EndGameViewController endGameViewController = endGameStoryBoard.InstantiateViewController("EndGameViewController") as EndGameViewController;
						endGameViewController.PlayerGameScore = PlayerGameScore;
						endGameViewController.DealerGameScore = DealerGameScore;
						NavigationController.PresentViewController(endGameViewController, true, null);
                    }
                    else
                    {
						LabelConvoText.Text = "Next Round!";
                        await Task.Delay(1000, ct);
                        GameStart();
                    }
                }
            }
            catch (System.OperationCanceledException ex)
            {
                Console.WriteLine(ex);
            }
        }

		private void CheckIfDealerHasFiveCardTrick()
        {
            if (DealersHand.Count == 5 && DealersHandTotal != -1)
            {
				LabelDealersHandTotal.Text = "Dealers hand total: Five cards under!";
                DealersHandTotal = 100;
            }
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

		private void SetMaxMatchPoint(int maxMatchPoint)
		{
			MaxMatchPoint = maxMatchPoint;
			GameStart();
		}

		private void SelectMatchPointsDialogPopUp()
		{
			UIAlertController actionSheetAlert = UIAlertController.Create("New Game", "Please select the number of points you want to play to.", UIAlertControllerStyle.Alert);

			actionSheetAlert.View.TintColor = UIColor.FromRGB(245, 0, 0);

			actionSheetAlert.AddAction(UIAlertAction.Create("3", UIAlertActionStyle.Default, (action) => SetMaxMatchPoint(3)));

			actionSheetAlert.AddAction(UIAlertAction.Create("5", UIAlertActionStyle.Default, (action) => SetMaxMatchPoint(5)));

			actionSheetAlert.AddAction(UIAlertAction.Create("10", UIAlertActionStyle.Default, (action) => SetMaxMatchPoint(10)));

            this.PresentViewController(actionSheetAlert, true, null);
		}

		private void StartShufflePlayer()
		{
			shuffleSound.PlayAlertSound();
		}
	}
}
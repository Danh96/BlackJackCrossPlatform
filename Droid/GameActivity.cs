﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using BlackJack.Droid;
using DeckOfCards;

namespace BlackJack
{
	[Activity(Label = "Game", Theme = "@android:style/Theme.Holo.NoActionBar.Fullscreen", ScreenOrientation = ScreenOrientation.Portrait)]
	public class GameActivity : Activity
	{
        private GameFunctions gameFunctions = new GameFunctions();

        private CancellationTokenSource CancellationToken = new CancellationTokenSource();

		private MediaPlayer _player = new MediaPlayer();

		private TextView playerGameScoreText;
		private TextView dealerGameScoreText;
		private TextView playersHandText;
		private TextView dealersHandText;
		private TextView convoText;

		private Button buttonStick;
		private Button buttonHit;

		private CardView dealersFirstCard;
		private CardView dealersSecondCard;
		private CardView dealersThirdCard;
		private CardView dealersFourthCard;
		private CardView dealersFifthCard;
		private CardView playersFirstCard;
		private CardView playersSecondCard;
		private CardView playersThirdCard;
		private CardView playersFourthCard;
		private CardView playersFifthCard;

		protected override void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);

            gameFunctions.PropertyChanged += GameFunctions_PropertyChanged;

            SetContentView(Resource.Layout.Game);

			playerGameScoreText = FindViewById<TextView>(Resource.Id.PlayerGameScore);
            dealerGameScoreText = FindViewById<TextView>(Resource.Id.DealerGameScore);
            playersHandText = FindViewById<TextView>(Resource.Id.PlayersHandText);
            dealersHandText = FindViewById<TextView>(Resource.Id.DealersHandText);
            convoText = FindViewById<TextView>(Resource.Id.ConvoText);

            dealersFirstCard = FindViewById<CardView>(Resource.Id.DealersFirstCard);
            dealersSecondCard = FindViewById<CardView>(Resource.Id.DealersSecondCard);
            dealersThirdCard = FindViewById<CardView>(Resource.Id.DealersThirdCard);
            dealersFourthCard = FindViewById<CardView>(Resource.Id.DealersFourthCard);
            dealersFifthCard = FindViewById<CardView>(Resource.Id.DealersFifthCard);
            playersFirstCard = FindViewById<CardView>(Resource.Id.PlayersFirstCard);
            playersSecondCard = FindViewById<CardView>(Resource.Id.PlayersSecondCard);
            playersThirdCard = FindViewById<CardView>(Resource.Id.PlayersThirdCard);
            playersFourthCard = FindViewById<CardView>(Resource.Id.PlayersFourthCard);
            playersFifthCard = FindViewById<CardView>(Resource.Id.PlayersFifthCard);

            buttonStick = FindViewById<Button>(Resource.Id.ButtonStick);
            buttonHit = FindViewById<Button>(Resource.Id.ButtonHit);

            buttonStick.Click += StickButton_Click;
            buttonHit.Click += HitButton_Click;

            SetCardsToInvisible();
		}

		private async void HitButton_Click(object sender, EventArgs e)
		{
            await gameFunctions.PlayerHit(CancellationToken.Token);
        }

		private async void StickButton_Click(object sender, EventArgs e)
		{
            await gameFunctions.PlayerStick(CancellationToken.Token);
		}

		protected override void OnPause()
		{
			base.OnPause();
            CancellationToken.Cancel();
		}

		protected override void OnResume()
		{
			base.OnResume();
            CancellationToken = new CancellationTokenSource();
            SetCardsToInvisible();
            gameFunctions.ResetGame();
            SelectMatchPointsDialogPopUp();
		}

        void GameFunctions_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Equals(nameof(gameFunctions.PlayerScoreText), e.PropertyName))
            {
                playerGameScoreText.Text = gameFunctions.PlayerScoreText;
            }
            else if (string.Equals(nameof(gameFunctions.DealerScoreText), e.PropertyName))
            {
                dealerGameScoreText.Text = gameFunctions.DealerScoreText;
            }
            else if (string.Equals(nameof(gameFunctions.PlayersHandTotalText), e.PropertyName))
            {
                playersHandText.Text = gameFunctions.PlayersHandTotalText;
            }
            else if (string.Equals(nameof(gameFunctions.DealersHandTotalText), e.PropertyName))
            {
                dealersHandText.Text = gameFunctions.DealersHandTotalText;
            }
            else if (string.Equals(nameof(gameFunctions.ConvoText), e.PropertyName))
            {
                convoText.Text = gameFunctions.ConvoText;
            }
            else if (string.Equals(nameof(gameFunctions.ButtonsEnabled), e.PropertyName))
            {
                buttonHit.Enabled = gameFunctions.ButtonsEnabled;
                buttonStick.Enabled = gameFunctions.ButtonsEnabled;
            }
            else if (string.Equals(nameof(gameFunctions.GameContinues), e.PropertyName))
            {
                if(gameFunctions.GameContinues == false)
                {
                    Intent intent = new Intent(this, typeof(EndGameActivity));
                    intent.PutExtra("playerGameScore", gameFunctions.PlayerGameScore);
                    intent.PutExtra("dealerGameScore", gameFunctions.DealerGameScore);
                    StartActivity(intent);
                    Finish();
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

        private void SetCardsToInvisible()
        {
            dealersFirstCard.Visibility = ViewStates.Invisible;
            dealersSecondCard.Visibility = ViewStates.Invisible;
            dealersThirdCard.Visibility = ViewStates.Invisible;
            dealersFourthCard.Visibility = ViewStates.Invisible;
            dealersFifthCard.Visibility = ViewStates.Invisible;
            playersFirstCard.Visibility = ViewStates.Invisible;
            playersSecondCard.Visibility = ViewStates.Invisible;
            playersThirdCard.Visibility = ViewStates.Invisible;
            playersFourthCard.Visibility = ViewStates.Invisible;
            playersFifthCard.Visibility = ViewStates.Invisible;
        }

        private void SetNewHand()
		{
			dealersFirstCard.Visibility = ViewStates.Visible;
            dealersSecondCard.Visibility = ViewStates.Visible;
            dealersThirdCard.Visibility = ViewStates.Invisible;
            dealersFourthCard.Visibility = ViewStates.Invisible;
            dealersFifthCard.Visibility = ViewStates.Invisible;

            dealersFirstCard.SetDealerCardFaceDown();
            dealersSecondCard.SetDealerCardFaceDown();

            playersThirdCard.Visibility = ViewStates.Invisible;
            playersFourthCard.Visibility = ViewStates.Invisible;
            playersFifthCard.Visibility = ViewStates.Invisible;
		}

		private void PrintPlayerHand(List<Card> hand)
        {
            var count = hand.Count();

            foreach (Card card in hand)
            {
                switch (count)
                {
                    case 1:
                        playersFirstCard.SetCardValues(gameFunctions.PlayersHand[count - 1]);
                        playersFirstCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 2:
                        playersSecondCard.SetCardValues(gameFunctions.PlayersHand[count - 1]);
                        playersSecondCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 3:
                        playersThirdCard.SetCardValues(gameFunctions.PlayersHand[count - 1]);
                        playersThirdCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 4:
                        playersFourthCard.SetCardValues(gameFunctions.PlayersHand[count - 1]);
                        playersFourthCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 5:
                        playersFifthCard.SetCardValues(gameFunctions.PlayersHand[count - 1]);
                        playersFifthCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                }
            }
        }

        private void PrintDealersHand(List<Card> hand)
        {
            var count = hand.Count();

            foreach (Card card in hand)
            {
                switch (count)
                {
                    case 1:
                        dealersFirstCard.SetDealerCardFaceDown(false);
                        dealersFirstCard.SetCardValues(gameFunctions.DealersHand[count - 1]);
                        dealersFirstCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 2:
                        dealersSecondCard.SetDealerCardFaceDown(false);
                        dealersSecondCard.SetCardValues(gameFunctions.DealersHand[count - 1]);
                        dealersSecondCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 3:
                        dealersThirdCard.SetCardValues(gameFunctions.DealersHand[count - 1]);
                        dealersThirdCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 4:
                        dealersFourthCard.SetCardValues(gameFunctions.DealersHand[count - 1]);
                        dealersFourthCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                    case 5:
                        dealersFifthCard.SetCardValues(gameFunctions.DealersHand[count - 1]);
                        dealersFifthCard.Visibility = ViewStates.Visible;
                        count--;
                        break;
                }
            }
        }

        private void SelectMatchPointsDialogPopUp()
        {
            var MatchPointAlert = (new AlertDialog.Builder(this)).Create();
            MatchPointAlert.SetCancelable(false);
            MatchPointAlert.SetMessage("Please select the number of points you want to play to.");
            MatchPointAlert.SetTitle("New Game");
            MatchPointAlert.SetButton("10 points", SetMatchPointsToTen);
            MatchPointAlert.SetButton2("3 points", SetMatchPointsToThree);
            MatchPointAlert.SetButton3("5 points", SetMatchPointsToFive);
            MatchPointAlert.Show();

            StyleAlertDialog(MatchPointAlert);
        }

        private static void StyleAlertDialog(Dialog dialog)
        {
            try
            {
                var resources = dialog.Context.Resources;

                var alertTitleId = resources.GetIdentifier("alertTitle", "id", "android");
                var alertTitle = (TextView)dialog.Window.DecorView.FindViewById(alertTitleId);
                alertTitle.SetTextColor(Android.Graphics.Color.Red);

                var titleDividerId = resources.GetIdentifier("titleDivider", "id", "android");
                var titleDivider = dialog.Window.DecorView.FindViewById(titleDividerId);
                titleDivider.SetBackgroundColor(Android.Graphics.Color.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void SetMatchPointsToTen(object sender, DialogClickEventArgs e)
        {
            gameFunctions.SetMaxMatchPoint(10);
            SetNewHand();
            StartShufflePlayer();
        }

        private void SetMatchPointsToFive(object sender, DialogClickEventArgs e)
        {
            gameFunctions.SetMaxMatchPoint(5);
            SetNewHand();
            StartShufflePlayer();
        }

        private void SetMatchPointsToThree(object sender, DialogClickEventArgs e)
        {
            gameFunctions.SetMaxMatchPoint(3);
            SetNewHand();
            StartShufflePlayer();
        }

        private void StartShufflePlayer()
        {
            _player = MediaPlayer.Create(this, Resource.Raw.ShuffleSound);
            _player.Start();
        }
	}
}
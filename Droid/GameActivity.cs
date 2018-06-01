using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
		private CancellationTokenSource CancellationToken;

		private MediaPlayer _player = new MediaPlayer();

		private PlayingCardDeck Deck;
		private List<Card> PlayersHand = new List<Card>();
		private List<Card> DealersHand = new List<Card>();

		private int DealersHandTotal;
		private int PlayersHandTotal;
		private int DealerGameScore;
		private int PlayerGameScore;
		private int MaxMatchPoint;

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
		}

		private void HitButton_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void StickButton_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

		protected override void OnResume()
		{
			base.OnResume();
		}
	}
}
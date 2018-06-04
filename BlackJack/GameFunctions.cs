using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DeckOfCards;

namespace BlackJackIOS
{
    public class GameFunctions : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CancellationTokenSource CancellationToken;

		public List<Card> PlayersHand = new List<Card>();
		public List<Card> DealersHand = new List<Card>();

		public int DealersHandTotal { get; private set; }
		public int PlayersHandTotal { get; private set; }
		public int DealerGameScore { get; private set; }
		public int PlayerGameScore { get; private set; }
		public int MaxMatchPoint { get; private set; }

        public readonly string PopUpTitle = "New Game";
        public readonly string PopUpInfo = "Please select the number of points you want to play to.";

        public string PlayerScoreText
        {
            get { return _playerScoreText; }
            set
            {
                _playerScoreText = value;
                HandlePropertyChanged(nameof(PlayerScoreText));
            }
        }

        public string DealerScoreText
        {
            get { return _dealerScoreText; }
            set
            {
                _dealerScoreText = value;
                HandlePropertyChanged(nameof(DealerScoreText));
            }
        }

        public string PlayersHandTotalText
        {
            get { return _playersHandTotalText; }
            set
            {
                _playersHandTotalText = value;
                HandlePropertyChanged(nameof(PlayersHandTotalText));
            }
        }

        public string DealersHandTotalText
        {
            get { return _dealersHandTotalText; }
            set
            {
                _dealersHandTotalText = value;
                HandlePropertyChanged(nameof(DealersHandTotalText));
            }
        }

        public string ConvoText
        {
            get { return _convoText; }
            set
            {
                _convoText = value;
                HandlePropertyChanged(nameof(ConvoText));
            }
        }

        public bool ButtonsEnabled
        {
            get { return _buttonsEnabled; }
            set
            {
                _buttonsEnabled = value;
                HandlePropertyChanged(nameof(ButtonsEnabled));
            }
        }

        public bool GameContinues
        {
            get { return _gameContinues; }
            set
            {
                _gameContinues = value;
                HandlePropertyChanged(nameof(GameContinues));
            }
        }

        private PlayingCardDeck Deck;
        private string _playerScoreText;
        private string _dealerScoreText;
        private string _playersHandTotalText;
        private string _dealersHandTotalText;
        private string _convoText;
        private bool _buttonsEnabled;
        private bool _gameContinues;

        public async Task PlayerHit()
		{
			PlayersHand.Add(Deck.RemoveTopCard());
			PlayersHandTotal = UpdateScore(PlayersHand);
            HandlePropertyChanged(nameof(PlayersHand));
            await CheckIfBust();
            await CheckIfPlayerHasFiveCardTrick();
            PlayersHandTotalText = "Players hand total: " + PlayersHandTotal.ToString();
		}

        public async Task PlayerStick()
        {
            ButtonsEnabled = false;

            await DealersTurn();
        }
        
		public void ResetGame()
		{
		    DealersHandTotalText = string.Empty;
		    PlayersHandTotalText = string.Empty;
		    ConvoText = string.Empty;
		    DealerGameScore = 0;
		    PlayerGameScore = 0;
		    DealerScoreText = "Dealers score: " + DealerGameScore.ToString();
		    PlayerScoreText = "Players score: " + PlayerGameScore.ToString();
		}

        public void SetMaxMatchPoint(int maxMatchPoint)
        {
            MaxMatchPoint = maxMatchPoint;
            GameStart();
        }

		private void GameStart()
		{
			Deck = new PlayingCardDeck();

			Deck.Shuffle();

			PlayersHandTotal = 0;
			PlayersHand.Clear();

			DealersHandTotal = 0;
			DealersHand.Clear();

			ButtonsEnabled = true;

			PlayerScoreText = "Players score: " + PlayerGameScore.ToString();
			DealerScoreText = "Dealers score: " + DealerGameScore.ToString();

			PlayersHand.Add(Deck.RemoveTopCard());
			DealersHand.Add(Deck.RemoveTopCard());
			PlayersHand.Add(Deck.RemoveTopCard());
			DealersHand.Add(Deck.RemoveTopCard());

			DealersHandTotalText = "Dealers hand total: " + DealersHandTotal;

            HandlePropertyChanged(nameof(PlayersHand));

			PlayersHandTotal = UpdateScore(PlayersHand);
			PlayersHandTotalText = "Players hand total: " + PlayersHandTotal.ToString();

			ConvoText = "Players turn";
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
        
		private async Task CheckIfBust()
		{
			try
			{
				if (!CancellationToken.IsCancellationRequested)
				{
					if (PlayersHandTotal > 21 && PlayersHandTotal != 100)
					{
						PlayersHandTotal = -1;
						PlayersHandTotalText = "Players hand total: Bust!";
						ButtonsEnabled = false;
						await DealersTurn();
					}

					if (DealersHandTotal > 21 && DealersHandTotal != 100)
					{
						DealersHandTotal = -1;
						DealersHandTotalText = "Dealers hand total: Bust!";
					}
				}
			}
			catch (OperationCanceledException)
			{

			}
		}
        
		private async Task CheckIfPlayerHasFiveCardTrick()
		{
			try
			{
				if (PlayersHand.Count == 5 && PlayersHandTotal != -1 && !CancellationToken.IsCancellationRequested)
				{
					ButtonsEnabled = false;
					PlayersHandTotalText = "Players hand total: Five cards under!";
					PlayersHandTotal = 100;
					await DealersTurn();
				}
			}
			catch (OperationCanceledException)
			{

			}
		}

		private void CheckIfDealerHasFiveCardTrick()
		{
			if (DealersHand.Count == 5 && DealersHandTotal != -1)
			{
				DealersHandTotalText = "Dealers hand total: Five cards under!";
				DealersHandTotal = 100;
			}
		}
        
        private async Task DealersTurn()
		{
			try
			{
				if (!CancellationToken.IsCancellationRequested)
				{
					bool dealersTurn = true;

					await Task.Delay(1000, CancellationToken.Token);
					ConvoText = "Dealers turn";
					await Task.Delay(2000, CancellationToken.Token);

                    HandlePropertyChanged(nameof(DealersHand));
					DealersHandTotal = UpdateScore(DealersHand);
					DealersHandTotalText = "Dealers hand total: " + DealersHandTotal;

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
                            HandlePropertyChanged(nameof(DealersHand));
							DealersHandTotal = UpdateScore(DealersHand);
							DealersHandTotalText = "Dealers hand total: " + DealersHandTotal;
							await CheckIfBust();
							CheckIfDealerHasFiveCardTrick();
						}
						else
						{
							dealersTurn = false;
						}

						await Task.Delay(1000, CancellationToken.Token);
					}

					await UpdateGameScore();
				}
			}
			catch (OperationCanceledException)
			{

			}
		}
        
        private async Task UpdateGameScore()
		{
			try
			{
				if (!CancellationToken.IsCancellationRequested)
				{
					if (PlayersHandTotal > DealersHandTotal)
					{
						PlayerGameScore++;
						PlayerScoreText = "Players score: " + PlayerGameScore.ToString();
						ConvoText = "Players hand wins.";
					}
					else if (PlayersHandTotal < DealersHandTotal)
					{
						DealerGameScore++;
						DealerScoreText = "Dealers score: " + DealerGameScore.ToString();
						ConvoText = "Dealers hand wins.";
					}
					else if (PlayersHandTotal == DealersHandTotal)
					{
						DealerGameScore++;
						ConvoText = "Draw, points go to dealer.";
					}

					await Task.Delay(2000, CancellationToken.Token);
					await CheckIfGameContinues();
				}
			}
			catch (OperationCanceledException)
			{

			}
		}

        private async Task CheckIfGameContinues()
        {
            try
            {
                if (!CancellationToken.IsCancellationRequested)
                {
                    if (PlayerGameScore == MaxMatchPoint || DealerGameScore == MaxMatchPoint)
                    {
                        GameContinues = false;
                    }
                    else
                    {
                        ConvoText = "Next Round!";
                        await Task.Delay(1000, CancellationToken.Token);
                        GameStart();
                    }
                }
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void HandlePropertyChanged(string name)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
	}
}
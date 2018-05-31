using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeckOfCards;

namespace BlackJackIOS
{
	public class GameFunctions
	{
		public CancellationTokenSource CancellationToken;

		private Action<List<Card>> PrintDealersHand;
		private Action<List<Card>> PrintPlayersHand;
        
		private PlayingCardDeck Deck;

		private List<Card> PlayersHand = new List<Card>();
		private List<Card> DealersHand = new List<Card>();

		public int DealersHandTotal { get; private set; }
		public int PlayersHandTotal { get; private set; }
		public int DealerGameScore { get; private set; }
		public int PlayerGameScore { get; private set; }
		public int MaxMatchPoint { get; private set; }

		public string PlayerScoreText { get; private set; }
		public string DealerScoreText { get; private set; }
		public string PlayersHandTotalText { get; private set; }
		public string DealersHandTotalText { get; private set; }
		public string ConvoTextText { get; private set; }

		public readonly string PopUpTitle = "New Game";
		public readonly string PopUpInfo = "Please select the number of points you want to play to.";

		public bool IsHitButtonEnabled { get; private set; }
		public bool IsStickButtonEnabled { get; private set; }
		public bool GameContinues { get; private set; }

		public GameFunctions(Action<List<Card>> printDealersHand, Action<List<Card>> printPlayersHand)
		{
			PrintDealersHand = printDealersHand;
			PrintPlayersHand = printPlayersHand;
		}

		private string PlayerHit()
		{
			PlayersHand.Add(Deck.RemoveTopCard());
			PlayersHandTotal = UpdateScore(PlayersHand);
			return "Players hand total: " + PlayersHandTotal.ToString();
		}
        
		private void ResetGame()
		{
		    DealersHandTotalText = string.Empty;
		    PlayersHandTotalText = string.Empty;
		    ConvoTextText = string.Empty;
		    DealerGameScore = 0;
		    PlayerGameScore = 0;
		    DealerScoreText = "Dealers score: " + DealerGameScore.ToString();
		    PlayerScoreText = "Players score: " + PlayerGameScore.ToString();
		}

		private void GameStart()
		{
			Deck = new PlayingCardDeck();

			Deck.Shuffle();

			PlayersHandTotal = 0;
			PlayersHand.Clear();

			DealersHandTotal = 0;
			DealersHand.Clear();

			IsHitButtonEnabled = true;
			IsStickButtonEnabled = true;

			PlayerScoreText = "Players score: " + PlayerGameScore.ToString();
			DealerScoreText = "Dealers score: " + DealerGameScore.ToString();

			PlayersHand.Add(Deck.RemoveTopCard());
			DealersHand.Add(Deck.RemoveTopCard());
			PlayersHand.Add(Deck.RemoveTopCard());
			DealersHand.Add(Deck.RemoveTopCard());

			DealersHandTotalText = "Dealers hand total: " + DealersHandTotal;

			PrintPlayersHand(PlayersHand);

			PlayersHandTotal = UpdateScore(PlayersHand);
			PlayersHandTotalText = "Players hand total: " + PlayersHandTotal.ToString();

			ConvoTextText = "Players turn";
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
						IsHitButtonEnabled = false;
						IsStickButtonEnabled = false;
						await DealersTurn();
					}

					if (DealersHandTotal > 21 && DealersHandTotal != 100)
					{
						DealersHandTotal = -1;
						DealersHandTotalText = "Dealers hand total: Bust!";
					}
				}
			}
			catch (System.OperationCanceledException)
			{

			}
		}
        
		private async Task CheckIfPlayerHasFiveCardTrick()
		{
			try
			{
				if (PlayersHand.Count == 5 && PlayersHandTotal != -1 && !CancellationToken.IsCancellationRequested)
				{
					IsHitButtonEnabled = false;
					IsStickButtonEnabled = false;
					PlayersHandTotalText = "Players hand total: Five cards under!";
					PlayersHandTotal = 100;
					await DealersTurn();
				}
			}
			catch (System.OperationCanceledException)
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
        
		public async Task DealersTurn()
		{
			try
			{
				if (!CancellationToken.IsCancellationRequested)
				{
					bool dealersTurn = true;

					await Task.Delay(1000, CancellationToken.Token);
					ConvoTextText = "Dealers turn";
					await Task.Delay(2000, CancellationToken.Token);

					PrintDealersHand(DealersHand);
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
							PrintDealersHand(DealersHand);
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
			catch (System.OperationCanceledException)
			{

			}
		}
        
		public async Task UpdateGameScore()
		{
			try
			{
				if (!CancellationToken.IsCancellationRequested)
				{
					if (PlayersHandTotal > DealersHandTotal)
					{
						PlayerGameScore++;
						PlayerScoreText = "Players score: " + PlayerGameScore.ToString();
						ConvoTextText = "Players hand wins.";
					}
					else if (PlayersHandTotal < DealersHandTotal)
					{
						DealerGameScore++;
						DealerScoreText = "Dealers score: " + DealerGameScore.ToString();
						ConvoTextText = "Dealers hand wins.";
					}
					else if (PlayersHandTotal == DealersHandTotal)
					{
						DealerGameScore++;
						ConvoTextText = "Draw, points go to dealer.";
					}

					await Task.Delay(2000, CancellationToken.Token);
					await CheckIfGameContinues();
				}
			}
			catch (System.OperationCanceledException)
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
						ConvoTextText = "Next Round!";
						await Task.Delay(1000, CancellationToken.Token);
						GameStart();
					}
				}
			}
			catch (System.OperationCanceledException)
			{

			}
		}

		public void SetMaxMatchPoint(int maxMatchPoint)
		{
			MaxMatchPoint = maxMatchPoint;
			GameStart();
		}
	}
}
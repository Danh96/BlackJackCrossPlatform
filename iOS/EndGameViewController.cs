using System;
using UIKit;

namespace BlackJackIOS
{
	public partial class EndGameViewController : UIViewController
	{
		public int PlayerGameScore { get; set; }
		public int DealerGameScore { get; set; }

		public EndGameViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad()
		{
			ButtonMainMenu.Layer.CornerRadius = 5;
			ButtonMainMenu.Layer.BorderWidth = 1;
			ButtonMainMenu.Layer.BorderColor = UIColor.White.CGColor;

			ButtonPlayAgain.Layer.CornerRadius = 5;
			ButtonPlayAgain.Layer.BorderWidth = 1;
			ButtonPlayAgain.Layer.BorderColor = UIColor.White.CGColor;

			LabelWinnerText.Font = UIFont.BoldSystemFontOfSize(26);

			SetWinnerText();

			ButtonMainMenu.TouchUpInside += (sender, e) =>
            {
				((UINavigationController)PresentingViewController).ViewControllers = new UIViewController[] { ((UINavigationController)PresentingViewController).ViewControllers[0] };
				                                                  
				PresentingViewController.DismissViewController(true, null);
            };

			ButtonPlayAgain.TouchUpInside += (sender, e) =>
            {
				PresentingViewController.DismissViewController(true, null);
            };
		}
        
		private void SetWinnerText()
        {
			LabelEndGamePoints.Text = $"Players score: {PlayerGameScore.ToString()}     Dealers score: {DealerGameScore.ToString()}";

            if (PlayerGameScore > DealerGameScore)
            {
				LabelWinnerText.Text = "Player wins!";
            }
            else
            {
				LabelWinnerText.Text = "Dealer wins!";
            }
        }
	}
}
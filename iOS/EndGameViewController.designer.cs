// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BlackJack.iOS
{
	[Register ("EndGameViewController")]
	partial class EndGameViewController
	{
		[Outlet]
		UIKit.UIButton ButtonMainMenu { get; set; }

		[Outlet]
		UIKit.UIButton ButtonPlayAgain { get; set; }

		[Outlet]
		UIKit.UILabel LabelEndGamePoints { get; set; }

		[Outlet]
		UIKit.UILabel LabelWinnerText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelEndGamePoints != null) {
				LabelEndGamePoints.Dispose ();
				LabelEndGamePoints = null;
			}

			if (LabelWinnerText != null) {
				LabelWinnerText.Dispose ();
				LabelWinnerText = null;
			}

			if (ButtonMainMenu != null) {
				ButtonMainMenu.Dispose ();
				ButtonMainMenu = null;
			}

			if (ButtonPlayAgain != null) {
				ButtonPlayAgain.Dispose ();
				ButtonPlayAgain = null;
			}
		}
	}
}

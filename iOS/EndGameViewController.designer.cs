// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace BlackJackIOS
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
            if (ButtonMainMenu != null) {
                ButtonMainMenu.Dispose ();
                ButtonMainMenu = null;
            }

            if (ButtonPlayAgain != null) {
                ButtonPlayAgain.Dispose ();
                ButtonPlayAgain = null;
            }

            if (LabelEndGamePoints != null) {
                LabelEndGamePoints.Dispose ();
                LabelEndGamePoints = null;
            }

            if (LabelWinnerText != null) {
                LabelWinnerText.Dispose ();
                LabelWinnerText = null;
            }
        }
    }
}
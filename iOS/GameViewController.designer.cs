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
    [Register ("GameViewController")]
    partial class GameViewController
    {
        [Outlet]
        UIKit.UIButton ButtonHit { get; set; }


        [Outlet]
        UIKit.UIButton ButtonStick { get; set; }


        [Outlet]
        UIKit.UIView DealerFifthCard { get; set; }


        [Outlet]
        UIKit.UIView DealerFirstCard { get; set; }


        [Outlet]
        UIKit.UIView DealerFourthCard { get; set; }


        [Outlet]
        UIKit.UIView DealerSecondCard { get; set; }


        [Outlet]
        UIKit.UIView DealerThirdCard { get; set; }


        [Outlet]
        UIKit.UILabel LabelConvoText { get; set; }


        [Outlet]
        UIKit.UILabel LabelDealerScore { get; set; }


        [Outlet]
        UIKit.UILabel LabelDealersHandTotal { get; set; }


        [Outlet]
        UIKit.UILabel LabelPlayerScore { get; set; }


        [Outlet]
        UIKit.UILabel LabelPlayersHandTotal { get; set; }


        [Outlet]
        UIKit.UIView PlayerFifthCard { get; set; }


        [Outlet]
        UIKit.UIView PlayerFirstCard { get; set; }


        [Outlet]
        UIKit.UIView PlayerFourthCard { get; set; }


        [Outlet]
        UIKit.UIView PlayerSecondCard { get; set; }


        [Outlet]
        UIKit.UIView PlayerThirdCard { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ButtonHit != null) {
                ButtonHit.Dispose ();
                ButtonHit = null;
            }

            if (ButtonStick != null) {
                ButtonStick.Dispose ();
                ButtonStick = null;
            }

            if (DealerFifthCard != null) {
                DealerFifthCard.Dispose ();
                DealerFifthCard = null;
            }

            if (DealerFirstCard != null) {
                DealerFirstCard.Dispose ();
                DealerFirstCard = null;
            }

            if (DealerFourthCard != null) {
                DealerFourthCard.Dispose ();
                DealerFourthCard = null;
            }

            if (DealerSecondCard != null) {
                DealerSecondCard.Dispose ();
                DealerSecondCard = null;
            }

            if (DealerThirdCard != null) {
                DealerThirdCard.Dispose ();
                DealerThirdCard = null;
            }

            if (LabelConvoText != null) {
                LabelConvoText.Dispose ();
                LabelConvoText = null;
            }

            if (LabelDealerScore != null) {
                LabelDealerScore.Dispose ();
                LabelDealerScore = null;
            }

            if (LabelDealersHandTotal != null) {
                LabelDealersHandTotal.Dispose ();
                LabelDealersHandTotal = null;
            }

            if (LabelPlayerScore != null) {
                LabelPlayerScore.Dispose ();
                LabelPlayerScore = null;
            }

            if (LabelPlayersHandTotal != null) {
                LabelPlayersHandTotal.Dispose ();
                LabelPlayersHandTotal = null;
            }

            if (PlayerFifthCard != null) {
                PlayerFifthCard.Dispose ();
                PlayerFifthCard = null;
            }

            if (PlayerFirstCard != null) {
                PlayerFirstCard.Dispose ();
                PlayerFirstCard = null;
            }

            if (PlayerFourthCard != null) {
                PlayerFourthCard.Dispose ();
                PlayerFourthCard = null;
            }

            if (PlayerSecondCard != null) {
                PlayerSecondCard.Dispose ();
                PlayerSecondCard = null;
            }

            if (PlayerThirdCard != null) {
                PlayerThirdCard.Dispose ();
                PlayerThirdCard = null;
            }
        }
    }
}
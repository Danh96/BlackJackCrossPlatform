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
    [Register ("PlayingCard")]
    partial class PlayingCard
    {
        [Outlet]
        UIKit.UIImageView CardBackgroundImage { get; set; }


        [Outlet]
        UIKit.UIImageView CardSuitImage { get; set; }


        [Outlet]
        UIKit.UILabel CardValueBottom { get; set; }


        [Outlet]
        UIKit.UILabel CardValueTop { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CardBackgroundImage != null) {
                CardBackgroundImage.Dispose ();
                CardBackgroundImage = null;
            }

            if (CardSuitImage != null) {
                CardSuitImage.Dispose ();
                CardSuitImage = null;
            }

            if (CardValueBottom != null) {
                CardValueBottom.Dispose ();
                CardValueBottom = null;
            }

            if (CardValueTop != null) {
                CardValueTop.Dispose ();
                CardValueTop = null;
            }
        }
    }
}
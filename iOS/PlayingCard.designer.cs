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

			if (CardValueTop != null) {
				CardValueTop.Dispose ();
				CardValueTop = null;
			}

			if (CardValueBottom != null) {
				CardValueBottom.Dispose ();
				CardValueBottom = null;
			}

			if (CardSuitImage != null) {
				CardSuitImage.Dispose ();
				CardSuitImage = null;
			}
		}
	}
}

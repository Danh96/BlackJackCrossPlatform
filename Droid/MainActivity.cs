using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Content.PM;
using BlackJack.Droid;

namespace BlackJack
{
    [Activity(Theme = "@android:style/Theme.Holo.NoActionBar.Fullscreen", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            Button buttonGame = FindViewById<Button>(Resource.Id.ButtonGame);

            buttonGame.Click += ButtonGame_Click;

        }

        private void ButtonGame_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            StartActivity(intent);
        }
    }
}
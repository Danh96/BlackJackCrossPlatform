using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace BlackJack
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true, Icon = "@drawable/AppLogo", Label = "Black Jack", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { AppStartup(); });
            startupWork.Start();
        }

        async Task AppStartup()
        {
            await Task.Delay(3000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace EventGoApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Window != null)
        {
            // Set the status bar color to white
            Window.SetStatusBarColor(Android.Graphics.Color.White);

            // Ensure status bar icons are dark (for white background)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                Window.InsetsController?.SetSystemBarsAppearance(
                    (int)WindowInsetsControllerAppearance.LightStatusBars, 
                    (int)WindowInsetsControllerAppearance.LightStatusBars);
            }
            else
            {
#pragma warning disable CS0618 // Type or member is obsolete
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
    }
}

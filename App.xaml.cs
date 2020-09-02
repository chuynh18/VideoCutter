using System.Windows;
using VideoCutter.HelperClasses;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //IsoStorageHelper.CreateStorageIfNecessary("preferences.xml");
            PreferencesHelper.LoadPrefs("preferences.xml");

            if (string.IsNullOrWhiteSpace(PreferencesHelper.ffmpegPath) || string.IsNullOrWhiteSpace(PreferencesHelper.ffprobePath))
            {
                FirstTimeSetup.AutoSetupFFMpeg();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            PreferencesHelper.SavePrefs("preferences.xml");
        }
    }
}

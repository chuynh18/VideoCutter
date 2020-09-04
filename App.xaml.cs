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
            PreferencesHelper.LoadPrefs("preferences.xml");

            var ffmpegNotFound = string.IsNullOrWhiteSpace(PreferencesHelper.ffmpegPath);
            var ffprobeNotFound = string.IsNullOrWhiteSpace(PreferencesHelper.ffprobePath);
            var ffplayNotFound = string.IsNullOrWhiteSpace(PreferencesHelper.ffplayPath);

            if (ffmpegNotFound || ffprobeNotFound || ffplayNotFound)
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

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
            IsoStorageHelper.CreateStorageIfNecessary("preferences.xml");
            PreferencesHelper.LoadPrefs("preferences.xml");

            //ffmpegLocation = IsoStorageHelper.ReadStorage("ffmpegLocation.txt");
            //ffprobeLocation = FFMpegHelper.CreateFFProbePath(ffmpegLocation);

            if (string.IsNullOrWhiteSpace(PreferencesHelper.ffmpegPath))
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

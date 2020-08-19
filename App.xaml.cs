using System.Windows;
using VideoCutter.HelperClasses;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string ffmpegLocation { get; set; }
        public string ffprobeLocation { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IsoStorageHelper.CreateStorageIfNecessary("ffmpegLocation.txt");

            ffmpegLocation = IsoStorageHelper.ReadStorage("ffmpegLocation.txt");
            ffprobeLocation = FFMpegHelper.CreateFFProbePath(ffmpegLocation);
        }
    }
}

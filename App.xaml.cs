using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string ffmpegLocation { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IsoStorageHelper.CreateStorageIfNecessary("ffmpegLocation.txt");

            ffmpegLocation = IsoStorageHelper.ReadStorage("ffmpegLocation.txt");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();

            FFMpeg_Location.Text = ((App)Application.Current).ffmpegLocation;
        }

        /// <summary>
        /// Opens a dialog to allow the user to select the FFMpeg executable.  Saves the path
        /// to the executable to the TextBox and to an application-scope variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_FFMpeg(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("\"Select FFMpeg executable\" button clicked.");

            var findFFMpegDialog = new OpenFileDialog();
            findFFMpegDialog.Title = "Select ffmpeg.exe";
            findFFMpegDialog.DefaultExt = ".exe";
            findFFMpegDialog.Filter = "Executables (.exe) | *.exe";

            if (findFFMpegDialog.ShowDialog() == true)
            {
                var pathToFFMpeg = findFFMpegDialog.FileName;
                FFMpeg_Location.Text = pathToFFMpeg;
                ((App)Application.Current).ffmpegLocation = pathToFFMpeg;
                IsoStorageHelper.WriteStorage("ffmpegLocation.txt", pathToFFMpeg);
            }
        }
    }
}

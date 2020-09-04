using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VideoCutter.HelperClasses;

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

            FFMpeg_Location.Text = FFMpegHelper.GetFFMpegPath();
        }

        /// <summary>
        /// Opens a dialog to allow the user to select the FFMpeg executable.  Saves the path
        /// to the executable to the TextBox and to an application-scope variable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_FFMpeg(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("\"Select FFMpeg executable\" button clicked.");

            var findFFMpegDialog = new OpenFileDialog();
            findFFMpegDialog.Title = "Select ffmpeg.exe";
            findFFMpegDialog.DefaultExt = ".exe";
            findFFMpegDialog.Filter = "Executables (.exe) | *.exe";

            if (findFFMpegDialog.ShowDialog() == true)
            {
                var pathToFFMpeg = findFFMpegDialog.FileName;
                FFMpeg_Location.Text = pathToFFMpeg;

                string pathToFFProbe = Modify_Path(pathToFFMpeg, "ffprobe.exe");
                string pathToFFPlay = Modify_Path(pathToFFMpeg, "ffplay.exe");
                PreferencesHelper.setFFMpegLocations(pathToFFMpeg, pathToFFProbe, pathToFFPlay);
            }
        }

        private string Modify_Path(string pathToFFMpeg, string otherExecutable)
        {
            string[] splitPath = pathToFFMpeg.Split(Path.DirectorySeparatorChar);
            splitPath[splitPath.Length - 1] = otherExecutable;
            return string.Join(Path.DirectorySeparatorChar.ToString(), splitPath);
        }

        private void Autodetect_FFMpeg(object sender, RoutedEventArgs e)
        {
            FirstTimeSetup.AutoSetupFFMpeg();
            FFMpeg_Location.Text = PreferencesHelper.ffmpegPath;
        }
    }
}

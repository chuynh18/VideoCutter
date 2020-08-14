using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Cutter.xaml
    /// </summary>
    public partial class Cutter : UserControl
    {
        public Cutter()
        {
            InitializeComponent();

            // default output directory to user's Videos folder
            Output_Dir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        }

        /// <summary>
        /// Allows user to select an input video via dialog box.
        /// The path to the chosen video is placed into the Input_Video TextBox.
        /// A suggested filename is placed into the Output_Video_name TextBox.
        /// </summary>
        private void Select_Video(object sender, RoutedEventArgs e)
        {
            var chooseVideoFileDialog = new OpenFileDialog();
            chooseVideoFileDialog.Title = "Choose a video file";
            chooseVideoFileDialog.Filter = "All files (*.*)|*.*";

            if (chooseVideoFileDialog.ShowDialog() == true)
            {
                var pathToVideo = chooseVideoFileDialog.FileName;
                Input_Video.Text = pathToVideo;
                Autofill_Suggested_Name(Output_Video_Name, pathToVideo);
            }
        }

        /// <summary>
        /// Handles DropEvent when user drags and drops video file onto the application window when the Cut video tab is active
        /// </summary>
        private void HandleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 1)
                {
                    string warningText = "You dragged multiple files.  A file has been selected but may not be your desired file.  Please drag your desired file onto VideoCutter.";
                    string title = "Multiple files were dropped";

                    MessageBox.Show(warningText, title, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                var path = files[0];

                Input_Video.Text = path;
                Autofill_Suggested_Name(Output_Video_Name, path);
            }
        }

        /// <summary>
        /// Places path to selected output directory into the Output_Dir TextBox.
        /// 
        /// WARNING!  Has a dirty hack to make OpenFileDialog accept folders.
        /// </summary>
        private void Select_Output_Folder(object sender, RoutedEventArgs e)
        {
            var chooseOutputFolderDialog = new OpenFileDialog();

            var USER_INSTRUCTIONS = "Select a folder";

            // hacky - disables validation and file existence check, so user can click OK despite not selecting a real file
            chooseOutputFolderDialog.ValidateNames = false;
            chooseOutputFolderDialog.CheckFileExists = false;
            chooseOutputFolderDialog.CheckPathExists = true;

            // pre-fills in text, forcing OpenFileDialog to select a nonexistent file
            chooseOutputFolderDialog.FileName = USER_INSTRUCTIONS;

            if (chooseOutputFolderDialog.ShowDialog() == true)
            {
                var outputPath = chooseOutputFolderDialog.FileName;

                // remove nonexistent file name from path, resulting in a path to a directory
                var splitOutputPath = outputPath.Split('\\');
                var fakeFileName = splitOutputPath[splitOutputPath.Length - 1];
                var correctedOutput = outputPath.Substring(0, outputPath.Length - fakeFileName.Length);

                Output_Dir.Text = correctedOutput;
            }
        }

        /// <summary>
        /// Does string manupulation on an absolute path to a file to grab the filename extension.
        /// </summary>
        /// <param name="path">
        /// An absolute path to a file.
        /// </param>
        /// <returns>
        /// The filename extension of the input file.
        /// </returns>
        private string Get_File_Extension(string path)
        {
            string[] splitPath = path.Split('\\');
            string fileName = splitPath[splitPath.Length - 1];
            string[] splitFileName = fileName.Split('.');
            string fileExtension = splitFileName[splitFileName.Length - 1];

            return fileExtension;
        }

        private string Create_Suggested_Name(string fileExtension)
        {
            string HARDCODED_FILENAME = "clipped_video";

            return HARDCODED_FILENAME + "." + fileExtension;
        }


        /// <summary>
        /// Populates a TextBox with a suggested output file name.  The file extension of
        /// the output file is the same as the extension on the input file.  The base name
        /// is a hardcoded string in Create_Suggested_Name().
        /// </summary>
        /// <param name="textBox">
        /// The target TextBox to populate
        /// </param>
        /// <param name="path">
        /// The absolute path to the target file.  In this case, it's provided by user input
        /// into the Input_Video TextBox.
        /// </param>
        private void Autofill_Suggested_Name(TextBox textBox, string path)
        {
            var fileExtension = Get_File_Extension(path);
            var suggestedName = Create_Suggested_Name(fileExtension);

            textBox.Text = suggestedName;
        }

        /// <summary>
        /// Grabs user inputs from the various fields in Cutter.xaml,
        /// builds the arguments list, and calls FFMpeg.
        /// </summary>
        private void Cut_Video(object sender, RoutedEventArgs e)
        {
            var startTime = Start_Time.Text;
            var endTime = End_Time.Text;

            var durationAsFloat = Convert.ToDouble(endTime) - Convert.ToDouble(startTime);
            var duration = Convert.ToString(durationAsFloat);

            var input = Input_Video.Text;
            var output = Output_Dir.Text + "\\" + Output_Video_Name.Text;
        }


    }
}

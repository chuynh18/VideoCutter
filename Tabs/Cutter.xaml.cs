using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
                UpdateUIAfterVideoSelected(pathToVideo);
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

                UpdateUIAfterVideoSelected(path);
            }
        }

        /// <summary>
        /// Shared code for updating the VideoCutter UI after a video is selected.
        /// Used by both Select_Video() and HandleDrop().
        /// </summary>
        /// <param name="path"></param>
        private void UpdateUIAfterVideoSelected(string path)
        {
            Input_Video.Text = path;
            Autofill_Suggested_Name(Output_Video_Name, path);
            End_Time.Text = FFMpegHelper.GetVideoDuration(path);
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
                var splitOutputPath = outputPath.Split(Path.DirectorySeparatorChar);
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
            string[] splitPath = path.Split(Path.DirectorySeparatorChar);
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
        /// Cuts video.  Grabs all necessary user inputs from the UI to build the appropriate arguments.
        /// Calls ffmpeg.  If the output filename is already taken, asks the user whether they wish to overwrite.
        /// </summary>
        private void Cut_Video(object sender, RoutedEventArgs e)
        {
            var directorySeparatorCharacter = char.ToString(Path.DirectorySeparatorChar);

            var startTime = Start_Time.Text;
            var endTime = End_Time.Text;

            var input = Input_Video.Text;
            var output = Output_Dir.Text + directorySeparatorCharacter + Output_Video_Name.Text;

            if (File.Exists(output))
            {
                MessageBoxResult result = MessageBox.Show(
                    "There is already a video named \"" + Output_Video_Name.Text + "\".  Would you like to overwrite this video?",
                    "Overwrite this video?",
                    MessageBoxButton.OKCancel
                    );

                if (result == MessageBoxResult.Cancel) {
                    return;
                }
            }

            FFMpegHelper.CutVideo(input, output, startTime, endTime);

            if (Open_When_Finished.IsChecked == true)
            {
                DirectoryOperationsHelper.OpenFolderAndHighlightFile(output);
            }
        }

        /// <summary>
        /// Opens the folder whose path is currently listed in the Output_Dir TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Destination_Folder(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Open Destination Folder button clicked");
            DirectoryOperationsHelper.OpenFolder(Output_Dir.Text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using VideoCutter.HelperClasses;

namespace VideoCutter
{
    /// <summary>
    /// Interaction logic for Cutter.xaml
    /// </summary>
    public partial class Cutter : UserControl
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public Cutter()
        {
            InitializeComponent();
            Load_Cutter_Prefs();
            Save_Cutter_Prefs();

            worker.DoWork += new DoWorkEventHandler(Worker_Cut_Video);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_Complete);
        }

        private void Select_Video(object sender, RoutedEventArgs e)
        {
            SelectFileHelper.Select_Video(UpdateUIAfterVideoSelected);
        }

        private void HandleDrop(object sender, DragEventArgs e)
        {
            SelectFileHelper.HandleDrop(e, UpdateUIAfterVideoSelected);
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

        private void Select_Output_Folder(object sender, RoutedEventArgs e)
        {
            SelectFileHelper.Select_Output_Folder(Output_Dir, Save_Cutter_Prefs);
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
            var HARDCODED_OUTPUT_NAME = "clipped_video";
            return HARDCODED_OUTPUT_NAME + "." + fileExtension;
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
            var startTime = Start_Time.Text;
            var endTime = End_Time.Text;

            var input = Input_Video.Text;
            var output = Output_Dir.Text + Path.DirectorySeparatorChar + Output_Video_Name.Text;

            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show(
                    "Please select a video to cut by either clicking the \"Choose Video\" button, or dragging and dropping a video file. ",
                    "Please select a video first",
                    MessageBoxButton.OK
                    );

                return;
            }

            if (!File.Exists(input))
            {
                MessageBox.Show(
                    "The input video was not found.  Ensure that it has not been moved or deleted, or select another video to cut.",
                    "Input video not found",
                    MessageBoxButton.OK
                    );

                return;
            }

            if (!Directory.Exists(Output_Dir.Text))
            {
                MessageBox.Show(
                    "The specified output directory does not exist.  Please create it or choose another output location.",
                    "Output directory not found",
                    MessageBoxButton.OK
                    );

                Output_Dir.Select(0, Output_Dir.Text.Length - 1);

                return;
            }

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

            Cut_Video_Button.Content = "Cutting...";
            Cut_Video_Button.IsEnabled = false;

            List<object> cutArguments = new List<object>();
            cutArguments.Add(input);
            cutArguments.Add(output);
            cutArguments.Add(startTime);
            cutArguments.Add(endTime);

            worker.RunWorkerAsync(cutArguments);
            Save_Cutter_Prefs();
        }

        /// <summary>
        /// Opens the folder whose path is currently listed in the Output_Dir TextBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Destination_Folder(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Open Destination Folder button clicked");
            DirectoryOperationsHelper.OpenFolder(Output_Dir.Text);
        }

        private void Save_Cutter_Prefs()
        {
            PreferencesHelper.cutOutputDir = Output_Dir.Text;

            if (Open_When_Finished.IsChecked == true)
            {
                PreferencesHelper.cutHighlightOnCompletion = true;
            }
            else
            {
                PreferencesHelper.cutHighlightOnCompletion = false;
            }        
        }

        /// <summary>
        /// Used by event handlers for saving prefs e.g. saving prefs when user clicks a UI element
        /// </summary>
        private void Save_Cutter_Prefs(object sender, RoutedEventArgs e)
        {
            Save_Cutter_Prefs();
        }

        private void Load_Cutter_Prefs()
        {
            if (string.IsNullOrWhiteSpace(PreferencesHelper.cutOutputDir))
            {
                Output_Dir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }
            else
            {
                Output_Dir.Text = PreferencesHelper.cutOutputDir;
            }

            if (PreferencesHelper.cutHighlightOnCompletion == true)
            {
                Open_When_Finished.IsChecked = true;
            }
            else
            {
                Open_When_Finished.IsChecked = false;
            }
        }

        /// <summary>
        /// Saves preferences when the user types in a valid output directory, then clicks away from the input box
        /// </summary>
        private void Output_Dir_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Output_Dir.Text))
            {
                Save_Cutter_Prefs();
            }
        }

        /// <summary>
        /// Cut video method for BackgroundWorker
        /// </summary>
        private void Worker_Cut_Video(object sender, DoWorkEventArgs e)
        {
            List<object> arguments = e.Argument as List<object>;
            string input = (string)arguments[0];
            string output = (string)arguments[1];
            string startTime = (string)arguments[2];
            string endTime = (string)arguments[3];

            FFMpegHelper.CutVideo(input, output, startTime, endTime);
        }

        /// <summary>
        /// Runs when BackgroundWorker finishes cutting video.  Re-enables cut button.
        /// </summary>
        private void Worker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            Cut_Video_Button.Content = "Cut video";
            Cut_Video_Button.IsEnabled = true;

            if (Open_When_Finished.IsChecked == true)
            {
                var outputFile = Output_Dir.Text + Path.DirectorySeparatorChar + Output_Video_Name.Text;
                DirectoryOperationsHelper.OpenFolderAndHighlightFile(outputFile);
            }
        }
    }
}

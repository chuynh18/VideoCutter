using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace VideoCutter.HelperClasses
{
    class SelectFileHelper
    {
        /// <summary>
        /// Allows user to select an input video via dialog box.
        /// The path to the chosen video is placed into the Input_Video TextBox.
        /// A suggested filename is placed into the Output_Video_name TextBox.
        /// </summary>
        public static void Select_Video(Action<string> updateUI)
        {
            var chooseVideoFileDialog = new OpenFileDialog();
            chooseVideoFileDialog.Title = "Choose a video file";
            chooseVideoFileDialog.Filter = "All files (*.*)|*.*";

            if (chooseVideoFileDialog.ShowDialog() == true)
            {
                var pathToVideo = chooseVideoFileDialog.FileName;
                updateUI(pathToVideo);
            }
        }

        /// <summary>
        /// Handles DropEvent when user drags and drops video file onto the application window when the Cut video tab is active
        /// </summary>
        public static void HandleDrop(DragEventArgs e, Action<string> updateUI)
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

                updateUI(path);
            }
        }

        /// <summary>
        /// Places path to selected output directory into the targetTextBox TextBox.
        /// 
        /// WARNING!  Has a dirty hack to make OpenFileDialog accept folders.
        /// </summary>
        public static void Select_Output_Folder(TextBox targetTextBox, Action callback)
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

                targetTextBox.Text = correctedOutput;

                callback();
            }
        }

        public static void Select_Output_Folder(TextBox targetTextBox)
        {

            Select_Output_Folder(targetTextBox, Dummy_Callback);
        }

        private static void Dummy_Callback()
        {

        }
    }
}

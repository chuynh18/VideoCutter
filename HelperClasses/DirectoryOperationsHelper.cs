using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace VideoCutter
{
    class DirectoryOperationsHelper
    {
        /// <summary>
        /// Opens the directory at folderPath in Windows Explorer.
        /// If the folder is already open, the existing window is brought to the front.
        /// </summary>
        /// <param name="folderPath">
        /// The path of the directory to open.
        /// </param>
        public static void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                Process.Start("file://" + folderPath);
            }
            else
            {
                MessageBox.Show("The folder " + folderPath + " does not exist.");
            }
        }

        /// <summary>
        /// Opens a new Windows Explorer window and highlights the output file.
        /// This will open a new window even when the output folder is already open.
        /// </summary>
        /// <param name="filePath">
        /// The path to the file to highlight.
        /// </param>
        public static void OpenFolderAndHighlightFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string arg = "/select, \"" + filePath + "\"";

                Process.Start("explorer", arg);
            }
             else
            {
                MessageBox.Show("The file " + filePath + " does not exist.");
            }
        }
    }
}

﻿using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

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
            Console.WriteLine("\"Select FFMpeg executable\" button clicked.");

            var findFFMpegDialog = new OpenFileDialog();
            findFFMpegDialog.Title = "Select ffmpeg.exe";
            findFFMpegDialog.DefaultExt = ".exe";
            findFFMpegDialog.Filter = "Executables (.exe) | *.exe";

            if (findFFMpegDialog.ShowDialog() == true)
            {
                var pathToFFMpeg = findFFMpegDialog.FileName;
                FFMpeg_Location.Text = pathToFFMpeg;
                FFMpegHelper.SetFFMpegPath(pathToFFMpeg);
                FFMpegHelper.SetFFProbePath(FFMpegHelper.CreateFFProbePath(pathToFFMpeg));
                IsoStorageHelper.WriteStorage("ffmpegLocation.txt", pathToFFMpeg);
            }
        }
    }
}
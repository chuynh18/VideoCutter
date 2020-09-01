using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace VideoCutter.HelperClasses
{
    class FirstTimeSetup
    {
        class FFMpegLocation
        {
            public readonly string path;

            public readonly string[] ffmpeg;

            public readonly string[] ffprobe;

            public FFMpegLocation(string path, string[] ffmpeg, string[] ffprobe)
            {
                this.path = path;
                this.ffmpeg = ffmpeg;
                this.ffprobe = ffprobe;
            }
        }

        /// <summary>
        /// Searches several predefined locations for existing installations of FFMpeg,
        /// then selects the latest version available.
        /// 
        /// Locations searched are Program Files and Program Files (x86) on fixed disks,
        /// the user's home directory, and VideoCutter's own directory.
        /// 
        /// This method should only be called when necessary, as it may attempt to access
        /// idling hard drives, causing the application to feel unresponsive.
        /// </summary>
        public static void AutoSetupFFMpeg()
        {
            List<FFMpegLocation> ffmpegLocations = FindFFMpegDirectories();

            foreach (FFMpegLocation location in ffmpegLocations)
            {
                Debug.WriteLine("Path to ffmpeg directory: " + location.path);
                Debug.WriteLine("Path to ffmpeg: " + location.ffmpeg[0]);
                Debug.WriteLine("Path to ffprobe: " + location.ffprobe[0] + "\n");
            }

            if (ffmpegLocations.Count == 0)
            {
                Debug.WriteLine("No installations of FFMpeg were found.");
            }
            else
            {
                FFMpegLocation latestFFMpeg = GetLatestFFMpeg(ffmpegLocations);
                Debug.WriteLine("Path to latest ffmpeg directory: " + latestFFMpeg.path);
                FFMpegHelper.SetFFMpegPath(latestFFMpeg.ffmpeg[0]);
                FFMpegHelper.SetFFProbePath(latestFFMpeg.ffprobe[0]);
            }
        }

        /// <summary>
        /// Checks local drives' Program Files and Program Files (x86) folders, user's home directory,
        /// and VideoCutter's directory for ffmpeg executables.
        /// </summary>
        /// <returns>
        /// Returns a list of ffmpeg installations.
        /// </returns>
        private static List<FFMpegLocation> FindFFMpegDirectories()
        {
            var ffmpegLocations = new List<FFMpegLocation>();
            DriveInfo[] drives = DriveInfo.GetDrives();

            // check "Program Files" and "Program Files (x86)" 
            foreach (DriveInfo drive in drives)
            {
                if (drive.DriveType == DriveType.Fixed)
                {
                    string[] locationsToCheck = Directory.GetDirectories(drive.Name, "Program Files*");

                    foreach (string location in locationsToCheck)
                    {
                        ffmpegLocations.AddRange(CheckDirectoryForFFMpeg(location));
                    }
                }
            }

            // check VideoCutter's own directory
            ffmpegLocations.AddRange(
                CheckDirectoryForFFMpeg(AppDomain.CurrentDomain.BaseDirectory)
            );

            // check user folder
            ffmpegLocations.AddRange(
                CheckDirectoryForFFMpeg(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                )
            );

            return ffmpegLocations;
        }

        /// <summary>
        /// Recursively searches the directory at pathToDir for ffmpeg.exe and ffprobe.exe.
        /// First, searches for directories containing "ffmpeg" within the search directory.
        /// Then searches those directories for the presence of ffmpeg.exe and ffprobe.exe.
        /// </summary>
        /// <param name="pathToDir">
        /// The path to the directory to search.
        /// </param>
        /// <returns>
        /// A list of paths to directories that contain ffmpeg.exe and ffprobe.exe.
        /// If no such directories were found, returns an empty list.
        /// </returns>
        private static List<FFMpegLocation> CheckDirectoryForFFMpeg(string pathToDir)
        {
            var ffmpegLocations = new List<FFMpegLocation>();

            if (Directory.Exists(pathToDir))
            {
                var ffmpegCandidates = Directory.GetDirectories(pathToDir, "*ffmpeg*");

                foreach (var candidate in ffmpegCandidates)
                {
                    var ffmpeg = Directory.GetFiles(candidate, "ffmpeg.exe", SearchOption.AllDirectories);
                    var ffprobe = Directory.GetFiles(candidate, "ffprobe.exe", SearchOption.AllDirectories);

                    if (ffmpeg.Length > 0 && ffprobe.Length > 0)
                    {
                        ffmpegLocations.Add(new FFMpegLocation(candidate, ffmpeg, ffprobe));
                    }
                }
            }

            return ffmpegLocations;
        }

        private static FFMpegLocation GetLatestFFMpeg(List<FFMpegLocation> ffmpegFolders)
        {
            int index = 0;
            int indexOfLatestFFMpeg = 0;
            DateTime modification = new DateTime();

            foreach (var folder in ffmpegFolders)
            {
                DateTime ffmpegModification = File.GetLastWriteTime(folder.ffmpeg[0]);

                if (DateTime.Compare(ffmpegModification, modification) > 0)
                {
                    indexOfLatestFFMpeg = index;
                    modification = ffmpegModification;
                }

                index++;
            }

            return ffmpegFolders[indexOfLatestFFMpeg];
        }
    }
}
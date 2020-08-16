using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace VideoCutter
{
    /// <summary>
    /// Handles all things FFMpeg:  calling the exe, building the arguments, etc.
    /// </summary>
    class FFMpegHelper
    {
        public static string GetFFMpegPath()
        {
            if (File.Exists(((App)Application.Current).ffmpegLocation))
            {
                return ((App)Application.Current).ffmpegLocation;
            }
            else
            {
                throw new FileNotFoundException("FFMpeg not found.  Please select ffmpeg.exe in the Settings tab.");
            }
        }

        public static void SetFFMpegPath(string ffmpegPath)
        {
            ((App)Application.Current).ffmpegLocation = ffmpegPath;
        }

        /// <summary>
        /// Does string manipulation to the path of ffmpeg
        /// to create a string representing the path to ffprobe.
        /// </summary>
        /// <param name="ffmpegPath"></param>
        /// <returns></returns>
        public static string CreateFFProbePath(string ffmpegPath)
        {
            string[] ffmpegPathList = ffmpegPath.Split('\\');
            ffmpegPathList[ffmpegPathList.Length - 1] = "ffprobe.exe";

            return string.Join("\\", ffmpegPathList);
        }

        public static string GetFFProbePath()
        {
            if (File.Exists(((App)Application.Current).ffprobeLocation))
            {
                return ((App)Application.Current).ffprobeLocation;
            }
            else
            {
                throw new FileNotFoundException("FFProbe not found.  Please select ffprobe.exe in the Settings tab.");
            }
        }

        public static void SetFFProbePath(string ffprobePath)
        {
            ((App)Application.Current).ffprobeLocation = ffprobePath;
        }

        /// <summary>
        /// Runs a target executable.
        /// </summary>
        /// <param name="exePath">
        /// Path to an executable
        /// </param>
        /// <param name="args">
        /// List of arguments to pass to the executable
        /// </param>
        /// <returns></returns>
        private static string Execute(string exePath, string args)
        {
            Console.WriteLine("running Execute(" + exePath + " " + args + ")");

            var result = string.Empty;

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = args;
                p.Start();
                p.WaitForExit();

                result = p.StandardOutput.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// Uses ffprobe to retrieve the duration (in seconds) of a video file
        /// </summary>
        /// <param name="videoPath">
        /// Path to a video file
        /// </param>
        /// <returns>
        /// The duration of the target video file in seconds.
        /// </returns>
        public static string GetVideoDuration(string videoPath)
        {
            var ffprobePath = GetFFProbePath();
            var VIDEO_LENGTH_ARGUMENTS = "-i \"" + videoPath + "\" -show_entries format=duration -v quiet -of csv=\"p=0\"";
            var videoLength = Execute(ffprobePath, VIDEO_LENGTH_ARGUMENTS);

            Console.WriteLine("Video length is: " + videoLength);
            return videoLength.Trim().Trim('0');
        }

        /// <summary>
        /// Calls ffmpeg to cut a target video file
        /// </summary>
        /// <param name="videoPath">
        /// Path to input video file
        /// </param>
        /// <param name="outputPath">
        /// Path to output video file (the file that will be created)
        /// </param>
        /// <param name="startTime">
        /// Timestamp (in seconds) from which to start cutting
        /// </param>
        /// <param name="endTime">
        /// Timestamp (in seconds) at which to end the cut video
        /// </param>
        public static void CutVideo(string videoPath, string outputPath, string startTime, string endTime)
        {
            var ffmpegPath = GetFFMpegPath();
            var VIDEO_CUT_ARGUMENTS = "-i \"" + videoPath + "\" -ss " + startTime + " -c copy -to " + endTime + " \"" + outputPath + "\" -y";

            Execute(ffmpegPath, VIDEO_CUT_ARGUMENTS);
        }
    }
}

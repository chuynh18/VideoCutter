using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        private static string Execute(string exePath, string args)
        {
            Console.WriteLine("running Execute(" + exePath + ", " + args + ")");

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

        public static string GetVideoDuration(string videoPath)
        {
            var ffprobePath = GetFFProbePath();
            var VIDEO_LENGTH_ARGUMENTS = "-i \"" + videoPath + "\" -show_entries format=duration -v quiet -of csv=\"p=0\"";
            var videoLength = Execute(ffprobePath, VIDEO_LENGTH_ARGUMENTS);

            Console.WriteLine("Video length is: " + videoLength);
            return videoLength.Trim().Trim('0');
        }

        public static void CutVideo(string videoPath, string outputPath, string startTime, string endTime)
        {
            var ffmpegPath = GetFFMpegPath();
            var VIDEO_CUT_ARGUMENTS = "-i \"" + videoPath + "\" -ss " + startTime + " -c copy -t " + endTime + " \"" + outputPath + "\"";

            Execute(ffmpegPath, VIDEO_CUT_ARGUMENTS);
        }
    }
}

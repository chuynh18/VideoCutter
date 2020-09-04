using System.Diagnostics;
using System.IO;
using VideoCutter.HelperClasses;

namespace VideoCutter
{
    /// <summary>
    /// Handles all things FFMpeg:  calling the exe, building the arguments, etc.
    /// </summary>
    class FFMpegHelper
    {
        public static string GetFFMpegPath()
        {
            if (File.Exists(PreferencesHelper.ffmpegPath))
            {
                return PreferencesHelper.ffmpegPath;
            }
            else
            {
                throw new FileNotFoundException("FFMpeg not found.  Please select ffmpeg.exe in the Settings tab.");
            }
        }

        public static string GetFFProbePath()
        {
            if (File.Exists(PreferencesHelper.ffprobePath))
            {
                return PreferencesHelper.ffprobePath;
            }
            else
            {
                throw new FileNotFoundException("FFProbe not found.  Please select ffprobe.exe in the Settings tab.");
            }
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
        /// <returns>
        /// Standard output of the executed program.
        /// </returns>
        private static string Execute(string exePath, string args)
        {
            Debug.WriteLine("running Execute(" + exePath + " " + args + ")");

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
        /// The duration of the target video file in sexagesimal format (h:mm:ss.microseconds).
        /// </returns>
        public static string GetVideoDuration(string videoPath)
        {
            var ffprobePath = GetFFProbePath();
            var VIDEO_LENGTH_ARGUMENTS = "-i \"" + videoPath + "\" -sexagesimal -show_entries format=duration -v quiet -of csv=\"p=0\"";
            var videoLength = Execute(ffprobePath, VIDEO_LENGTH_ARGUMENTS);
            var simplifiedVideoLength = TimeHelper.SimplifyTimestamp(videoLength);

            Debug.WriteLine("Video length is: " + simplifiedVideoLength);
            return simplifiedVideoLength.Trim();
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

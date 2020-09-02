using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace VideoCutter.HelperClasses
{
    class PreferencesHelper
    {
        [Serializable()]
        class SerializablePrefs
        {
            public string ffmpegPath { get; set; }

            public string ffprobePath { get; set; }

            public string cutOutputDir { get; set; }

            public bool cutHighlightOnCompletion { get; set; }
        }

        public static string ffmpegPath { get; set; }

        public static string ffprobePath { get; set; }

        public static string cutOutputDir { get; set; }

        public static bool cutHighlightOnCompletion { get; set; }

        public static void SavePrefs(string fileName)
        {
            SerializablePrefs prefs = new SerializablePrefs();

            prefs.ffmpegPath = ffmpegPath.Trim();
            prefs.ffprobePath = ffprobePath.Trim();
            prefs.cutOutputDir = cutOutputDir.Trim();
            prefs.cutHighlightOnCompletion = cutHighlightOnCompletion;

            var userStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            try
            {
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.Open, userStore))
                {
                    SoapFormatter serializer = new SoapFormatter();
                    serializer.Serialize(stream, prefs);
                }
            } catch (FileNotFoundException e)
            {
                Debug.WriteLine("Somehow, preferences.xml was not found.");
                Debug.WriteLine(e);
            }
        }

        public static void LoadPrefs(string fileName)
        {
            var userStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            try
            {
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, userStore))
                {
                    if (stream.Length > 0)
                    {
                        var formatter = new SoapFormatter();
                        var prefs = (SerializablePrefs)formatter.Deserialize(stream);

                        ffmpegPath = prefs.ffmpegPath.Trim();
                        ffprobePath = prefs.ffprobePath.Trim();
                        cutOutputDir = prefs.cutOutputDir.Trim();
                        cutHighlightOnCompletion = prefs.cutHighlightOnCompletion;
                    }
                }
            } catch (FileNotFoundException e)
            {
                Debug.WriteLine("Somehow, preferences.xml was not found.");
                Debug.WriteLine(e);
            }
        }
    }
}

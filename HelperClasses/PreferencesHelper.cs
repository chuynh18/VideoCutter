using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace VideoCutter.HelperClasses
{
    public class PreferencesHelper
    {
        [Serializable]
        public class SerializablePrefs
        {
            public string ffmpegPath { get; set; }

            public string ffprobePath { get; set; }

            public string ffplayPath { get; set; }

            public string cutOutputDir { get; set; }

            public bool cutHighlightOnCompletion { get; set; }
        }

        public static string ffmpegPath { get; set; }

        public static string ffprobePath { get; set; }

        public static string ffplayPath { get; set; }

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
                using (var stream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, userStore))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SerializablePrefs));
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
                        XmlSerializer serializer = new XmlSerializer(typeof(SerializablePrefs));
                        serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                        serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                        try
                        {
                            var prefs = (SerializablePrefs)serializer.Deserialize(stream);

                            ffmpegPath = string.IsNullOrEmpty(prefs.ffmpegPath) ? "" : prefs.ffmpegPath.Trim();
                            ffprobePath = string.IsNullOrEmpty(prefs.ffprobePath) ? "" : prefs.ffprobePath.Trim();
                            cutOutputDir = string.IsNullOrEmpty(prefs.cutOutputDir) ? "" : prefs.cutOutputDir.Trim();
                            cutHighlightOnCompletion = prefs.cutHighlightOnCompletion;
                        }
                        catch (InvalidOperationException e) 
                        {
                            Debug.WriteLine("Somehow, preferences.xml was malformed.  It will be deleted.");
                            Debug.WriteLine(e);
                            stream.Dispose();
                            userStore.DeleteFile(fileName);
                        }
                        
                    }
                }
            } catch (FileNotFoundException e)
            {
                Debug.WriteLine("Somehow, preferences.xml was not found.");
                Debug.WriteLine(e);
            }
        }

        private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Debug.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            Debug.WriteLine("Unknown Attribute: " + e.Attr.Name + " = " + e.Attr.Value);
        }

        public static void setFFMpegLocations(string ffmpegPath, string ffprobePath, string ffplayPath)
        {
            Debug.WriteLine("Path to FFMpeg set to: " + ffmpegPath);
            Debug.WriteLine("Path to FFProbe set to: " + ffprobePath);
            Debug.WriteLine("Path to FFPlay set to: " + ffplayPath);

            PreferencesHelper.ffmpegPath = ffmpegPath;
            PreferencesHelper.ffprobePath = ffprobePath;
            PreferencesHelper.ffplayPath = ffplayPath;
        }
    }
}

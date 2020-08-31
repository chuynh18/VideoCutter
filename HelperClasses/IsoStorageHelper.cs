using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;

namespace VideoCutter
{
    /// <summary>
    /// Static methods for reading and writing text to isolated storage.
    /// </summary>
    class IsoStorageHelper
    {
        static public void CreateStorageIfNecessary(string fileName)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (!isoStore.FileExists(fileName))
            {
                Debug.WriteLine(fileName + " does not exist.");

                new IsolatedStorageFileStream(fileName,
                    FileMode.CreateNew,
                    isoStore);

                Debug.WriteLine(fileName + " has been created.");
            }
            else
            {
                Debug.WriteLine(fileName + " already exists.  No action will be taken.");
            }
        }

        static public void WriteStorage(string fileName, string content)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(fileName))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isoStore))
                {
                    using (StreamWriter writer = new StreamWriter(isoStream))
                    {
                        writer.WriteLine(content.Trim());
                        Debug.WriteLine("You have written the following to the file:");
                        Debug.WriteLine(content);
                    }
                }
            }
        }

        static public string ReadStorage(string fileName)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (isoStore.FileExists(fileName))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        return reader.ReadToEnd().Trim();
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(fileName + " not found.");
            }
        }
    }
}

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
    }
}

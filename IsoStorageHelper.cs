using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Console.WriteLine(fileName + " does not exist.");

                new IsolatedStorageFileStream(fileName, FileMode.CreateNew, isoStore);

                Console.WriteLine(fileName + " has been created.");
            }
            else
            {
                Console.WriteLine(fileName + " already exists.  No action will be taken.");
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
                        Console.WriteLine("You have written the following to the file:");
                        Console.WriteLine(content);
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
                        string content = reader.ReadToEnd().Trim();

                        Console.WriteLine("Reading contents:");
                        Console.WriteLine(content);

                        return content;
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

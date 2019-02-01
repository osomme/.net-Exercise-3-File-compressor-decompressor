using Windows.Storage;

namespace Exercise3.UWP.Utils
{
    public static class StorageFileExtensions
    {

        /// <summary>On compressed files, gets the original filename without the zip extension.</summary>
        /// <param name="file">The file.</param>
        /// <returns>A string which contains the name of the file inside the compressed folder</returns>
        public static string GetNameWithoutZipExtension(this StorageFile file)
        {                                    
            var indexOfLastDot = file.Name.LastIndexOf('.');
            return file.Name.Remove(indexOfLastDot);
        }
    }
}

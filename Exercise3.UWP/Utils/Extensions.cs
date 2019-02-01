using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Exercise3.UWP.Utils
{
    public static class Extensions
    {

        /// <summary>On compressed files, gets the original filename without the zip extension.</summary>
        /// <param name="file">The file.</param>
        /// <returns>A string which contains the name of the file inside the compressed folder</returns>
        public static string GetNameWithoutZipExtension(this StorageFile file)
        {
            var splitName = file.Name.Split(".");
            var result = "";

            for (var i = 0; i < splitName.Length - 1; i++)
            {
                if (i == splitName.Length - 2)
                {
                    result += $".{splitName[i]}";
                }
                else
                {
                    result += splitName[i];
                }                
            }

            return result;
        }
    }
}

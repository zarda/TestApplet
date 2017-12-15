using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    /// <summary>
    /// A helper class to query information about directories
    /// </summary>
    class DirectoryStructure
    {
        /// <summary>
        /// Get all logical drives on computer
        /// </summary>
        /// <returns></returns>
        public static List<DirectoryItem> GetLogicalDrives()
        {
            // Get every logical drive on the machine
            return Directory.GetLogicalDrives().Select(drive => new DirectoryItem { FullPath = drive, Type = DirectoryItemType.Drive }).ToList();
        }

        /// <summary>
        /// Get the directories top-level content
        /// </summary>
        /// <param name="fullPath">The full path to the directory</param>
        /// <returns></returns>
        public static List<DirectoryItem> GetDirectoryContents(string fullPath)
        {
            // Create an empty list
            var items = new List<DirectoryItem>();

            #region Get Folders

            // Try and get directories from the folder
            // igoring any issues doing so
            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                {
                    items.AddRange(dirs.Select(dir => new DirectoryItem { FullPath = dir, Type = DirectoryItemType.Folder }));
                }

            }
            catch { }
                        
            #endregion

            #region Get Files            

            // Try and get files from the folder
            // igoring any issues doing so
            try
            {
                var fs = Directory.GetFiles(fullPath);

                if (fs.Length > 0)
                {
                    items.AddRange(fs.Select(file => new DirectoryItem { FullPath = file, Type = DirectoryItemType.File }));
                }

            }
            catch { }

            #endregion

            return items;
        }

        #region Helpers
        /// <summary>
        /// Find the file or folder name from a full path
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns>sub path</returns>
        public static string GetFileFoldName(string path)
        {
            // If we have no path, return empty
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            // Make all slashes back slashes
            var normalizedPath = path.Replace('/', '\\');

            // Find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // If we don't find a backlash. return the path itself
            if (lastIndex <= 0)
            {
                return path;
            }

            // Return the name after the last back slash
            return path.Substring(lastIndex + 1);
        }
        #endregion
    }
}

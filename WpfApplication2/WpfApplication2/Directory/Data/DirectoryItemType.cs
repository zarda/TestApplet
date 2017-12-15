using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2
{
    public enum DirectoryItemType
    {
        /// <summary>
        /// A logical drive
        /// </summary>
        Drive,
        /// <summary>
        /// A physcal file
        /// </summary>
        File,
        /// <summary>
        /// A folder
        /// </summary>
        Folder,
        /// <summary>
        /// A Expanded folder
        /// </summary>
        FolderExpanded

    }
}

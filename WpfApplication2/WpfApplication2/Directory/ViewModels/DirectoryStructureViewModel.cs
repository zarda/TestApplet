

using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApplication2
{
    /// <summary>
    /// The view model for the application main Directory view
    /// </summary>
    public class DirectoryStructureViewModel : BaseViewModel
    {
        #region Public property
        /// <summary>
        /// A list of all directories on the machine
        /// </summary>
        public ObservableCollection<DirectoryItemViewModel> Items { set; get; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public DirectoryStructureViewModel()
        {
            // Get the ligical drives
            var children = DirectoryStructure.GetLogicalDrives();

            // Create the view models from the data
            this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive)));
        }

        #endregion
    }
}

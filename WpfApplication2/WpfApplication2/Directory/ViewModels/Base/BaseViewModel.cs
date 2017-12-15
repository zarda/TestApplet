using PropertyChanged;
using System.ComponentModel;

namespace WpfApplication2
{
    /// <summary>
    /// A base view model that fires Property Changed as needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The event that is fired when any child property changs its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}

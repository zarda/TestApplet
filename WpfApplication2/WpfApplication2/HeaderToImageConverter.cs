using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfApplication2
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder or file
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            // By default, we presume an image
            var image = "Image/file.png";

            switch((DirectoryItemType)value)
            {
                case DirectoryItemType.Drive:
                    image = "Image/drive.png";
                    break;
                case DirectoryItemType.Folder:
                    image = "Image/folder-closed.png";
                    break;
                case DirectoryItemType.FolderExpanded:
                    image = "Image/folder-open.png";
                    break;                                   
            }                    

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

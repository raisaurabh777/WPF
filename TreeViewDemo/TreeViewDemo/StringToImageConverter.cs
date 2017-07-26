using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TreeViewDemo
{
    public class StringToImageConverter : IValueConverter
    {
        public static StringToImageConverter Instance = new StringToImageConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Get the full path
            var path = (string)value;

            if (path == null)
                return null;

            var name = MainWindow.GetFileFolderName(path);

            // By default filter Image
            var image = "Images/file.jpg";

            // If name is empty, means it's a drive
            if (name == "")
                image = "Images/drive.jpg";
            // check for folder
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Images/folder.ico";

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

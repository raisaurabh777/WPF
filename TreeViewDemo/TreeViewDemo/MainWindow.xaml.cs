using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace TreeViewDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get all the loical drives
            foreach (var drive in Directory.GetLogicalDrives())
            {
                // Create new Tree View Items
                var item = new TreeViewItem()
                {
                    Header = drive,
                    // To store the full path
                    Tag = drive
                };
                // Add a dumy node for expansion
                item.Items.Add(null);

                // Listen for drive to be expanded
                item.Expanded += Item_Expanded;

                // Add it to the tree view
                explorerTreeView.Items.Add(item);
            }
        }

        private void Item_Expanded(object sender, RoutedEventArgs e)
        {
            #region Init
            var item = sender as TreeViewItem;

            // If we only have default stuff, return
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;
            item.Items.Clear();
            var fullPath = item.Tag.ToString();
            var directories = new List<string>();
            var files = new List<string>();
            #endregion

            #region Get Folders
            try
            {
                directories = Directory.GetDirectories(fullPath).ToList();
            }
            catch (Exception ex)
            {
                // Handle Exception
            }

            directories.ForEach(directoryPath =>
           {
               // create directory item
               var subItem = new TreeViewItem()
               {
                   // Set header as folder name
                   Header = GetFileFolderName(directoryPath),
                   // Copy full path in Tag
                   Tag = directoryPath
               };
               // Adding a dummy item so that we can expand
               subItem.Items.Add(null);

               // Handle expanding
               subItem.Expanded += Item_Expanded;

               // Add item to parent
               item.Items.Add(subItem);
           });

            #endregion

            #region Get Files
            try
            {
                files = Directory.GetFiles(fullPath).ToList();
            }
            catch (Exception ex)
            {
                // Handle Exception
            }

            files.ForEach(filePath =>
            {
                // create directory item
                var subItem = new TreeViewItem()
                {
                    // Set header as folder name
                    Header = GetFileFolderName(filePath),
                    // Copy full path in Tag
                    Tag = filePath
                };
                
                // Add item to parent
                item.Items.Add(subItem);
            });

            #endregion
        }

        public static string GetFileFolderName(string path)
        {
            // C:\folder\a folder
            // C:\folder\a file.png

            // if path is empty, return
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            //make all /'s a \'s
            var normalizedPath = path.Replace('/', '\\');

            // get the index of the last backslash
            var lastIndex = normalizedPath.LastIndexOf('\\');

            // if there is no backslash, return the path
            if (lastIndex <= 0)
                return path;

            return normalizedPath.Substring(lastIndex + 1);
        }
    }
}

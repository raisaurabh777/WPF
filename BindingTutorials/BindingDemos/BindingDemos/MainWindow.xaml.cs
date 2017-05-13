using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace BindingDemos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<User> Users = new ObservableCollection<User>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            // Initialising the User List

            Users.Add(new User { Name = "Saurabh Rai" });
            Users.Add(new User { Name = "John Doe" });
            Users.Add(new User { Name = "Kathy Bert" });

            UsersListBox.ItemsSource = Users;
        }

        private void UpdateScrcBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the Binding Expression of the Destination Control nd update source
            BindingExpression binding = TitleTextBox.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void AddNewUserBtn_Click(object sender, RoutedEventArgs e)
        {
            // Add new user to Users List
            Users.Add(new User { Name = "New User" });
        }

        private void RenameUserBtn_Click(object sender, RoutedEventArgs e)
        {
            // Rename the selected Item
            if (UsersListBox.SelectedItem != null)
                (UsersListBox.SelectedItem as User).Name = "Random Name";
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            // Remove the selected Item
            if (UsersListBox.SelectedItem != null)
                Users.Remove(UsersListBox.SelectedItem as User);
        }
    }

    public class User : INotifyPropertyChanged
    {
        private string name;
        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                    name = value;
                this.NotifyPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class YesNoToBooleanConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString().ToLower())
            {
                case "yes": return true;
                case "no": return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "yes";
                else
                    return "no";
            }
            return "no";
        }
    }
}

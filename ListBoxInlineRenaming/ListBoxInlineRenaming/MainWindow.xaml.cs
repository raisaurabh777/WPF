using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ListBoxInlineRenaming
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<User> Users = new ObservableCollection<User>();
        string newText = null;
        public MainWindow()
        {
            // Populating
            Users.Add(new User { ActualName="Area-Tag", UserDefinedName=String.Empty });
            Users.Add(new User { ActualName="Process Control-Description", UserDefinedName=String.Empty});
            InitializeComponent();
            this.DataContext = this;
            myListBox.ItemsSource = Users;

            myListBox.KeyDown += MyListBox_KeyDown;
            myListBox.MouseDoubleClick += MyListBox_MouseDoubleClick;
        }

        private void MyListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1)
            {
                (myListBox.SelectedItem as User).IsEditable = true;
            }
        }

        private void MyListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                (myListBox.SelectedItem as User).IsEditable = true;
            }
            if (e.Key == Key.Enter)
            {
                if (e.OriginalSource is TextBox)
                {
                    (myListBox.SelectedItem as User).UserDefinedName = newText;
                    (myListBox.SelectedItem as User).IsEditable = false;
                }
            }

            if (e.Key == Key.Escape)
            {
                if (e.OriginalSource is TextBox)
                {
                    (myListBox.SelectedItem as User).IsEditable = false;
                }
            }
        }

        private void myTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                newText = (sender as TextBox).Text;
            }

        }

        private void myTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if ((bool)e.NewValue == true)
            {
                TextBox txtBox = sender as TextBox;
                if (txtBox != null)
                {
                    if (txtBox.Text == String.Empty)
                    {
                        txtBox.ToolTip = "Enter User Defined Name Here";
                        txtBox.Text = "User Defined Name";
                    }
                    txtBox.SelectAll();
                    txtBox.Focus();
                }

            }
               
        }

        private void myTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // To Do: Get the ListBox Here using Visual Tree
        }
    }

    public class User: INotifyPropertyChanged
    {
        private bool _isEditable;
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                if(_isEditable != value)
                {
                    _isEditable = value;
                    NotifyPropertyChanged("IsEditable");
                }
            }

        }

        private string _actualName;
        public String ActualName
        {
            get
            {
                return _actualName;
            }
            set
            {
                if (_actualName != value)
                    _actualName = value;
                NotifyPropertyChanged("ActualName");
                NotifyPropertyChanged("DisplayName");
            }
        }

        private string _userDefinedName;
        public String UserDefinedName
        {
            get
            {
                return _userDefinedName;
            }
            set
            {
                if (_userDefinedName != value)
                    _userDefinedName = value;
                NotifyPropertyChanged("UserDefinedName");
                NotifyPropertyChanged("DisplayName");
            }
        }

        //private string _displayName;

        public String DisplayName
        {
            get
            {
                if (UserDefinedName.Trim() == String.Empty)
                    return ActualName;
                else
                    return UserDefinedName + " " + "[" + ActualName + "]";
            }
            //set
            //{
            //    if (_displayName != value)
            //        _displayName = value;
            //    NotifyPropertyChanged("DisplayName");
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}

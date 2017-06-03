using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using RelayCommandTutorial.Model;
using System.Windows.Input;

namespace RelayCommandTutorial.ViewModel
{
    public class PersonViewModel
    {
        public RelayCommand UpdateCommand { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _addr;
        public string Address
        {
            get { return _addr; }
            set { _addr = value; }
        }

        private ObservableCollection<Person> _personList = new ObservableCollection<Person>();
       
        public ObservableCollection<Person> PersonList
        {
            get
            {
                return _personList;
            }
            set
            {
                if (_personList != null)
                    _personList = value;
            }
        }
            
        public PersonViewModel()
        {
            PersonList.Add(new Person { Name = "Saurabh", Address = "Mountain View" });
            PersonList.Add(new Person { Name = "Mary Jane", Address = "Singapore" });

            // Adding Commands
            
            UpdateCommand = new RelayCommand(x => { ListUpdater(x); }, x => true);
        }

        public void ListUpdater(Object x)
        {
            // Update List
            PersonList.Add(new Person { Name = this.Name, Address = this.Address });
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}

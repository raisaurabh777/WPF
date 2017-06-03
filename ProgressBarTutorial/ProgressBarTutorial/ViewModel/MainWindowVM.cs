using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgressBarTutorial.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand StartWork { get; set; }
        public BackgroundWorker worker;

        private int _currentProgress;
        public int CurrentProgress
        {
            get
            {
                return _currentProgress;
            }
            set
            {
                if (_currentProgress != value)
                {
                    _currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }
        public MainWindowVM()
        {
            StartWork = new RelayCommand(param => this.worker.RunWorkerAsync(), param => true);

            // Making the Background Worker ready
            this.worker = new BackgroundWorker();
            this.worker.DoWork += Worker_DoWork;
            this.worker.ProgressChanged += Worker_ProgressChanged;
            this.worker.WorkerReportsProgress = true;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentProgress = e.ProgressPercentage;
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                for (int i = 0; i <= 10; i++)
                {
                    Thread.Sleep(1000);
                    this.worker.ReportProgress(i * (100 / 10));
                }
            }
            catch (Exception Ex)
            {

                Console.WriteLine(Ex.Message.ToString());
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            if(this.PropertyChanged != null)
                {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
}

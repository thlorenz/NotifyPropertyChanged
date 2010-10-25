using System;
using System.Windows.Input;
using WPFCoreTools;

namespace NotifyPropertyChanged.LinFu
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            InitializeCommand();
        }

        public void InitializeCommand()
        {
            Console.WriteLine("Initializing AddCommand, was null = {0}", AddCommand == null);
            AddCommand = new SimpleCommand { ExecuteDelegate = Add };
        }

        public ICommand AddCommand { get; set; }

        public virtual int Counter { get; set; }

        public int SquareOfCounter
        {
            get { return Counter * Counter; }
        }

        public double RootOfCounter
        {
            get { return Math.Sqrt(Counter); }
        }

        void Add(object obj)
        {
            Counter++;
            Console.WriteLine("Added to counter -> count is now: {0}", Counter);
        }
    }
}
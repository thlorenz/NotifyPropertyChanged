using System;
using System.Windows.Input;
using WPFCoreTools;

namespace NotifyPropertyChanged.PostSharp
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            AddCommand = new SimpleCommand { ExecuteDelegate = Add };
        }

        public ICommand AddCommand { get; set; }

        [NotifyPropertyChanged("SquareOfCounter", "RootOfCounter")]
        public int Counter { get; set; }

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
        }
    }
}
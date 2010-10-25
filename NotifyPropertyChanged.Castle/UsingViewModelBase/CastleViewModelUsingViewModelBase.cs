using System;
using System.Windows.Input;
using WPFCoreTools;

namespace NotifyPropertyChanged.Castle.UsingViewModelBase
{
    public class CastleViewModelUsingViewModelBase : ViewModelBase
    {
        public CastleViewModelUsingViewModelBase()
        {
            AddCommand = new SimpleCommand { ExecuteDelegate = Add };
        }

        public ICommand AddCommand { get; set; }

        [NotifyPropertyChanged("RootOfCounter", "SquareOfCounter")]
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
        }
    }
}
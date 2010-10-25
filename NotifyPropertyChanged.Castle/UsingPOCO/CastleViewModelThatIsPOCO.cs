using System;
using System.Windows.Input;
using WPFCoreTools;

namespace NotifyPropertyChanged.Castle.UsingPOCO
{
    public class CastleViewModelThatIsPOCO
    {
        public CastleViewModelThatIsPOCO()
        {
            AddCommand = new SimpleCommand { ExecuteDelegate = Add };
        }

        public ICommand AddCommand { get; set; }

        [NotifyPropertyChanged("RootOfCounter", "SquareOfCounter")]
        public virtual int Counter { get; set; }

        public virtual double RootOfCounter
        {
            get { return Math.Sqrt(Counter); }
        }
        public virtual int SquareOfCounter
        {
            get { return Counter * Counter; }
        }

        void Add(object obj)
        {
            Counter++;
        }
    }
}
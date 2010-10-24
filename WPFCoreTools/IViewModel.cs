using System.ComponentModel;

namespace WPFCoreTools
{
    public interface IViewModel : INotifyPropertyChanged
    {
        void RaisePropertyChanged(string propertyName);
    }
}
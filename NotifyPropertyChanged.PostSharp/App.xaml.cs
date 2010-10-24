using System.Windows;
using NotifyPropertyChanged;
using NotifyPropertyChanged.PostSharp;

namespace WPFTestingNotifyPropertyChanged
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var viewModel = new MainViewModel();
            var view = new MainView(viewModel);
            new Window { Content = view, Topmost = true, Width = 300, Height = 200 }.Show();
        }
    }
}
using System.Windows.Controls;
using WPFTestingNotifyPropertyChanged;

namespace NotifyPropertyChanged.PostSharp
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        public MainView(MainViewModel mainViewModel)
            : this()
        {
            DataContext = mainViewModel;
        }
    }
}
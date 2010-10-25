using System.Windows.Controls;

namespace NotifyPropertyChanged.LinFu
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

        public MainView(CastleViewModelUsingViewModelBase castleViewModelUsingViewModelBase)
            : this()
        {
            DataContext = castleViewModelUsingViewModelBase;
        }
    }
}
using System.Windows.Controls;

namespace NotifyPropertyChanged.Castle
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

        public MainView(object viewModel)
            : this()
        {
            DataContext = viewModel;
        }
    }
}
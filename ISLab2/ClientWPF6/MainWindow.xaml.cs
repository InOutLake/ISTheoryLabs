using System.Windows;
using ClientWPF6.Views;

namespace ClientWPF6
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new PlayersListView(new EventAggregator());
            InitializeComponent();
        }
    }
}

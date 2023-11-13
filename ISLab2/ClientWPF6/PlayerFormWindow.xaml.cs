using System.Windows;
using Server;
using ClientWPF6.Views;
using System.Drawing.Imaging;
using Microsoft.Identity.Client;

namespace ClientWPF6
{
    /// <summary>
    /// Логика взаимодействия для PlayerFormWindow.xaml
    /// </summary>
    public partial class PlayerFormWindow : Window
    {
        public PlayerFormWindow(EventAggregator eventAggregator)
        {
            PlayerFormView playerFormView = new PlayerFormView(eventAggregator);
            DataContext = playerFormView;
            InitializeComponent();
        }
    }
}

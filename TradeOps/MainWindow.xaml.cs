using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TradeOps.View.WindowView;

namespace TradeOps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(5000);

            // Open the Login Window
            LoginWindow login = new LoginWindow();
            login.Show();

            Application.Current.MainWindow = login;
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Close the Splash Screen
            this.Close();
        }
    }
}
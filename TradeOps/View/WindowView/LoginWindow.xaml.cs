using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TradeOps.View.WindowView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the password entered by the user
            string enteredPassword = PasswordInput.Password;

            // Replace with your actual check (e.g., compare with database, config, etc.)
            string correctPassword = "goodbyetradeops#144";



            if (enteredPassword == correctPassword)
            {
                Window Dashboard = new View.WindowView.DashboardWindowView();
                Dashboard.Show();

                // Set ShutdownMode to close app when dashboard is closed
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Application.Current.MainWindow = Dashboard;

                // Close Login
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials, please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }
    }
}

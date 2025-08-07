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
            //string enteredPassword = PasswordInput.Password;
            string enteredPassword = isPasswordVisible
    ? PasswordBoxVisible.Text
    : PasswordBoxHidden.Password;

            // Replace with your actual check (e.g., compare with database, config, etc.)
            string correctPassword = "tradeops#144";



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

        private bool isPasswordVisible = false;

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                PasswordBoxVisible.Text = PasswordBoxHidden.Password;
                PasswordBoxVisible.Visibility = Visibility.Visible;
                PasswordBoxHidden.Visibility = Visibility.Collapsed;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOff;
            }
            else
            {
                PasswordBoxHidden.Password = PasswordBoxVisible.Text;
                PasswordBoxHidden.Visibility = Visibility.Visible;
                PasswordBoxVisible.Visibility = Visibility.Collapsed;
                EyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Eye;
            }
        }

        private void PasswordBoxHidden_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isPasswordVisible)
                PasswordBoxVisible.Text = PasswordBoxHidden.Password;
        }

        private void PasswordBoxVisible_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isPasswordVisible)
                PasswordBoxHidden.Password = PasswordBoxVisible.Text;
        }


    }
}

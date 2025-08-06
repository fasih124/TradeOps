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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TradeOps.Helper;

namespace TradeOps.View.UserControlView
{
    /// <summary>
    /// Interaction logic for SettingUserControlView.xaml
    /// </summary>
    public partial class SettingUserControlView : UserControl
    {
        public SettingUserControlView()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Importing data...", "Import", MessageBoxButton.OK, MessageBoxImage.Information);
            DataManagement.loadData();
            MessageBox.Show("Successfully loaded data from the Location", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Exporting data...", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
            DataManagement.BackupData();
            MessageBox.Show("Successfully Exporting data to backup location.", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to delete all data? This action cannot be undone.",
                "Reset Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                DataManagement.ResetDatabase();
            }
        }
    }
}

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
using TradeOps.ViewModel;

namespace TradeOps.View.WindowView
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            var vm = new addProduct_viewModel();
            vm.CloseAction = this.Close;
            this.DataContext = vm;
        }

        private void TextBox_SelectAll_IfZero(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Text == "0")
            {
                tb.Clear(); // Remove zero
            }
        }

        private void TextBox_RestoreZeroIfEmpty(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = "0"; // Restore zero if nothing entered
            }
        }

    }
}

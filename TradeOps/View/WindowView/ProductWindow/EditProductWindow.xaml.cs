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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        public EditProductWindow()
        {
            InitializeComponent();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TradeOps.View.WindowView.CustomerWindow
{
    /// <summary>
    /// Interaction logic for EditCustomerWindow.xaml
    /// </summary>
    public partial class EditCustomerWindow : Window
    {
        public EditCustomerWindow()
        {
            InitializeComponent();
        }

        private static readonly Regex _numericRegex = new Regex(@"^[0-9]*(\.[0-9]*)?$"); // Allow decimal
                                                                                         // For integer-only: use @"^[0-9]+$"

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var fullText = ((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text);
            e.Handled = !_numericRegex.IsMatch(fullText);
        }

    }
}

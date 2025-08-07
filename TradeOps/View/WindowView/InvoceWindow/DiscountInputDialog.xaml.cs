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

namespace TradeOps.View.WindowView.InvoceWindow
{
    /// <summary>
    /// Interaction logic for DiscountInputDialog.xaml
    /// </summary>
    public partial class DiscountInputDialog : Window
    {
        private double total;
        private double previousDiscount;
        public double? DiscountValue { get; private set; }
        public DiscountInputDialog(double total, double previousDiscount)
        {
            InitializeComponent();
            this.total = total;
            this.previousDiscount = previousDiscount;

            DiscountTextBox.Text = previousDiscount.ToString();
            DiscountTextBox.Focus();
            DiscountTextBox.CaretIndex = DiscountTextBox.Text.Length;

        }

        private static readonly Regex _numericRegex = new Regex(@"^[0-9]*(\.[0-9]*)?$"); // Allow decimal
                                                                                         // For integer-only: use @"^[0-9]+$"

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var fullText = ((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text);
            e.Handled = !_numericRegex.IsMatch(fullText);
        }


        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(DiscountTextBox.Text, out double discount) && discount<= this.total)
            {
                DiscountValue = discount;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter a valid number. Less that or Equal to Total Price ", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}

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
using TradeOps.Model;
using TradeOps.ViewModel.OrderRelatedViewModel;

namespace TradeOps.View.WindowView.OrderWindow
{
    /// <summary>
    /// Interaction logic for ProductPickerOrderWindow.xaml
    /// </summary>
    public partial class ProductPickerOrderWindow : Window
    {
        public ProductPicker_ViewModel ViewModel { get; set; }
        public ProductPickerOrderWindow()
        {
            InitializeComponent();
            ViewModel = new ProductPicker_ViewModel();
            DataContext = ViewModel;
        }

        public Product SelectedProduct => ViewModel.SelectedProduct;

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProduct != null)
            {
                DialogResult = true;
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

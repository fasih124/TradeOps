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
    /// Interaction logic for EditProductPickerDialog.xaml
    /// </summary>
    public partial class EditProductPickerDialog : Window
    {
        public EditProductDialogViewModel ViewModel => (EditProductDialogViewModel)DataContext;

        public Product SelectedProduct => ViewModel.SelectedProduct;
        public EditProductPickerDialog()
        {
            InitializeComponent();
            DataContext = new EditProductDialogViewModel();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.SelectedProduct != null)
            {
                DialogResult = true;
                Close();
            }
        }
    }
}

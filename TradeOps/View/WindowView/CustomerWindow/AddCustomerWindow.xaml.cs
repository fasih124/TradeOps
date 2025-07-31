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
using TradeOps.ViewModel.CustomerRelatedViewModel;

namespace TradeOps.View.WindowView.CustomerWindow
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow()
        {
            InitializeComponent();
            var vm = new AddCustomer_ViewModel();
            vm.CloseAction = this.Close;
            this.DataContext = vm;
        }
    }
}

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

namespace TradeOps.View.UserControlView
{
    /// <summary>
    /// Interaction logic for OrderUserControlView.xaml
    /// </summary>
    public partial class OrderUserControlView : UserControl
    {
        public OrderUserControlView()
        {
            InitializeComponent();
            DataContext = new ViewModel.OrderRelatedViewModel.OrderViewModel();
        }
    }
}

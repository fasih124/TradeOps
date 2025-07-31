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

namespace TradeOps.View.WindowView.OrderWindow
{
    /// <summary>
    /// Interaction logic for DetailOrderWindow.xaml
    /// </summary>
    public partial class DetailOrderWindow : Window
    {
        public DetailOrderWindow(CustomerOrder order)
        {
            InitializeComponent();
            this.DataContext = order;
        }
    }
}

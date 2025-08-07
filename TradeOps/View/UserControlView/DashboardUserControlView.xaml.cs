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
using TradeOps.ViewModel.DashboradViewModel;

namespace TradeOps.View.UserControlView
{
    /// <summary>
    /// Interaction logic for DashboardUserControlView.xaml
    /// </summary>
    public partial class DashboardUserControlView : UserControl
    {
        public DashboardUserControlView()
        {
            InitializeComponent();

            DataContext = new DashboardViewModel();
        }
    }
}

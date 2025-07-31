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
using TradeOps.View.UserControlView;

namespace TradeOps.View.WindowView
{
    /// <summary>
    /// Interaction logic for DashboardWindowView.xaml
    /// </summary>
    public partial class DashboardWindowView : Window
    {
        public DashboardWindowView()
        {
            InitializeComponent();
        }
        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string viewName = button?.Tag?.ToString();

            System.Windows.Controls.UserControl view = viewName switch
            {
                //"OrderView" => new OrderUserControl(),
                "ProductView" => new ProductUserControlView(),
                "CustomerView" => new CustomerUserControlView(),
                _ => null
            };

            if (view != null)
                MainContent.Content = view;
        }
    }
}

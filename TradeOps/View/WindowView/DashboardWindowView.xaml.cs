using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TradeOps.Helper;
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
            MainContent.Content = new DashboardUserControlView();
            NavigationListBox.ItemsSource = GetSampleItems();
        }

        private ObservableCollection<SampleItem> GetSampleItems()
        {
            return new ObservableCollection<SampleItem>
            {
                new SampleItem
                {
                    Title = "Dashboard",
                    Notification = 0,
                    SelectedIcon = PackIconKind.ViewDashboard,
                    UnselectedIcon = PackIconKind.ViewDashboardOutline
                },
                new SampleItem
                {
                    Title = "Orders",
                    Notification = 0,
                    SelectedIcon = PackIconKind.Cart,
                    UnselectedIcon = PackIconKind.CartOutline
                },
                new SampleItem
                {
                    Title = "Product",
                    Notification = 0,
                    SelectedIcon = PackIconKind.PackageVariant,
                    UnselectedIcon = PackIconKind.PackageVariantClosed
                },
                new SampleItem
                {
                    Title = "Customer",
                    Notification = 0,
                    SelectedIcon = PackIconKind.AccountMultiple,
                    UnselectedIcon =  PackIconKind.AccountMultipleOutline
                },
                new SampleItem
                {
                    Title = "Invoice",
                    Notification = 0,
                    SelectedIcon = PackIconKind.InvoiceMultiple,
                    UnselectedIcon = PackIconKind.InvoiceMultipleOutline
                },
                new SampleItem
                {
                    Title = "Setting",
                    Notification = 0,
                    SelectedIcon = PackIconKind.Cog,
                    UnselectedIcon = PackIconKind.CogOutline
                }
            };
        }

        private void NavigationListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationListBox.SelectedItem is SampleItem selectedItem)
            {
                switch (selectedItem.Title)
                {
                    case "Dashboard":
                        MainContent.Content = new DashboardUserControlView(); 
                        break;
                    case "Orders":
                        MainContent.Content = new OrderUserControlView();
                        break;
                    case "Product":
                        MainContent.Content = new ProductUserControlView();
                        break;
                    case "Customer":
                        MainContent.Content = new CustomerUserControlView();
                        break;
                    case "Invoice":
                        MainContent.Content = new InvoiceUserControlView();
                        break;
                    case "Setting":
                        MainContent.Content = new SettingUserControlView();
                        break;
                    default:
                        MainContent.Content = new DashboardUserControlView();
                        break;
                }
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            Application.Current.MainWindow = login; // Set new MainWindow
            login.Show();
            this.Close();

        }
    }


}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using TradeOps.Helper;
using TradeOps.Model;
using TradeOps.View.WindowView.OrderWindow;

namespace TradeOps.ViewModel.OrderRelatedViewModel
{
    internal class OrderViewModel : BaseViewModel
    {

        public ICommand OpenAddOrderWindowCommand { get; set; }

        private ObservableCollection<CustomerOrder> _orders;
        public ObservableCollection<CustomerOrder> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }


        public OrderViewModel()
        {
            LoadOrders();
            OpenAddOrderWindowCommand = new RelayCommand(OpenAddOrderWindow);
        }

        private void OpenAddOrderWindow(object obj)
        {
            var window = new AddOrderWindow(); // Make sure it has its own DataContext inside
            window.ShowDialog();
        }


        private void LoadOrders()
        {
            var allOrders = DB_Queries.GetAllOrders();
            Orders = new ObservableCollection<CustomerOrder>(allOrders);
        }

        private CustomerOrder _selectedOrder;
        public CustomerOrder SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public RelayCommand ViewDetailsCommand => new RelayCommand(ViewDetails, (obj) => SelectedOrder != null);

        private void ViewDetails(Object obj)
        {
            var detailWindow = new DetailOrderWindow(SelectedOrder);
            detailWindow.ShowDialog();
        }


    }
}

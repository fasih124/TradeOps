using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public ICommand DeleteOrderCommand { get; set; }

        public ICommand EditOrderCommand { get; }


        private List<CustomerOrder> _allOrders;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterOrders(); // Automatically filter on change
                }
            }
        }

        private string _searchType;
        public string SearchType
        {
            get => _searchType;
            set
            {
                if (SetProperty(ref _searchType, value))
                {
                    FilterOrders(); // Re-filter if search type changes
                }
            }
        }

        private ObservableCollection<CustomerOrder> _orders;
        public ObservableCollection<CustomerOrder> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        private CustomerOrder _selectedOrder;
        public CustomerOrder SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }


        public OrderViewModel()
        {
            
            SearchType = "ID";
            LoadOrders();
            FilterOrders();
            OpenAddOrderWindowCommand = new RelayCommand(OpenAddOrderWindow);
            EditOrderCommand = new RelayCommand(OpenEditOrderWindow, _ => SelectedOrder != null);
            DeleteOrderCommand = new RelayCommand(DeleteOrder, (obj) => SelectedOrder != null);
        }

        private void OpenEditOrderWindow(object obj)
        {
            var vm = new EditOrder_ViewModel(SelectedOrder);
            var window = new EditOrderWindow { DataContext = vm };
            window.ShowDialog();
            LoadOrders(); // Refresh after update
        }

        private void OpenAddOrderWindow(object obj)
        {
            var window = new AddOrderWindow(); // Make sure it has its own DataContext inside
            window.ShowDialog();
            LoadOrders(); // Reload orders after adding a new one
        }


        private void LoadOrders()
        {
            _allOrders = DB_Queries.GetAllOrders().ToList();
            Orders = new ObservableCollection<CustomerOrder>(_allOrders);
        }

        private void FilterOrders()
        {
            if (_allOrders == null)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Orders = new ObservableCollection<CustomerOrder>(_allOrders);
                return;
            }
            string filter = SearchText.Trim().ToLower();

            IEnumerable<CustomerOrder> filtered;

            if (SearchType == "ID")
            {
                filtered = _allOrders
                    .Where(p => p.ID.ToString().Contains(filter));
            }
            else if (SearchType == "Name")
            {
                filtered = _allOrders
                    .Where(p => p.Customer.Name != null && p.Customer.Name.ToLower().Contains(filter));
            }else if (SearchType == "Date")
            {
                filtered = _allOrders
                    .Where(p => p.Date != null && p.Date.ToLower().Contains(filter));
            }
            else
            {
                filtered = _allOrders;
            }
            System.Diagnostics.Debug.WriteLine($"Filtering: {SearchType} | '{SearchText}'");
            Orders = new ObservableCollection<CustomerOrder>(filtered);
        }

        public RelayCommand ViewDetailsCommand => new RelayCommand(ViewDetails, (obj) => SelectedOrder != null);

        private void ViewDetails(Object obj)
        {
            var detailWindow = new DetailOrderWindow(SelectedOrder);
            detailWindow.ShowDialog();
            LoadOrders(); // Reload orders after viewing details
        }


        // delete order command
        private void DeleteOrder(object obj)
        {
            if (SelectedOrder == null)
            {
                MessageBox.Show("Please select an order to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete Order #{SelectedOrder.ID}?",
                "Confirm Deletion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                DB_Queries.DeleteOrder(SelectedOrder.ID);
                LoadOrders(); // Refresh the list after deletion
            }
        }



    }
}

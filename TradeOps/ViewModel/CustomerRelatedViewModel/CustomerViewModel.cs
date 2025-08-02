using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeOps.Helper;
using TradeOps.Model;
using TradeOps.View.WindowView;
using TradeOps.View.WindowView.CustomerWindow;

namespace TradeOps.ViewModel.CustomerRelatedViewModel
{
    internal class CustomerViewModel: BaseViewModel
    {
        private List<Customer> _allCustomers;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    FilterCustomers();
            }
        }

        private string _searchType;
        public string SearchType
        {
            get => _searchType;
            set
            {
                if (SetProperty(ref _searchType, value))
                    FilterCustomers();
            }
        }



        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set => SetProperty(ref _customers, value);
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        // check
        public CustomerViewModel()
        {
            SearchType = "ID";
            LoadCustomers();
            FilterCustomers();

            System.Diagnostics.Debug.WriteLine($"this excute.");       
            OpenAddWindowCommand = new RelayCommand(OpenAddCustomerWindow);
            EditCommand = new RelayCommand(EditSelectedCustomer);
            DeleteCommand = new RelayCommand(DeleteSelectedCustomer);
        }



        public void LoadCustomers()
        {
            _allCustomers = DB_Queries.GetAllCustomers().ToList();
            Customers = new ObservableCollection<Customer>(_allCustomers);
            System.Diagnostics.Debug.WriteLine($"Loaded {Customers.Count} customer.");
        }


        private void FilterCustomers()
        {
            if (_allCustomers == null)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Customers = new ObservableCollection<Customer>(_allCustomers);
                return;
            }

            string filter = SearchText.Trim().ToLower();
            IEnumerable<Customer> filtered;

            switch (SearchType)
            {
                case "ID":
                    filtered = _allCustomers.Where(c => c.ID.ToString().Contains(filter));
                    break;
                case "Name":
                    filtered = _allCustomers.Where(c => c.Name != null && c.Name.ToLower().Contains(filter));
                    break;
                case "Area":
                    filtered = _allCustomers.Where(c => c.Area != null && c.Area.ToLower().Contains(filter));
                    break;
                case "PhoneNumber":
                    filtered = _allCustomers.Where(c => c.PhoneNumber != null && c.PhoneNumber.ToLower().Contains(filter));
                    break;
                default:
                    filtered = _allCustomers;
                    break;
            }

            Customers = new ObservableCollection<Customer>(filtered);
        }

        // add product window
        public ICommand OpenAddWindowCommand { get; }



        private void OpenAddCustomerWindow(object obj)
        {
            var window = new AddCustomerWindow();
            window.ShowDialog();

            LoadCustomers(); // reload to refresh list
        }



        // edit product window
        public ICommand EditCommand { get; }



        private void EditSelectedCustomer(object obj)
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a Customer to Edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new EditCustomerWindow();
            editWindow.DataContext = new EditCustomer_ViewModel(SelectedCustomer, editWindow);
            editWindow.ShowDialog();

            LoadCustomers(); // reload to refresh list
        }


        // delete product
        public ICommand DeleteCommand { get; }
        private void DeleteSelectedCustomer(object obj)
        {
            if (SelectedCustomer == null)
            {
                MessageBox.Show("Please select a Customer to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete \"{SelectedCustomer.Name}\"?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DB_Queries.DeleteCustomer(SelectedCustomer);
                LoadCustomers(); // Refresh the product list
            }
        }
    }
}

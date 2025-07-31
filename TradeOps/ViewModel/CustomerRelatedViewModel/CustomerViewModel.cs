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
            System.Diagnostics.Debug.WriteLine($"this excute.");
            LoadProducts();
            OpenAddWindowCommand = new RelayCommand(OpenAddCustomerWindow);
            EditCommand = new RelayCommand(EditSelectedCustomer);
            DeleteCommand = new RelayCommand(DeleteSelectedCustomer);
        }



        public void LoadProducts()
        {
            Customers = DB_Queries.GetAllCustomers();
            System.Diagnostics.Debug.WriteLine($"Loaded {Customers.Count} products.");
        }

        // add product window
        public ICommand OpenAddWindowCommand { get; }



        private void OpenAddCustomerWindow(object obj)
        {
            var window = new AddCustomerWindow();
            window.ShowDialog();

            // Optionally refresh product list after closing
            Customers = DB_Queries.GetAllCustomers();
            OnPropertyChanged(nameof(Customers));
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

            LoadProducts(); // reload to refresh list
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
                LoadProducts(); // Refresh the product list
            }
        }
    }
}

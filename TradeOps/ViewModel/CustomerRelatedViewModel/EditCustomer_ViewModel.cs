using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel.CustomerRelatedViewModel
{
    internal class EditCustomer_ViewModel
    {
        public Customer Customer { get; set; }
        public ICommand SaveCommand { get; }

        private Window _window;

        public EditCustomer_ViewModel(Customer customer, Window window)
        {
            Customer = new Customer
            {
                ID = customer.ID,
                Name = customer.Name,
                Area = customer.Area,
                Address= customer.Address,
                PhoneNumber= customer.PhoneNumber,
               
            };

            _window = window;
            SaveCommand = new RelayCommand(SaveCustomer);
        }

        private void SaveCustomer(object obj)
        {
            DB_Queries.UpdateCustomer(Customer);
            _window.Close();
        }
    }
}

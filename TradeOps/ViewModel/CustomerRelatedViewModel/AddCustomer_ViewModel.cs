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
    internal class AddCustomer_ViewModel
    {
        public Customer Customer { get; set; } = new Customer();

        public ICommand SaveCommand { get; }

        public Action CloseAction { get; set; } // to close window from ViewModel


        public AddCustomer_ViewModel()
        {
            int nextId = DB_Queries.GetNextCustomerId();
            Customer = new Customer { ID = nextId };
            SaveCommand = new RelayCommand(SaveCustomer);
        }


        private void SaveCustomer(object obj)
        {
            if (DB_Queries.InsertCustomer(Customer))
            {
                MessageBox.Show("Customer added successfully!");
                CloseAction?.Invoke();
            }
            else
            {
                MessageBox.Show("Error while adding Customer.");
            }
        }


    }
}

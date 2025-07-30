using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel
{
    class addProduct_viewModel : BaseViewModel
    {
        public Product Product { get; set; } = new Product();

        public ICommand SaveCommand { get; }

        public Action CloseAction { get; set; } // to close window from ViewModel


        public addProduct_viewModel()
        {
            int nextId = DB_Queries.GetNextProductId();
            Product = new Product { ID = nextId.ToString() };
            SaveCommand = new RelayCommand(SaveProduct);
        }
        

        private void SaveProduct(object obj)
        {
            if (DB_Queries.InsertProduct(Product))
            {
                MessageBox.Show("Product added successfully!");
                CloseAction?.Invoke();
            }
            else
            {
                MessageBox.Show("Error while adding product.");
            }
        }




    }
}

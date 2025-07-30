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
    class EditProduct_ViewModel
    {
        public Product Product { get; set; }
        public ICommand SaveCommand { get; }

        private Window _window;

        public EditProduct_ViewModel(Product product, Window window)
        {
            Product = new Product
            {
                ID = product.ID,
                Name = product.Name,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                ThresholdLevel = product.ThresholdLevel,
                StockQuantity = product.StockQuantity,
                IsTracked = product.IsTracked
            };

            _window = window;
            SaveCommand = new RelayCommand(SaveProduct);
        }

        private void SaveProduct(object obj)
        {
            DB_Queries.UpdateProduct(Product);
            _window.Close();
        }
    }
}


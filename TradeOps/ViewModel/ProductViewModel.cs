using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel
{
    class ProductViewModel : BaseViewModel
    {

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public ProductViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"this excute.");
            LoadProducts();
        }

        public void LoadProducts()
        {
            Products = DB_Queries.GetAllProducts();
            System.Diagnostics.Debug.WriteLine($"Loaded {Products.Count} products.");
        }

    }
}

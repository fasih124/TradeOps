using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel.OrderRelatedViewModel
{
    public class EditProductDialogViewModel : BaseViewModel
    {
        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterProducts();
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public EditProductDialogViewModel()
        {
            AllProducts = DB_Queries.GetAllProducts();
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
        }

        private void FilterProducts()
        {
            FilteredProducts.Clear();
            foreach (var product in AllProducts.Where(p => p.Name.ToLower().Contains(SearchText?.ToLower() ?? "")))
            {
                FilteredProducts.Add(product);
            }
        }
    }
}

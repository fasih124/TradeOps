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
    public class ProductPicker_ViewModel : BaseViewModel
    {
        private string _searchText;
        private Product _selectedProduct;

        public ObservableCollection<Product> AllProducts { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterProducts();
            }
        }

        public ProductPicker_ViewModel()
        {
            AllProducts = new ObservableCollection<Product>(DB_Queries.GetAllProducts());
            FilteredProducts = new ObservableCollection<Product>(AllProducts);
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = new ObservableCollection<Product>(AllProducts);
            }
            else
            {
                var lower = SearchText.ToLower();
                var filtered = AllProducts.Where(p =>
                    (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(lower)) ||
                    p.ID.ToString().Contains(lower) ||
                    p.SellingPrice.ToString().Contains(lower)
                ).ToList();

                //FilteredProducts = new ObservableCollection<Product>(filtered);
                FilteredProducts.Clear();
                foreach (var product in filtered)
                {
                    FilteredProducts.Add(product);
                }
            }

            OnPropertyChanged(nameof(FilteredProducts));
        }
    }
}

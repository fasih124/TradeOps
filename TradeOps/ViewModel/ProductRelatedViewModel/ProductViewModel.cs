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

namespace TradeOps.ViewModel
{
    class ProductViewModel : BaseViewModel
    {

        private List<Product> _allProducts;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterProducts(); // Automatically filter on change
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
                    FilterProducts(); // Re-filter if search type changes
                }
            }
        }


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
            SearchType = "ID";
            LoadProducts();
            FilterProducts();
            OpenAddWindowCommand = new RelayCommand(OpenAddProductWindow);
            EditCommand = new RelayCommand(EditSelectedProduct);
            DeleteCommand = new RelayCommand(DeleteSelectedProduct);
        }


        public void LoadProducts()
        {
            _allProducts = DB_Queries.GetAllProducts().ToList();
            Products = new ObservableCollection<Product>(_allProducts);
            System.Diagnostics.Debug.WriteLine($"Loaded {Products.Count} products.");
        }
        private void FilterProducts()
        {
            if (_allProducts == null)
                return;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Products = new ObservableCollection<Product>(_allProducts);
                return;
            }

            string filter = SearchText.Trim().ToLower();

            IEnumerable<Product> filtered;

            if (SearchType == "ID")
            {
                filtered = _allProducts
                    .Where(p => p.ID.ToString().Contains(filter));
            }
            else if (SearchType == "Name")
            {
                filtered = _allProducts
                    .Where(p => p.Name != null && p.Name.ToLower().Contains(filter));
            }
            else
            {
                filtered = _allProducts;
            }
            System.Diagnostics.Debug.WriteLine($"Filtering: {SearchType} | '{SearchText}'");
            Products = new ObservableCollection<Product>(filtered);
        }


        // add product window
        public ICommand OpenAddWindowCommand { get; }

        private void OpenAddProductWindow(object obj)
        {
            var window = new AddProductWindow();
            window.ShowDialog();


            LoadProducts();
        }

        // edit product window
        public ICommand EditCommand { get; }
        private void EditSelectedProduct(object obj)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to Edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new EditProductWindow();
            editWindow.DataContext = new EditProduct_ViewModel(SelectedProduct, editWindow);
            editWindow.ShowDialog();

            LoadProducts(); // reload to refresh list
        }

        // delete product
        public ICommand DeleteCommand { get; }
        private void DeleteSelectedProduct(object obj)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete \"{SelectedProduct.Name}\"?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                DB_Queries.DeleteProduct(SelectedProduct);
                LoadProducts(); // Refresh the product list
            }
        }


    }
}

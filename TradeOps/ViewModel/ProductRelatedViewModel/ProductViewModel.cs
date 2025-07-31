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
            OpenAddWindowCommand = new RelayCommand(OpenAddProductWindow);
            EditCommand = new RelayCommand(EditSelectedProduct);
            DeleteCommand = new RelayCommand(DeleteSelectedProduct);
        }

    

        public void LoadProducts()
        {
            Products = DB_Queries.GetAllProducts();
            System.Diagnostics.Debug.WriteLine($"Loaded {Products.Count} products.");
        }

        // add product window
        public ICommand OpenAddWindowCommand { get; }

      

        private void OpenAddProductWindow(object obj)
        {
            var window = new AddProductWindow();
            window.ShowDialog();

            // Optionally refresh product list after closing
            Products = DB_Queries.GetAllProducts();
            OnPropertyChanged(nameof(Products));
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

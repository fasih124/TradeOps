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

namespace TradeOps.ViewModel.OrderRelatedViewModel
{
    internal class EditOrder_ViewModel : BaseViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<OrderDetail> CurrentOrderDetails { get; set; } = new();

        public Customer SelectedCustomer { get; set; }
        public Product SelectedProduct { get; set; }
        public int EnteredQuantity { get; set; }

        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double TotalProfit { get; set; }

        public ICommand AddProductCommand { get; set; }
        public ICommand SaveOrderCommand { get; set; }
        public ICommand RemoveProductCommand { get; }

        private readonly CustomerOrder _originalOrder;

        public EditOrder_ViewModel(CustomerOrder order)
        {
            _originalOrder = order;

            LoadCustomers();
            LoadProducts();

            SelectedCustomer = order.Customer;

           
            foreach (var detail in order.ProductDetails)
            {
                var newDetail = new OrderDetail(detail.Quantity, detail.Product);
                newDetail.PropertyChanged += OrderDetail_PropertyChanged;
                CurrentOrderDetails.Add(newDetail);
            }

            UpdateTotals();

            AddProductCommand = new RelayCommand(AddProduct);
            SaveOrderCommand = new RelayCommand(SaveOrder);
           RemoveProductCommand = new RelayCommand(RemoveProduct);
        }
        private void RemoveProduct(object obj)
        {
            if (obj is OrderDetail detail && CurrentOrderDetails.Contains(detail))
            {
                CurrentOrderDetails.Remove(detail);
                detail.PropertyChanged -= OrderDetail_PropertyChanged;
                UpdateTotals();
            }
        }
        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var customer in DB_Queries.GetAllCustomers())
                Customers.Add(customer);
        }

        private void LoadProducts()
        {
            Products.Clear();
            foreach (var product in DB_Queries.GetAllProducts())
                Products.Add(product);
        }

        private void AddProduct(object obj)
        {
            if (SelectedProduct != null && EnteredQuantity > 0)
            {
                var existing = CurrentOrderDetails.FirstOrDefault(p => p.Product.ID == SelectedProduct.ID);
                if (existing != null) { 
                    existing.Quantity += EnteredQuantity;
            }
            else
            {
                var detail = new OrderDetail(EnteredQuantity, SelectedProduct);
                    detail.PropertyChanged += OrderDetail_PropertyChanged;
                    CurrentOrderDetails.Add(detail);
            }
                EnteredQuantity = 0;
                OnPropertyChanged(nameof(EnteredQuantity));
                UpdateTotals();
            }
        }

        private void OrderDetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(OrderDetail.Quantity) ||
                e.PropertyName == nameof(OrderDetail.SubTotal) ||
                e.PropertyName == nameof(OrderDetail.SubProfit) ||
                e.PropertyName == nameof(OrderDetail.Product.SellingPrice) ||
                e.PropertyName == nameof(OrderDetail.Product.PurchasePrice))
            {
                UpdateTotals();
            }
        }

        private void UpdateTotals()
        {
            TotalQuantity = 0;
            TotalAmount = 0;
            TotalProfit = 0;

            foreach (var item in CurrentOrderDetails)
            {
                TotalQuantity += item.Quantity;
                TotalAmount += item.SubTotal;
                TotalProfit += item.SubProfit;
            }

            OnPropertyChanged(nameof(TotalQuantity));
            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(TotalProfit));
        }

        private void SaveOrder(object obj)
        {
            if (SelectedCustomer == null || CurrentOrderDetails.Count == 0)
            {
                MessageBox.Show("Please select a customer and add products.");
                return;
            }

            try
            {
                DB_Queries.UpdateOrder(_originalOrder.ID, SelectedCustomer, CurrentOrderDetails, TotalAmount, TotalProfit);
                MessageBox.Show("Order updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating order: " + ex.Message);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel.OrderRelatedViewModel
{
    public class AddOrderViewModel : BaseViewModel
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

        public AddOrderViewModel()
        {
            LoadCustomers();
            LoadProducts();

            AddProductCommand = new RelayCommand(AddProduct);
            SaveOrderCommand = new RelayCommand(SaveOrder);
        }


        private void LoadCustomers()
        {
            Customers.Clear();
            foreach (var customer in DB_Queries.GetAllCustomers())
            {
                Customers.Add(customer);
            }
        }

        private void LoadProducts()
        {
            Products.Clear();
            foreach (var product in DB_Queries.GetAllProducts())
            {
                Products.Add(product);
            }
        }


        private void AddProduct(object obj)
        {
            if (SelectedProduct != null && EnteredQuantity > 0)
            {
                var existing = CurrentOrderDetails.FirstOrDefault(p => p.Product.ID == SelectedProduct.ID);
                if (existing != null)
                {
                    existing.Quantity += EnteredQuantity;
                }
                else
                {
                    var detail = new OrderDetail(EnteredQuantity, SelectedProduct);
                    CurrentOrderDetails.Add(detail);
                }

                EnteredQuantity = 0;
                OnPropertyChanged(nameof(EnteredQuantity));
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
                MessageBox.Show("Select a customer and add products to the order.");
                return;
            }

            try
            {
                // 1. Insert the order (returns the new OrderID)
                long orderId = DB_Queries.InsertOrder(SelectedCustomer, CurrentOrderDetails);

                // 2. Insert invoice separately using the returned OrderID
                DB_Queries.InsertInvoice(orderId, TotalAmount, TotalProfit);

                MessageBox.Show("Order and Invoice saved successfully!");

                // 3. Reset the UI state
                CurrentOrderDetails.Clear();
                UpdateTotals();
                SelectedCustomer = null;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving order: " + ex.Message);
            }
        }

    }


}



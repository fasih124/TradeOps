using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.Model
{
    public class CustomerOrder : BaseViewModel
    {
        public double TotalPrice => ProductDetails?.Sum(p => p.SubTotal) ?? 0;
        public double TotalProfit => ProductDetails?.Sum(p => p.SubProfit) ?? 0;

        private int _id;
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private bool _isCompleted;

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; }
        }


        private string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }


        // Foreign Key: Helpful for database operations
        private int _customerID;
        public int CustomerID
        {
            get => _customerID;
            set => SetProperty(ref _customerID, value);
        }

        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                if (SetProperty(ref _customer, value) && value != null)
                {
                    CustomerID = value.ID;
                }
            }
        }


        private ObservableCollection<OrderDetail> _productDetails;
        public ObservableCollection<OrderDetail> ProductDetails
        {
            get => _productDetails;
            set => SetProperty(ref _productDetails, value);
        }


        public CustomerOrder(int id, string date, bool isCompleted, Customer customer)
        {
            ID = id;
            Date = date;
            IsCompleted = isCompleted;
            Customer = customer;
            CustomerID = customer?.ID ?? 0;
            ProductDetails = new ObservableCollection<OrderDetail>();
            ProductDetails.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalPrice));
            ProductDetails.CollectionChanged += (s, e) => OnPropertyChanged(nameof(TotalProfit));
        }

        public CustomerOrder()
        {
            ProductDetails = new ObservableCollection<OrderDetail>();
        }

    }
}



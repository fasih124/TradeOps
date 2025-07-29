using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    internal class CustomerOrder : BaseViewModel
    {
        private int _id;
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set => SetProperty(ref _customer, value);
        }

        private Invoice _customerInvoice;
        public Invoice CustomerInvoice
        {
            get => _customerInvoice;
            set => SetProperty(ref _customerInvoice, value);
        }

        private ObservableCollection<OrderDetail> _productDetails;
        public ObservableCollection<OrderDetail> ProductDetails
        {
            get => _productDetails;
            set => SetProperty(ref _productDetails, value);
        }

        public CustomerOrder(int id, string date, Customer customer)
        {
            ID = id;
            Date = date;
            Customer = customer;
            ProductDetails = new ObservableCollection<OrderDetail>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    public class Invoice : BaseViewModel
    {
        private int _id;
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private double _totalProfit;
        public double TotalProfit
        {
            get => _totalProfit;
            set => SetProperty(ref _totalProfit, value);
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        private double _discount;
        public double Discount
        {
            get => _discount;
            set
            {
                if (SetProperty(ref _discount, value))
                {
                    UpdateTotalPriceAfterDiscount();
                }
            }
        }

        private double _finalPrice;
        public double FinalPrice
        {
            get => _finalPrice;
            private set => SetProperty(ref _finalPrice, value);
        }

        private bool _isPaid;
        public bool IsPaid
        {
            get => _isPaid;
            set => SetProperty(ref _isPaid, value);
        }

        private string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private int _orderID;
        public int OrderID
        {
            get => _orderID;
            set => SetProperty(ref _orderID, value);
        }

        private CustomerOrder _customerOrder;
        public CustomerOrder CustomerOrder
        {
            get => _customerOrder;
            set
            {
                if (SetProperty(ref _customerOrder, value) && value != null)
                {
                    CalculateFromOrder(value);
                }
            }
        }

        // Constructors
        public Invoice() { }

        public Invoice(CustomerOrder order, double discount = 0, bool isPaid = false)
        {
            CustomerOrder = order;
            Discount = discount;
            IsPaid = isPaid;
            Date = DateTime.Now.ToString("yyyy-MM-dd");
        }

        // Logic
        private void CalculateFromOrder(CustomerOrder order)
        {
            TotalProfit = order.ProductDetails.Sum(p => p.SubProfit);
            TotalPrice = order.ProductDetails.Sum(p => p.SubTotal);
            UpdateTotalPriceAfterDiscount();
        }

        private void UpdateTotalPriceAfterDiscount()
        {
            FinalPrice = TotalPrice - Discount;
        }
    }

}

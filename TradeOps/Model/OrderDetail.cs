using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    public class OrderDetail : BaseViewModel
    {
        private Product _product;
        public Product Product
        {
            get => _product;
            set
            {
                if (SetProperty(ref _product, value) && value != null)
                {
                    ProductID = value.ID;
                    UpdateCalculatedValues();
                }
            }
        }

        private int _productID;
        public int ProductID
        {
            get => _productID;
            set => SetProperty(ref _productID, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (Product != null && value >= 0)
                {
                    int available = Product.StockQuantity + _quantity; // recover previous reserved stock

                    if (value <= available)
                    {
                        Product.StockQuantity = available - value;
                        SetProperty(ref _quantity, value);
                        UpdateCalculatedValues();
                    }
                }
            }
        }

        private double _subProfit;
        public double SubProfit
        {
            get => _subProfit;
            set => SetProperty(ref _subProfit, value);
        }

        private double _subTotal;
        public double SubTotal
        {
            get => _subTotal;
            set => SetProperty(ref _subTotal, value);
        }

        public OrderDetail(int quantity, Product product)
        {
            _quantity = 0;
            Product = product;

            Quantity = quantity; // Will auto-update stock and calculations
        }
        public OrderDetail() { } 
        private void UpdateCalculatedValues()
        {
            if (Product != null)
            {
                SubProfit = (Product.SellingPrice - Product.PurchasePrice) * Quantity;
                SubTotal = Product.SellingPrice * Quantity;
            }
        }
    }

}

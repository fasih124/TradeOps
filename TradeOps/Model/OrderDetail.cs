using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    internal class OrderDetail : BaseViewModel
    {
    
        private Product _product;
        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (Product?.StockQuantity >= value)
                {
                    int difference = value - _quantity;
                    Product.StockQuantity -= difference;
                    SetProperty(ref _quantity, value);
                    UpdateCalculatedValues();
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
            Product = product;

            if (product.StockQuantity >= quantity)
            {
                Quantity = quantity;
                product.StockQuantity -= quantity;
            }

            UpdateCalculatedValues();
        }

        private void UpdateCalculatedValues()
        {
            if (Product != null)
            {
                SubProfit = (Product.Sellingprice - Product.PurchasePrice) * Quantity;
                SubTotal = Product.Sellingprice * Quantity;
            }
        }
    }
}

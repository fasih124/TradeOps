using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    internal class Product : BaseViewModel
    {
        private int _id;
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private double _purchasePrice;
        public double PurchasePrice
        {
            get => _purchasePrice;
            set => SetProperty(ref _purchasePrice, value);
        }

        private double _sellingPrice;
        public double SellingPrice
        {
            get => _sellingPrice;
            set => SetProperty(ref _sellingPrice, value);
        }

        private int _thresholdLevel;
        public int ThresholdLevel
        {
            get => _thresholdLevel;
            set => SetProperty(ref _thresholdLevel, value);
        }

        private bool isTracked;
        public bool IsTracked
        {
            get => isTracked;
            set => SetProperty(ref isTracked, value);
        }

        private int _stockQuantity;
        public int StockQuantity
        {
            get => _stockQuantity;
            set => SetProperty(ref _stockQuantity, value);
        }


        public Product(int id, string name, double purchasePrice, double sellingPrice, int thresholdLevel, bool istrack, int stock)
        {
            ID = id;
            Name = name;
            PurchasePrice = purchasePrice;
            SellingPrice = sellingPrice;
            ThresholdLevel = thresholdLevel;
            isTracked = istrack;
            StockQuantity = stock;
        }

        public Product() { }



    }
}

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
        private string _id;
        public string ID
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
        public double Sellingprice
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

        private bool _isOutOfSeason;
        public bool IsOutOfSeason
        {
            get => _isOutOfSeason;
            set => SetProperty(ref _isOutOfSeason, value);
        }

        private int _stockQuantity;
        public int StockQuantity
        {
            get => _stockQuantity;
            set => SetProperty(ref _stockQuantity, value);
        }


        public Product(string id, string name, double purchasePrice, double sellingPrice, int thresholdLevel, bool isOutOfSeason, int stock)
        {
            ID = id;
            Name = name;
            PurchasePrice = purchasePrice;
            Sellingprice = sellingPrice;
            ThresholdLevel = thresholdLevel;
            IsOutOfSeason = isOutOfSeason;
            StockQuantity = stock;
        }

        public Product() { }



    }
}

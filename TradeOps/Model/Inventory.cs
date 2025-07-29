using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    internal class Inventory : BaseViewModel
    {
        private int _stockQuantity;
        public int StockQuantity
        {
            get => _stockQuantity;
            set => SetProperty(ref _stockQuantity, value);
        }

        public Inventory(int stockQuantity)
        {
            StockQuantity = stockQuantity;
        }

        public Inventory() { }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOps.Model
{
    internal class Inventory
    {
        private int _stockQuantity;
        public Inventory(int stockquantity) 
        {
            this._stockQuantity = stockquantity;
        }

        public int getstockQuantity() { return this._stockQuantity; }
        public void setstockQuantity(int value) { this._stockQuantity = value; }

    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOps.Model
{
    internal class Product
    {
        public String ID { get; set; }
        public String Name { get; set; }
        public double PurchasePrice { get; set; }
        public double Sellingprice { get; set; }
        public int ThresholdLevel { get; set; }
        public bool IsOutOfSeason { get; set; }

        public Inventory ProductInventory { get; set; }


        public Product(string id,string name,double purchasePrice,double sellingPrice,int thresholdLevel,bool isoutofseason,Inventory inventory)
        {
            this.ID = id;
            this.Name = name;
            this.PurchasePrice = purchasePrice;
            this.Sellingprice = sellingPrice;
            this.ThresholdLevel = thresholdLevel;
            this.IsOutOfSeason = isoutofseason;
            ProductInventory = inventory;
        }



    }
}

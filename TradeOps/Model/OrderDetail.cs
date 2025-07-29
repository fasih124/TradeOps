using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOps.Model
{
    internal class OrderDetail
    {
        public Product Product { get; }
        public int Quantity { get; }
        public double SubProfit { get; }
        public double SubTotal { get;  }
     

        public OrderDetail(int quantity, Product product) 
        {
            if (product.ProductInventory.getstockQuantity() >= quantity )
            {
                this.Quantity = quantity;
                product.ProductInventory.setstockQuantity(product.ProductInventory.getstockQuantity() - quantity);
            }
            this.Product = product;
            this.SubProfit = (Product.Sellingprice - Product.PurchasePrice) * Quantity;
            this.SubTotal = Product.Sellingprice * Quantity;
        }

    }
}

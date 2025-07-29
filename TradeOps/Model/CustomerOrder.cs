using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOps.Model
{
    internal class CustomerOrder
    {
        public int ID { get; set; }
        public string Date{ get; set; }

        public Customer customer { get; set; }
        public List<OrderDetail> ProductDetails { get; set; }

        public Invoice customerInvoice { get; set; }

        public CustomerOrder(int id ,string date ,Customer customer)
        {
            this.ID = id;
            this.Date = date;
            this.customer = customer;
            ProductDetails = new List<OrderDetail>();
        }

     
    }
}

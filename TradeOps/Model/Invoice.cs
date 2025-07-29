using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOps.Model
{
    internal class Invoice
    {
        public double TotalProfit { get; set; }
        public double TotalPrice { get; set; }
        public double Discount { get; set; }
        public bool IsPaid { get; set; }
        public string Date { get; set; }


        public Invoice(double totalprofit, double totalprice, double discount, bool ispaid,string date) 
        {
            this.TotalProfit = totalprofit;
            this.TotalPrice = totalprice;   
            this.Discount = discount;
            this.IsPaid = ispaid;
            this.Date = date;
        }


    }
}

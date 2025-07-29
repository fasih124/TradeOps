using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOps.Model
{
    internal class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string PhoneNumber { get; set; } 

        public Customer(int id,string name, string address , string  area , string  phonenumber)
        {
            this.ID = id;
            this.Name = name;
            this.Address = address;
            this.Area = area;
            this.PhoneNumber = phonenumber;
        }

    }
}

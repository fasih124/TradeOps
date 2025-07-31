using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    public class Customer : BaseViewModel
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

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _area;
        public string Area
        {
            get => _area;
            set => SetProperty(ref _area, value);
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        public Customer(int id, string name, string address, string area, string phoneNumber)
        {
            ID = id;
            Name = name;
            Address = address;
            Area = area;
            PhoneNumber = phoneNumber;
        }


        public Customer() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;

namespace TradeOps.Model
{
    internal class Invoice : BaseViewModel
    {
      private double _totalProfit;
    public double TotalProfit
    {
        get => _totalProfit;
        set => SetProperty(ref _totalProfit, value);
    }

    private double _totalPrice;
    public double TotalPrice
    {
        get => _totalPrice;
        set => SetProperty(ref _totalPrice, value);
    }

    private double _discount;
    public double Discount
    {
        get => _discount;
        set => SetProperty(ref _discount, value);
    }

    private bool _isPaid;
    public bool IsPaid
    {
        get => _isPaid;
        set => SetProperty(ref _isPaid, value);
    }

    private string _date;
    public string Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public Invoice(double totalProfit, double totalPrice, double discount, bool isPaid, string date)
    {
        TotalProfit = totalProfit;
        TotalPrice = totalPrice;
        Discount = discount;
        IsPaid = isPaid;
        Date = date;
    }

    public Invoice() { }


    }
}

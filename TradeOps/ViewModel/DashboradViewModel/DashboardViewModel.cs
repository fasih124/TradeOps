using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel.DashboradViewModel
{
    internal class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<ISeries> SalesSeries { get; set; }
        public ObservableCollection<ISeries> ProfitSeries { get; set; }

        public ObservableCollection<Axis> XAxes { get; set; }
        public ObservableCollection<Axis> YAxes { get; set; }


        private int _totalProducts;
        public int TotalProducts
        {
            get => _totalProducts;
            set => SetProperty(ref _totalProducts, value);
        }

        private int _lowStockProducts;
        public int LowStockProducts
        {
            get => _lowStockProducts;
            set => SetProperty(ref _lowStockProducts, value);
        }

        private int _outOfStockProducts;
        public int OutOfStockProducts
        {
            get => _outOfStockProducts;
            set => SetProperty(ref _outOfStockProducts, value);
        }

        private int _totalInvoices;
        public int TotalInvoices
        {
            get => _totalInvoices;
            set => SetProperty(ref _totalInvoices, value);
        }

        private int _completedInvoices;
        public int CompletedInvoices
        {
            get => _completedInvoices;
            set => SetProperty(ref _completedInvoices, value);
        }

        private int _pendingInvoices;
        public int PendingInvoices
        {
            get => _pendingInvoices;
            set => SetProperty(ref _pendingInvoices, value);
        }


        private double _totalSales;
        public double TotalSales
        {
            get => _totalSales;
            set => SetProperty(ref _totalSales, value);
        }

        private double _totalProfit;
        public double TotalProfit
        {
            get => _totalProfit;
            set => SetProperty(ref _totalProfit, value);
        }


        private int _totalOrders;
        public int TotalOrders
        {
            get => _totalOrders;
            set => SetProperty(ref _totalOrders, value);
        }

        private int _pendingOrders;
        public int PendingOrders
        {
            get => _pendingOrders;
            set => SetProperty(ref _pendingOrders, value);
        }

        private int _totalCustomers;
        public int TotalCustomers
        {
            get => _totalCustomers;
            set => SetProperty(ref _totalCustomers, value);
        }

        private ObservableCollection<CustomerSummary> _topCustomersBySales;
        public ObservableCollection<CustomerSummary> TopCustomersBySales
        {
            get => _topCustomersBySales;
            set
            {
                SetProperty(ref _topCustomersBySales, value);
                OnPropertyChanged(nameof(HasTopCustomersBySales));
            }
        }

        private ObservableCollection<CustomerSummary> _topCustomersByProfit;
        public ObservableCollection<CustomerSummary> TopCustomersByProfit
        {
            get => _topCustomersByProfit;
            set
            {
                SetProperty(ref _topCustomersByProfit, value);
                OnPropertyChanged(nameof(HasTopCustomersByProfit));
            }
        }

        public bool HasTopCustomersBySales => TopCustomersBySales?.Any() == true;
        public bool HasTopCustomersByProfit => TopCustomersByProfit?.Any() == true;



        public ObservableCollection<object> TopCustomersBySalesWithSrNo { get; set; }
        public ObservableCollection<object> TopCustomersByProfitWithSrNo { get; set; }

        

        private void LoadAllDashboardData()
        {
            LoadSaleDashboardData();
            LoadOrderDashboardStats();
            LoadInvoiceDashboardStats();
            LoadProductDashboardStats();
            LoadCustomerDashboardStats();
            LoadTopCustomers();
            LoadChartData();
        }

        public DashboardViewModel()
        {
            LoadAllDashboardData();
        }


        public void LoadTopCustomers()
        {
            var sales = DB_Queries.GetTop10CustomersBySales();
            var profit = DB_Queries.GetTop10CustomersByProfit();

            // Set original collections
            TopCustomersBySales = new ObservableCollection<CustomerSummary>(sales);
            TopCustomersByProfit = new ObservableCollection<CustomerSummary>(profit);

            // Add Sr No dynamically into a wrapper object
            TopCustomersBySalesWithSrNo = new ObservableCollection<object>(
                sales.Select((item, index) => new
                {
                    SrNo = index + 1,
                    item.CustomerName,
                    item.TotalSale 
                })
            );

            TopCustomersByProfitWithSrNo = new ObservableCollection<object>(
                profit.Select((item, index) => new
                {
                    SrNo = index + 1,
                    item.CustomerName,
                    item.TotalProfit
                })
            );

            OnPropertyChanged(nameof(TopCustomersBySalesWithSrNo));
            OnPropertyChanged(nameof(TopCustomersByProfitWithSrNo));
            OnPropertyChanged(nameof(HasTopCustomersBySales));
            OnPropertyChanged(nameof(HasTopCustomersByProfit));
        }


        public void LoadOrderDashboardStats()
        {
            TotalOrders = DB_Queries.GetTotalOrderCount();
            PendingOrders = DB_Queries.GetPendingOrderCount();
        }
        public void LoadCustomerDashboardStats()
        {
            TotalCustomers = DB_Queries.GetTotalCustomerCount();
        }

        public void LoadSaleDashboardData()
        {
            using var con = DB_Queries.GetConnection();
            con.Open();

            using var cmd1 = new SQLiteCommand("SELECT SUM(total_price) FROM Invoice", con);
            TotalSales = Convert.ToDouble(cmd1.ExecuteScalar() ?? 0);

            using var cmd2 = new SQLiteCommand("SELECT SUM(total_profit) FROM Invoice", con);
            TotalProfit = Convert.ToDouble(cmd2.ExecuteScalar() ?? 0);
        }

        private void LoadInvoiceDashboardStats()
        {
            TotalInvoices = DB_Queries.GetTotalInvoiceCount();
            CompletedInvoices = DB_Queries.GetCompletedInvoiceCount();
            PendingInvoices = DB_Queries.GetPendingInvoiceCount();
        }

        public void LoadProductDashboardStats()
        {
            TotalProducts = DB_Queries.GetTotalProductCount();
            LowStockProducts = DB_Queries.GetLowStockProductCount();
            OutOfStockProducts = DB_Queries.GetOutOfStockProductCount();
        }

        private void LoadChartData()
        {
            var salesData = DB_Queries.GetSalesOverTime();
            var profitData = DB_Queries.GetProfitOverTime();

            // Extract labels and values
            var salesLabels = salesData.Keys.ToList();
            var salesValues = salesData.Values.ToList();

            var profitLabels = profitData.Keys.ToList();
            var profitValues = profitData.Values.ToList();


            SalesSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<double>
            {
                Values = salesValues,
                Name = "Sales",
                GeometrySize = 10
            }
        };

            ProfitSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<double>
            {
                Values = profitValues,
                Name = "Profit",
                GeometrySize = 10
            }
        };

            XAxes = new ObservableCollection<Axis>
        {
            new Axis
            {
                Labels = salesData.Keys.ToList(),
                LabelsRotation = 15
            }
        };

            YAxes = new ObservableCollection<Axis>
        {
            new Axis
            {
                Labeler = value => value.ToString("N0")
            }
        };
        }

    }

}

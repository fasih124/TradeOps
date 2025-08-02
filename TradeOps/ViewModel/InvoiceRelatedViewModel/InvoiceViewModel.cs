using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Helper;
using TradeOps.Model;

namespace TradeOps.ViewModel.InvoiceRelatedViewModel
{
    internal class InvoiceViewModel : BaseViewModel
    {
        private List<Invoice> _allInvoices;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterInvoices();
                }
            }
        }

        private string _searchType ;
        public string SearchType
        {
            get => _searchType;
            set
            {
                if (SetProperty(ref _searchType, value))
                {
                    FilterInvoices();
                }
            }
        }

    

        private ObservableCollection<Invoice> _invoices;
        public ObservableCollection<Invoice> Invoices
        {
            get => _invoices;
            set => SetProperty(ref _invoices, value);
        }

        private Invoice _selectedInvoice;
        public Invoice SelectedInvoice
        {
            get => _selectedInvoice;
            set => SetProperty(ref _selectedInvoice, value);
        }



        public InvoiceViewModel()
        {
            SearchType = "Invoice ID";  // or "Order ID" or "Date" — whatever your default is
            LoadInvoices();
            FilterInvoices();
      
        }

        private void LoadInvoices() 
        {
            _allInvoices = DB_Queries.GetAllInvoices();
            Invoices = new ObservableCollection<Invoice>(_allInvoices);
            System.Diagnostics.Debug.WriteLine($"Loaded {Invoices.Count} Invoices.");
        }

        private void FilterInvoices()
        {
            if (_allInvoices == null)
                return;

            var trimmedText = SearchText?.Trim();

            if (string.IsNullOrWhiteSpace(trimmedText))
            {
                Invoices = new ObservableCollection<Invoice>(_allInvoices);
                return;
            }

            string filter = trimmedText.ToLower();
            IEnumerable<Invoice> filtered;

            switch (SearchType)
            {
                case "Invoice ID":
                    filtered = _allInvoices.Where(inv => inv.ID.ToString().Contains(filter));
                    break;

                case "Order ID":
                    filtered = _allInvoices.Where(inv => inv.OrderID.ToString().Contains(filter));
                    break;

                case "Date":
                    filtered = _allInvoices.Where(inv => inv.Date != null && inv.Date.ToLower().Contains(filter));
                    break;

                default:
                    filtered = _allInvoices;
                    break;
            }

            Invoices = new ObservableCollection<Invoice>(filtered);
        }


    }
}

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
        public ObservableCollection<Invoice> Invoices { get; set; } = new();
        public Invoice SelectedInvoice { get; set; }

        public InvoiceViewModel()
        {
            LoadInvoices();
        }

        private void LoadInvoices()
        {
            var invoiceList = DB_Queries.GetAllInvoices(); // You’ll add this method next
            Invoices.Clear();
            foreach (var invoice in invoiceList)
            {
                Invoices.Add(invoice);
            }
        }
    }
}

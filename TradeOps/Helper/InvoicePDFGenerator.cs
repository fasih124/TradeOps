using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Model;

namespace TradeOps.Helper
{
    public static class InvoicePDFGenerator
    {
        public static void GenerateInvoicePDF(Invoice invoice)
        {
            try
            {
                GlobalFontSettings.UseWindowsFontsUnderWindows = true;
                // Create Invoices directory
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // File path
                string filePath = Path.Combine(folderPath, $"Invoice_{invoice.ID}.pdf");

                // Create new PDF document
                PdfDocument document = new PdfDocument();
                document.Info.Title = $"Invoice #{invoice.ID}";
                PdfPage page = document.AddPage();

                // Graphics and Fonts
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 20, XFontStyleEx.Bold);     // Use Arial for better compatibility
                XFont bodyFont = new XFont("Arial", 12, XFontStyleEx.Regular);

                double y = 40;

                // Title
                gfx.DrawString("INVOICE", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
                y += 60;

                // Invoice Details
                gfx.DrawString($"Invoice ID: {invoice.ID}", bodyFont, XBrushes.Black, 40, y); y += 25;
                gfx.DrawString($"Date: {invoice.Date}", bodyFont, XBrushes.Black, 40, y); y += 25;
                gfx.DrawString($"Total Price: {invoice.TotalPrice:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
                gfx.DrawString($"Discount: {invoice.Discount:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
                gfx.DrawString($"Net Total: {invoice.FinalPrice:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
                gfx.DrawString($"Paid: {(invoice.IsPaid ? "Yes" : "No")}", bodyFont, XBrushes.Black, 40, y); y += 25;

                // Save 
                document.Save(filePath);
                //open
                //Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error generating invoice PDF: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }


        //public static void GenerateInvoicePDF(int invoiceId, string invoiceDate, double totalPrice, double discount, double finalPrice, bool isPaid)
        //{
        //    try
        //    {
        //        // Create Invoices directory
        //        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
        //        if (!Directory.Exists(folderPath))
        //            Directory.CreateDirectory(folderPath);

        //        // File path
        //        string filePath = Path.Combine(folderPath, $"Invoice_{invoiceId}.pdf");

        //        // Create new PDF document
        //        PdfDocument document = new PdfDocument();
        //        document.Info.Title = $"Invoice #{invoiceId}";
        //        PdfPage page = document.AddPage();

        //        // Graphics and Fonts
        //        XGraphics gfx = XGraphics.FromPdfPage(page);
        //        XFont titleFont = new XFont("Arial", 20, XFontStyleEx.Bold);     // Use Arial for better compatibility
        //        XFont bodyFont = new XFont("Arial", 12, XFontStyleEx.Regular);

        //        double y = 40;

        //        // Title
        //        gfx.DrawString("INVOICE", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
        //        y += 60;

        //        // Invoice Details
        //        gfx.DrawString($"Invoice ID: {invoiceId}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Date: {invoiceDate}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Total Price: {totalPrice:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Discount: {discount:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Net Total: {finalPrice:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Paid: {(isPaid ? "Yes" : "No")}", bodyFont, XBrushes.Black, 40, y); y += 25;

        //        // Save and open
        //        document.Save(filePath);
        //        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.MessageBox.Show($"Error generating invoice PDF: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        //    }
        //}
    }
}
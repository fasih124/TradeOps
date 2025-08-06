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
        //public static void GenerateInvoicePDF(Invoice invoice)
        //{
        //    try
        //    {
        //        GlobalFontSettings.UseWindowsFontsUnderWindows = true;
        //        // Create Invoices directory
        //        string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Invoices");
        //        if (!Directory.Exists(folderPath))
        //            Directory.CreateDirectory(folderPath);

        //        // File path
        //        string filePath = Path.Combine(folderPath, $"Invoice_{invoice.ID}.pdf");

        //        // Create new PDF document
        //        PdfDocument document = new PdfDocument();
        //        document.Info.Title = $"Invoice #{invoice.ID}";
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
        //        gfx.DrawString($"Invoice ID: {invoice.ID}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Date: {invoice.Date}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Total Price: {invoice.TotalPrice:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Discount: {invoice.Discount:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Net Total: {invoice.FinalPrice:C}", bodyFont, XBrushes.Black, 40, y); y += 25;
        //        gfx.DrawString($"Paid: {(invoice.IsPaid ? "Yes" : "No")}", bodyFont, XBrushes.Black, 40, y); y += 25;

        //        // Save 
        //        document.Save(filePath);
        //        //open
        //        //Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.MessageBox.Show($"Error generating invoice PDF: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        //    }
        //}


        public static void GenerateInvoicePdf(Invoice invoice, CustomerOrder order)
        {
            var doc = new PdfDocument();
            doc.Info.Title = "Invoice";
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12, XFontStyleEx.Regular);
            var boldFont = new XFont("Arial", 12, XFontStyleEx.Bold);
            double yPoint = 40;

            // --- Order Details ---
            gfx.DrawString("Order Details", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Order ID: {order.ID}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Order Date: {order.Date}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            // --- Customer Details ---
            yPoint += 30;
            gfx.DrawString("Customer Details", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Customer Name: {order.Customer?.Name}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Customer ID: {order.Customer?.ID}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            // --- Product Details Header ---
            yPoint += 30;
            gfx.DrawString("Product Details", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;

            gfx.DrawString("Name", boldFont, XBrushes.Black, new XRect(60, yPoint, 150, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Qty", boldFont, XBrushes.Black, new XRect(220, yPoint, 50, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Unit Price", boldFont, XBrushes.Black, new XRect(280, yPoint, 100, page.Height), XStringFormats.TopLeft);
            gfx.DrawString("Subtotal", boldFont, XBrushes.Black, new XRect(400, yPoint, 100, page.Height), XStringFormats.TopLeft);

            yPoint += 20;

            // --- Product Details Rows ---
            foreach (var detail in order.ProductDetails)
            {
                gfx.DrawString(detail.Product?.Name, font, XBrushes.Black, new XRect(60, yPoint, 150, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(detail.Quantity.ToString(), font, XBrushes.Black, new XRect(220, yPoint, 50, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(detail.Product.SellingPrice.ToString("F2"), font, XBrushes.Black, new XRect(280, yPoint, 100, page.Height), XStringFormats.TopLeft);
                gfx.DrawString(detail.SubTotal.ToString("F2"), font, XBrushes.Black, new XRect(400, yPoint, 100, page.Height), XStringFormats.TopLeft);
                yPoint += 20;
            }

            // --- Invoice Summary ---
            yPoint += 30;
            gfx.DrawString("Invoice Summary", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Invoice ID: {invoice.ID}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Total: {invoice.TotalPrice:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            gfx.DrawString($"Discount: {invoice.Discount:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
            yPoint += 20;
            double finalPrice = invoice.TotalPrice - invoice.Discount;
            gfx.DrawString($"Net Total: {finalPrice:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            //gfx.DrawString($"Net Total: {invoice.FinalPrice:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

            // --- Save File ---
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"Invoice_{invoice.ID}.pdf";
            string fullPath = Path.Combine(folderPath, fileName);

            doc.Save(fullPath);
            Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
        }

    }
}
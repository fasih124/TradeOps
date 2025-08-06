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
using System.Windows;
using TradeOps.Model;

namespace TradeOps.Helper
{
    public static class InvoicePDFGenerator
    {
        //public static void GenerateInvoicePdf(Invoice invoice, CustomerOrder order)
        //{
        //    try
        //    {
        //        GlobalFontSettings.UseWindowsFontsUnderWindows = true;

        //        var doc = new PdfDocument();
        //        doc.Info.Title = "Invoice";
        //        var page = doc.AddPage();
        //        var gfx = XGraphics.FromPdfPage(page);
        //        var font = new XFont("Arial", 12, XFontStyleEx.Regular);
        //        var boldFont = new XFont("Arial", 12, XFontStyleEx.Bold);
        //        double yPoint = 40;

        //        // --- Order Details ---
        //        gfx.DrawString("Order Details", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Order ID: \t\t{order.ID}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Order Date: \t\t{order.Date}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

        //        // --- Customer Details ---
        //        yPoint += 30;
        //        gfx.DrawString("Customer Details", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Customer Name: \t\t {order.Customer.Name}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Customer ID: \t\t{order.CustomerID}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

        //        // --- Product Details Header ---
        //        yPoint += 30;
        //        gfx.DrawString("Product Details", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;

        //        // Table Headers
        //        gfx.DrawString("Name", boldFont, XBrushes.Black, new XRect(60, yPoint, 150, page.Height), XStringFormats.TopLeft);
        //        gfx.DrawString("Qty", boldFont, XBrushes.Black, new XRect(220, yPoint, 50, page.Height), XStringFormats.TopLeft);
        //        gfx.DrawString("Unit Price", boldFont, XBrushes.Black, new XRect(280, yPoint, 100, page.Height), XStringFormats.TopLeft);
        //        gfx.DrawString("Subtotal", boldFont, XBrushes.Black, new XRect(400, yPoint, 100, page.Height), XStringFormats.TopLeft);

        //        yPoint += 20;

        //        // Product Detail Rows
        //        foreach (var detail in order.ProductDetails)
        //        {
        //            gfx.DrawString(detail.Product?.Name, font, XBrushes.Black, new XRect(60, yPoint, 150, page.Height), XStringFormats.TopLeft);
        //            gfx.DrawString(detail.Quantity.ToString(), font, XBrushes.Black, new XRect(220, yPoint, 50, page.Height), XStringFormats.TopLeft);
        //            gfx.DrawString(detail.Product.SellingPrice.ToString("F2"), font, XBrushes.Black, new XRect(280, yPoint, 100, page.Height), XStringFormats.TopLeft);
        //            gfx.DrawString(detail.SubTotal.ToString("F2"), font, XBrushes.Black, new XRect(400, yPoint, 100, page.Height), XStringFormats.TopLeft);
        //            yPoint += 20;
        //        }

        //        // --- Invoice Summary ---
        //        yPoint += 30;
        //        gfx.DrawString("Invoice Summary", boldFont, XBrushes.Black, new XRect(40, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Invoice ID: \t\t{invoice.ID}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Total: \t\t{invoice.TotalPrice:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;
        //        gfx.DrawString($"Discount: \t\t{invoice.Discount:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);
        //        yPoint += 20;

        //        double finalPrice = invoice.TotalPrice - invoice.Discount;
        //        gfx.DrawString($"Net Total: \t\t{finalPrice:F2}", font, XBrushes.Black, new XRect(60, yPoint, page.Width, page.Height), XStringFormats.TopLeft);

        //        // --- Save File ---
        //        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //        string fileName = $"Invoice_{invoice.ID}.pdf";
        //        string fullPath = Path.Combine(folderPath, fileName);

        //        doc.Save(fullPath);
        //        // Optionally open the file:
        //        // Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error generating invoice PDF:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        public static void GenerateInvoicePdf(Invoice invoice, CustomerOrder order)
        {
            try
            {
                GlobalFontSettings.UseWindowsFontsUnderWindows = true;

                var doc = new PdfDocument();
                doc.Info.Title = "Invoice";
                var page = doc.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12, XFontStyleEx.Regular);
                var boldFont = new XFont("Arial", 12, XFontStyleEx.Bold);
                var titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
                var netTotalFont = new XFont("Arial", 14, XFontStyleEx.Bold);

                double margin = 40;
                double yPoint = margin;

                // --- Black Line on Top ---
                gfx.DrawLine(XPens.Black, margin, yPoint - 20, page.Width - margin, yPoint - 20);

                // --- Title Centered ---
                gfx.DrawString("TrackOps", titleFont, XBrushes.Black,
                    new XRect(0, yPoint - 15, page.Width, 20), XStringFormats.TopCenter);

               
                yPoint += 10; 

                // --- Top Info Line (Order ID, Customer ID, Invoice ID) Centered and Spaced ---
                double infoY = yPoint;
                double sectionWidth = (page.Width - 2 * margin) / 3;

                gfx.DrawString($"Order ID: OR{order.ID}", boldFont, XBrushes.Black,
                    new XRect(margin, infoY, sectionWidth, 20), XStringFormats.TopLeft);

                gfx.DrawString($"Customer ID: CUS{order.CustomerID}", boldFont, XBrushes.Black,
                    new XRect(margin + sectionWidth, infoY, sectionWidth, 20), XStringFormats.TopCenter);

                gfx.DrawString($"Invoice ID: INV{invoice.ID}", boldFont, XBrushes.Black,
                    new XRect(margin + 2 * sectionWidth, infoY, sectionWidth, 20), XStringFormats.TopRight);

                yPoint += 25;

                // --- Customer Info Line (Customer Name, Order Date) ---
                string customerName = order.Customer?.Name ?? "Unknown Customer";

                gfx.DrawString($"Customer Name: {customerName}", font, XBrushes.Black,
                    new XRect(margin, yPoint, sectionWidth, 20), XStringFormats.TopLeft);

                gfx.DrawString($"Order Date: {order.Date:dd-MM-yyyy}", font, XBrushes.Black,
                    new XRect(page.Width - margin - sectionWidth, yPoint, sectionWidth, 20), XStringFormats.TopRight);

                yPoint += 30;

                // --- Product Table Header ---
                double tableLeft = margin;
                double tableWidth = page.Width - 2 * margin;
                double[] columnWidths = { 40, 150, 60, 80, 100 };
                double tableRowHeight = 25;

                gfx.DrawRectangle(XBrushes.LightGray, tableLeft, yPoint, tableWidth, tableRowHeight);
                gfx.DrawRectangle(XPens.Black, tableLeft, yPoint, tableWidth, tableRowHeight);

                double x = tableLeft;
                string[] headers = { "Sr#", "Name", "Qty", "Unit Price", "Subtotal" };
                for (int i = 0; i < headers.Length; i++)
                {
                    gfx.DrawString(headers[i], boldFont, XBrushes.Black,
                        new XRect(x + 5, yPoint + 5, columnWidths[i], tableRowHeight), XStringFormats.TopLeft);
                    x += columnWidths[i];
                }

                yPoint += tableRowHeight;

                // --- Product Rows ---
                int sr = 1;
                foreach (var detail in order.ProductDetails)
                {
                    x = tableLeft;
                    gfx.DrawRectangle(XPens.Black, tableLeft, yPoint, tableWidth, tableRowHeight);

                    string[] rowValues = {
                sr.ToString(),
                detail.Product?.Name ?? "N/A",
                detail.Quantity.ToString(),
                detail.Product?.SellingPrice.ToString("F2") ?? "0.00",
                detail.SubTotal.ToString("F2")
            };

                    for (int i = 0; i < rowValues.Length; i++)
                    {
                        gfx.DrawString(rowValues[i], font, XBrushes.Black,
                            new XRect(x + 5, yPoint + 5, columnWidths[i], tableRowHeight), XStringFormats.TopLeft);
                        x += columnWidths[i];
                    }

                    yPoint += tableRowHeight;
                    sr++;
                }

                yPoint += 30;

                // --- Summary Line ---
                double summaryLeft = margin;
                gfx.DrawString("Total: " + invoice.TotalPrice.ToString("F2"), font, XBrushes.Black,
                    new XRect(summaryLeft, yPoint, 150, page.Height), XStringFormats.TopLeft);

                gfx.DrawString("Discount: " + invoice.Discount.ToString("F2"), font, XBrushes.Black,
                    new XRect(summaryLeft + 200, yPoint, 150, page.Height), XStringFormats.TopLeft);

                gfx.DrawString("Net Total: " + (invoice.TotalPrice - invoice.Discount).ToString("F2"), netTotalFont, XBrushes.Black,
                    new XRect(summaryLeft + 400, yPoint, 150, page.Height), XStringFormats.TopLeft);

                yPoint += 30;

                // --- Black Line at Bottom ---
                gfx.DrawLine(XPens.Black, margin, yPoint + 10, page.Width - margin, yPoint + 10);

                // --- Thank You Note ---
                gfx.DrawString("Thank you for your business!", font, XBrushes.Black,
                    new XRect(0, yPoint + 20, page.Width, 20), XStringFormats.Center);

                // --- Save to PDF ---
                string exePath = AppDomain.CurrentDomain.BaseDirectory;
                string invoiceFolder = Path.Combine(exePath, "Invoices");
                if (!Directory.Exists(invoiceFolder)) Directory.CreateDirectory(invoiceFolder);

                string fileName = $"Invoice_{invoice.ID}.pdf";
                string fullPath = Path.Combine(invoiceFolder, fileName);

                doc.Save(fullPath);
                // Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating invoice PDF:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





    }
}
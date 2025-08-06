using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeOps.Helper
{
    internal class DataManagement
    {
        public static void ResetDatabase()
        {
           
          
            using (var connection = DB_Queries.GetConnection())
            {
                connection.Open();

                var tables = new List<string> { "OrderDetail", "Invoice", "CustomerOrder", "Customer", "Product"};

                foreach (var table in tables)
                {
                    using var cmd = new SQLiteCommand($"DELETE FROM {table};", connection);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("All data deleted successfully!", "Reset", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        public static void BackupData()
        {
            string backupFolder = "BackupCSV";
            Directory.CreateDirectory(backupFolder);

            ExportTableToCSV("Customer", Path.Combine(backupFolder, "Customer.csv"));
            ExportTableToCSV("Product", Path.Combine(backupFolder, "Product.csv"));
            ExportTableToCSV("CustomerOrder", Path.Combine(backupFolder, "CustomerOrder.csv"));
            ExportTableToCSV("OrderDetail", Path.Combine(backupFolder, "OrderDetail.csv"));
            ExportTableToCSV("Invoice", Path.Combine(backupFolder, "Invoice.csv"));

        }

        public static void ExportTableToCSV(string tableName, string filePath)
        {
            using var con = DB_Queries.GetConnection();
            con.Open();

            using var cmd = new SQLiteCommand($"SELECT * FROM {tableName}", con);
            using var reader = cmd.ExecuteReader();

            using var writer = new StreamWriter(filePath);

            // Write header
            var columnNames = Enumerable.Range(0, reader.FieldCount)
                                        .Select(i => reader.GetName(i));
            writer.WriteLine(string.Join(",", columnNames));

            // Write rows
            while (reader.Read())
            {
                var values = Enumerable.Range(0, reader.FieldCount)
                                       .Select(i => reader.GetValue(i)?.ToString()?.Replace(",", " ") ?? "");
                writer.WriteLine(string.Join(",", values));
            }
        }

        public static void ImportCSVToTable(string tableName, string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length < 2) return; // No data

            var headers = lines[0].Split(',');

            using var con = DB_Queries.GetConnection();
            con.Open();

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');

                var columnNames = string.Join(", ", headers);
                var paramPlaceholders = string.Join(", ", headers.Select(h => $"@{h}"));

                using var cmd = new SQLiteCommand($"INSERT INTO {tableName} ({columnNames}) VALUES ({paramPlaceholders})", con);

                for (int i = 0; i < headers.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@{headers[i]}", values[i]);
                }

                cmd.ExecuteNonQuery();
            }
        }


        public static void loadData() 
        {
            string backupFolder = "BackupCSV";
            Directory.CreateDirectory(backupFolder);

            ImportCSVToTable("Customer", Path.Combine(backupFolder, "Customer.csv"));
            ImportCSVToTable("Product", Path.Combine(backupFolder, "Product.csv"));
            ImportCSVToTable("CustomerOrder", Path.Combine(backupFolder, "CustomerOrder.csv"));
            ImportCSVToTable("OrderDetail", Path.Combine(backupFolder, "OrderDetail.csv"));
            ImportCSVToTable("Invoice", Path.Combine(backupFolder, "Invoice.csv"));
        }
    }
}

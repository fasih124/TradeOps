using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Model;

namespace TradeOps.Helper
{
    class DB_Queries
    {
        private static readonly string dbPath = "TrackOps.db";

        private static SQLiteConnection GetConnection()
        {
      

            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        public static ObservableCollection<Product> GetAllProducts()
        {
            var products = new ObservableCollection<Product>();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Product";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ID = reader["ID"].ToString(),
                                Name = reader["name"].ToString(),
                                PurchasePrice = double.Parse(reader["purchase_price"].ToString()),
                                SellingPrice = double.Parse(reader["selling_price"].ToString()),
                                ThresholdLevel = int.Parse(reader["threshold_Level"].ToString()),
                                StockQuantity = int.Parse(reader["inventory_Stock"].ToString()),
                                IsTracked = reader["isTracked"].ToString() == "1"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DB ERROR: " + ex.Message);
            }

            return products;
        }

    }
}

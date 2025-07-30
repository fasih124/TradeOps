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
        private static readonly string dbPath = "Assets\\TrackOps.db";

        private static SQLiteConnection GetConnection()
        {
     

            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        //===============================
        //Product Related Queries
        //================================
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
                                //IsTracked = reader["isTracked"].ToString() == "1"
                                IsTracked=Convert.ToBoolean(reader["isTracked"])
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

        public static bool InsertProduct(Product product)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO Product (ID, name, purchase_price, selling_price, threshold_Level, inventory_Stock, isTracked)
                             VALUES (@ID, @Name, @PurchasePrice, @SellingPrice, @ThresholdLevel, @StockQuantity, @IsTracked)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", product.ID);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@PurchasePrice", product.PurchasePrice);
                        command.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                        command.Parameters.AddWithValue("@ThresholdLevel", product.ThresholdLevel);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@IsTracked", product.IsTracked ? 1 : 0);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Insert ERROR: " + ex.Message);
                return false;
            }
        }

        public static void UpdateProduct(Product product)
        {
            using (var con = GetConnection())
            {
                con.Open();
                var cmd = new SQLiteCommand("UPDATE Product SET name = @Name, purchase_price = @PurchasePrice, selling_price = @SellingPrice, threshold_Level = @ThresholdLevel, inventory_Stock = @StockQuantity, isTracked = @IsTracked WHERE ID = @ID", con);

                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@PurchasePrice", product.PurchasePrice);
                cmd.Parameters.AddWithValue("@SellingPrice", product.SellingPrice);
                cmd.Parameters.AddWithValue("@ThresholdLevel", product.ThresholdLevel);
                cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                cmd.Parameters.AddWithValue("@IsTracked", product.IsTracked);
                cmd.Parameters.AddWithValue("@ID", product.ID);

                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteProduct(Product product)
        {
            using (var con = GetConnection())
            {
                con.Open();
                var cmd = new SQLiteCommand("DELETE FROM Product WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", product.ID);
                cmd.ExecuteNonQuery();
            }
        }

        public static int GetNextProductId()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT IFNULL(MAX(ID), 0) + 1 FROM Product", connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        //===============================
        //Product Related Queries
        //================================

    }
}

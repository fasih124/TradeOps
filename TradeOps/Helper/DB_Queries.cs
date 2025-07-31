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
                                ID = int.Parse(reader["ID"].ToString()),
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
        //customer Related Queries
        //================================

        public static ObservableCollection<Customer> GetAllCustomers()
        {
            var customers = new ObservableCollection<Customer>();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Customer";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["name"].ToString(),
                                Address = reader["address"].ToString(),
                                Area = reader["area"].ToString(),
                                PhoneNumber = reader["phone_number"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DB ERROR: " + ex.Message);
            }

            return customers;
        }


        public static bool InsertCustomer(Customer customer)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO Customer (ID, name, address, area, phone_number)
                             VALUES (@ID, @Name, @Address, @Area, @PhoneNumber)";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", customer.ID);
                        command.Parameters.AddWithValue("@Name", customer.Name);
                        command.Parameters.AddWithValue("@Address", customer.Address);
                        command.Parameters.AddWithValue("@Area", customer.Area);
                        command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);

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


        public static void UpdateCustomer(Customer customer)
        {
            using (var con = GetConnection())
            {
                con.Open();
                var cmd = new SQLiteCommand("UPDATE Customer SET name = @Name, address = @Address, area = @Area, phone_number = @PhoneNumber WHERE ID = @ID", con);

                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Area", customer.Area);
                cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@ID", customer.ID);

                cmd.ExecuteNonQuery();
            }
        }


        public static void DeleteCustomer(Customer customer)
        {
            using (var con = GetConnection())
            {
                con.Open();
                var cmd = new SQLiteCommand("DELETE FROM Customer WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", customer.ID);
                cmd.ExecuteNonQuery();
            }
        }


        public static int GetNextCustomerId()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT IFNULL(MAX(ID), 0) + 1 FROM Customer", connection);
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        //===============================
        //Product Related Queries
        //================================

    }
}

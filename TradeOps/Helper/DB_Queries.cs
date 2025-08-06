using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeOps.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TradeOps.Helper
{
    public class DB_Queries
    {
        private static readonly string dbPath = "Assets\\TrackOps.db";

        public static SQLiteConnection GetConnection()
        {


            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }


        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }

            using var con = GetConnection();
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS Product (
            ID INTEGER PRIMARY KEY,
            name TEXT NOT NULL,
            purchase_price REAL NOT NULL,
            selling_price REAL NOT NULL,
            threshold_Level INTEGER NOT NULL,
            inventory_Stock INTEGER NOT NULL,
            isTracked BOOLEAN NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Customer (
            ID INTEGER PRIMARY KEY,
            name TEXT NOT NULL,
            address TEXT,
            area TEXT,
            phone_number TEXT
        );

        CREATE TABLE IF NOT EXISTS CustomerOrder (
            ID INTEGER PRIMARY KEY,
            Date TEXT NOT NULL,
            isCompleted BOOLEAN NOT NULL,
            customerID INTEGER NOT NULL,
            FOREIGN KEY (customerID) REFERENCES Customer(ID) ON UPDATE CASCADE
        );

        CREATE TABLE IF NOT EXISTS OrderDetail (
            productID INTEGER NOT NULL,
            orderID INTEGER NOT NULL,
            quantity INTEGER NOT NULL,
            PRIMARY KEY (productID, orderID),
            FOREIGN KEY (productID) REFERENCES Product(ID) ON UPDATE CASCADE,
            FOREIGN KEY (orderID) REFERENCES CustomerOrder(ID) ON UPDATE CASCADE
        );

        CREATE TABLE IF NOT EXISTS Invoice (
            ID INTEGER PRIMARY KEY,
            total_price REAL NOT NULL,
            total_profit REAL NOT NULL,
            discount REAL DEFAULT 0,
            isPaid BOOLEAN NOT NULL,
            date TEXT NOT NULL,
            orderID INTEGER UNIQUE NOT NULL,
            FOREIGN KEY (orderID) REFERENCES CustomerOrder(ID) ON UPDATE CASCADE
        );";

            cmd.ExecuteNonQuery();
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
                                IsTracked = Convert.ToBoolean(reader["isTracked"])
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
        //order Related Queries
        //================================


        public static bool DeleteOrder(int orderId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {

                        // 1. Get order details before deletion
                        var orderDetails = new List<(int ProductID, int Quantity)>();
                        string selectDetailsQuery = "SELECT productID, quantity FROM OrderDetail WHERE orderID = @orderID";
                        using (var selectCmd = new SQLiteCommand(selectDetailsQuery, connection, transaction))
                        {
                            selectCmd.Parameters.AddWithValue("@orderID", orderId);
                            using (var reader = selectCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int productId = Convert.ToInt32(reader["productID"]);
                                    int quantity = Convert.ToInt32(reader["quantity"]);
                                    orderDetails.Add((productId, quantity));
                                }
                            }
                        }


                        // 2. Revert stock for each product
                        foreach (var detail in orderDetails)
                        {
                            string updateStockQuery = "UPDATE Product SET inventory_Stock = inventory_Stock + @quantity WHERE ID = @productID";
                            using (var updateCmd = new SQLiteCommand(updateStockQuery, connection, transaction))
                            {
                                updateCmd.Parameters.AddWithValue("@quantity", detail.Quantity);
                                updateCmd.Parameters.AddWithValue("@productID", detail.ProductID);
                                updateCmd.ExecuteNonQuery();
                            }
                        }


                        // 3. Delete OrderDetails
                        string deleteDetailsQuery = "DELETE FROM OrderDetail WHERE orderID = @orderID";
                        using (var cmd = new SQLiteCommand(deleteDetailsQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@orderID", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Delete Invoice (if exists)
                        string deleteInvoiceQuery = "DELETE FROM Invoice WHERE orderID = @orderID";
                        using (var cmd = new SQLiteCommand(deleteInvoiceQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@orderID", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        // 3. Delete Order
                        string deleteOrderQuery = "DELETE FROM CustomerOrder WHERE ID = @orderID";
                        using (var cmd = new SQLiteCommand(deleteOrderQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@orderID", orderId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DB ERROR (DeleteOrder): " + ex.Message);
                return false;
            }
        }


        public static List<CustomerOrder> GetAllOrders()
        {
            var orders = new List<CustomerOrder>();

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    string orderQuery = @"SELECT o.ID, o.Date, o.isCompleted, o.customerID, 
                                         c.ID as CustomerID, c.Name, c.address, c.area,c.phone_number 
                                  FROM CustomerOrder o 
                                  JOIN Customer c ON o.customerID = c.ID";

                    using (var cmd = new SQLiteCommand(orderQuery, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new CustomerOrder
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Date = reader["Date"].ToString(),
                                IsCompleted = Convert.ToBoolean(reader["isCompleted"]),
                                Customer = new Customer
                                {
                                    ID = Convert.ToInt32(reader["CustomerID"]),
                                    Name = reader["Name"].ToString(),
                                    PhoneNumber = reader["phone_number"].ToString(),
                                    Area = reader["area"].ToString(),
                                    Address = reader["address"].ToString()
                                },
                                ProductDetails = new ObservableCollection<OrderDetail>()
                            };

                            // Load OrderDetails with Product
                            string detailQuery = @"SELECT od.ProductID, od.Quantity,
                                                  p.ID, p.Name, p.purchase_price, p.selling_price, p.inventory_Stock, p.threshold_Level, p.isTracked
                                           FROM OrderDetail od
                                           JOIN Product p ON od.ProductID = p.ID
                                           WHERE od.OrderID = @OrderID";

                            using (var detailCmd = new SQLiteCommand(detailQuery, connection))
                            {
                                detailCmd.Parameters.AddWithValue("@OrderID", order.ID);
                                using (var detailReader = detailCmd.ExecuteReader())
                                {
                                    while (detailReader.Read())
                                    {
                                        var product = new Product
                                        {
                                            ID = Convert.ToInt32(detailReader["ID"]),
                                            Name = detailReader["Name"].ToString(),
                                            PurchasePrice = Convert.ToDouble(detailReader["purchase_price"]),
                                            SellingPrice = Convert.ToDouble(detailReader["selling_price"]),
                                            StockQuantity = Convert.ToInt32(detailReader["inventory_Stock"]),
                                            ThresholdLevel = Convert.ToInt32(detailReader["threshold_Level"]),
                                            IsTracked = Convert.ToBoolean(detailReader["isTracked"])
                                        };

                                        var orderDetail = new OrderDetail
                                        {
                                            ProductID = product.ID,
                                            Product = product,
                                            Quantity = Convert.ToInt32(detailReader["quantity"])
                                        };

                                        order.ProductDetails.Add(orderDetail);
                                    }
                                }
                            }

                            orders.Add(order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DB ERROR (GetAllOrders): " + ex.Message);
            }

            return orders;
        }

        public static long InsertOrder(Customer customer, ObservableCollection<OrderDetail> orderDetails)
        {
            using var con = GetConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            var orderDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            // Insert into CustomerOrder
            var insertOrderCmd = new SQLiteCommand("INSERT INTO CustomerOrder (Date, CustomerID, isCompleted) VALUES (@Date, @CustomerID, 0);", con);
            insertOrderCmd.Parameters.AddWithValue("@Date", orderDate);
            insertOrderCmd.Parameters.AddWithValue("@CustomerID", customer.ID);
            insertOrderCmd.ExecuteNonQuery();

            long orderId = con.LastInsertRowId;

            // Insert each OrderDetail and update stock
            foreach (var detail in orderDetails)
            {
                var insertDetailCmd = new SQLiteCommand("INSERT INTO OrderDetail (OrderID, ProductID, Quantity) VALUES (@OrderID, @ProductID, @Quantity);", con);
                insertDetailCmd.Parameters.AddWithValue("@OrderID", orderId);
                insertDetailCmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
                insertDetailCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                insertDetailCmd.ExecuteNonQuery();

                // Update Product Stock
                var updateStockCmd = new SQLiteCommand("UPDATE Product SET Inventory_Stock = @Stock WHERE ID = @ID", con);
                updateStockCmd.Parameters.AddWithValue("@Stock", detail.Product.StockQuantity);
                updateStockCmd.Parameters.AddWithValue("@ID", detail.ProductID);
                updateStockCmd.ExecuteNonQuery();
            }

            transaction.Commit();

            return orderId; // So it can be used later for inserting invoice
        }

        public static void InsertInvoice(long orderId, double totalAmount, double totalProfit)
        {
            using var con = GetConnection();
            con.Open();

            var date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            var insertInvoiceCmd = new SQLiteCommand("INSERT INTO Invoice (OrderID, total_price, total_profit, discount, isPaid, date) VALUES (@OrderID, @Total, @Profit, 0, 0, @Date);", con);
            insertInvoiceCmd.Parameters.AddWithValue("@OrderID", orderId);
            insertInvoiceCmd.Parameters.AddWithValue("@Total", totalAmount);
            insertInvoiceCmd.Parameters.AddWithValue("@Profit", totalProfit);
            insertInvoiceCmd.Parameters.AddWithValue("@Date", date);
            insertInvoiceCmd.ExecuteNonQuery();
        }


        public static void UpdateOrder(long orderId, Customer customer, ObservableCollection<OrderDetail> updatedDetails, double totalAmount, double totalProfit)
        {
            using var con = GetConnection();
            con.Open();
            using var transaction = con.BeginTransaction();

            // 1. Update CustomerOrder table
            var updateOrderCmd = new SQLiteCommand("UPDATE CustomerOrder SET CustomerID = @CustomerID WHERE ID = @ID", con);
            updateOrderCmd.Parameters.AddWithValue("@CustomerID", customer.ID);
            updateOrderCmd.Parameters.AddWithValue("@ID", orderId);
            updateOrderCmd.ExecuteNonQuery();

            // 2. Restore product stock from previous details
            var oldDetails = GetOrderDetails(orderId);
            foreach (var detail in oldDetails)
            {
                var product = detail.Product;
                product.StockQuantity += detail.Quantity;

                var restoreStockCmd = new SQLiteCommand("UPDATE Product SET Inventory_Stock = @Stock WHERE ID = @ID", con);
                restoreStockCmd.Parameters.AddWithValue("@Stock", product.StockQuantity);
                restoreStockCmd.Parameters.AddWithValue("@ID", product.ID);
                restoreStockCmd.ExecuteNonQuery();
            }

            // 3. Delete old order details
            var deleteCmd = new SQLiteCommand("DELETE FROM OrderDetail WHERE OrderID = @OrderID", con);
            deleteCmd.Parameters.AddWithValue("@OrderID", orderId);
            deleteCmd.ExecuteNonQuery();

            // 4. Insert new order details and update stock
            foreach (var detail in updatedDetails)
            {
                var insertDetailCmd = new SQLiteCommand("INSERT INTO OrderDetail (OrderID, ProductID, Quantity) VALUES (@OrderID, @ProductID, @Quantity)", con);
                insertDetailCmd.Parameters.AddWithValue("@OrderID", orderId);
                insertDetailCmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
                insertDetailCmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                insertDetailCmd.ExecuteNonQuery();

                detail.Product.StockQuantity -= detail.Quantity;

                var updateStockCmd = new SQLiteCommand("UPDATE Product SET Inventory_Stock = @Stock WHERE ID = @ID", con);
                updateStockCmd.Parameters.AddWithValue("@Stock", detail.Product.StockQuantity);
                updateStockCmd.Parameters.AddWithValue("@ID", detail.ProductID);
                updateStockCmd.ExecuteNonQuery();
            }

            // 5. Update Invoice
            var updateInvoiceCmd = new SQLiteCommand("UPDATE Invoice SET total_price = @Total, total_profit = @Profit WHERE OrderID = @OrderID", con);
            updateInvoiceCmd.Parameters.AddWithValue("@Total", totalAmount);
            updateInvoiceCmd.Parameters.AddWithValue("@Profit", totalProfit);
            updateInvoiceCmd.Parameters.AddWithValue("@OrderID", orderId);
            updateInvoiceCmd.ExecuteNonQuery();

            transaction.Commit();
        }


        public static List<OrderDetail> GetOrderDetails(long orderId)
        {
            var details = new List<OrderDetail>();

            using var con = GetConnection();
            con.Open();

            var cmd = new SQLiteCommand("SELECT * FROM OrderDetail WHERE OrderID = @OrderID", con);
            cmd.Parameters.AddWithValue("@OrderID", orderId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int productId = Convert.ToInt32(reader["ProductID"]);
                int qty = Convert.ToInt32(reader["Quantity"]);
                var product = GetProductByID(productId);
                details.Add(new OrderDetail(qty, product));
            }

            return details;
        }
        public static Product GetProductByID(int id)
        {
            using var con = GetConnection();
            con.Open();

            var cmd = new SQLiteCommand("SELECT * FROM Product WHERE ID = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Product
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Name = reader["Name"].ToString(),
                    PurchasePrice = double.Parse(reader["purchase_price"].ToString()),
                    SellingPrice = double.Parse(reader["selling_price"].ToString()),
                    ThresholdLevel = int.Parse(reader["threshold_Level"].ToString()),
                    StockQuantity = int.Parse(reader["inventory_Stock"].ToString()),
                    IsTracked = Convert.ToBoolean(reader["isTracked"])
                };
            }

            return null; // not found
        }



        //===============================
        //Invoice Related Queries
        //================================
        public static List<Invoice> GetAllInvoices()
        {
            var invoices = new List<Invoice>();

            using var con = GetConnection();
            con.Open();
            var cmd = new SQLiteCommand("SELECT * FROM Invoice", con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                invoices.Add(new Invoice
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    OrderID = Convert.ToInt32(reader["OrderID"]),
                    TotalPrice = Convert.ToDouble(reader["total_price"]),
                    TotalProfit = Convert.ToDouble(reader["total_profit"]),
                    Discount = Convert.ToDouble(reader["discount"]),
                    IsPaid = Convert.ToInt32(reader["isPaid"]) == 1,
                    Date = reader["date"].ToString()
                });
            }

            return invoices;
        }

    }
}

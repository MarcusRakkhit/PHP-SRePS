using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;

namespace PHP_SRePS
{
    internal class Facade
    {
        public NpgsqlConnection DbConnection(NpgsqlConnection connection)
        {
            string host = "bud5tqxpcjrxerlzivbs-postgresql.services.clever-cloud.com";
            string userName = "uke4h2td0yt4wxax0fty";
            string password = "8PherTANQtquzJktM4pF";
            string database = "bud5tqxpcjrxerlzivbs";
            string connectionStatement = $"Host={host};Username={userName};Password={password};Database={database}";
            connection = new NpgsqlConnection(connectionStatement);
            connection.Open();
            return connection;
        }

        public bool AddProduct(EntityProduct tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"INSERT INTO product(productname, category, price, quantity) VALUES('{tableKey.ProductName}', '{tableKey.Category}', '{tableKey.Price}','{tableKey.Quantity}')"
            };
            int check = command.ExecuteNonQuery();
            command.Connection.Close();
            return check != 0;
        }

        public bool RemoveProduct(string productName)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"DELETE FROM product WHERE productname = '{productName}'"
            };
            try
            {
                int check = command.ExecuteNonQuery();
                command.Connection.Close();
                return check != 0;
            }
            catch
            {
                MessageBox.Show("You cannot delete items that already contain orders", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
        }

        public void AddOrder(EntityOrder tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = "INSERT INTO orders(orderid, orderdate) " +
                $"VALUES('{tableKey.Order}', '{tableKey.Date}')"
            };
            command.Connection.Close();
        }

        public bool AddProductOrders(EntityProductOrders tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = "INSERT INTO product_orders(productid, orderid, " +
                    $"quantity, orderdate) VALUES('{tableKey.ProductID}', '{tableKey.OrderID}'," +
                    $"'{tableKey.Quantity}', '{tableKey.Date}')"
            };
            int check = command.ExecuteNonQuery();
            command.Connection.Close();
            return check != 0;
        }

        public void AddUserAccount(EntityUserAccount tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = "INSERT INTO useraccount(userid, password, " +
                    $"accounttype) VALUES('{tableKey.UserID}', " +
                    $"'{tableKey.Password}', '{tableKey.AccountType}')"
            };
            command.Connection.Close();

        }

        public EntityProduct SelectProduct(string searchID)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"SELECT * FROM product WHERE productid = {searchID}"
            };
            command.ExecuteNonQuery();
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                //string productID = reader[0].ToString();
                string productName = reader[0].ToString();
                string category = reader[1].ToString();
                string price = reader[2].ToString();
                string quantity = reader[3].ToString();

                EntityProduct getProduct = new EntityProduct(productName,
                category, price, quantity);
                command.Connection.Close();
                return getProduct;
            }
            command.Connection.Close();
            return null;
        }

        public EntityOrder SelectOrders(string searchID)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"SELECT * FROM orders WHERE orderid = {searchID}"
            };
            command.ExecuteNonQuery();
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string orderID = reader[0].ToString();
                string orderDate = reader[1].ToString();

                EntityOrder getOrder = new EntityOrder(orderID, orderDate);
                command.Connection.Close();
                return getOrder;
            }
            command.Connection.Close();
            return null;
        }

        public EntityProductOrders SelectProductOrders(string searchPID, string searchOID)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"SELECT * FROM product_orders WHERE productid = {searchPID} AND orderid = {searchOID}"
            };
            command.ExecuteNonQuery();
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string orderID = reader[0].ToString();
                string productID = reader[1].ToString();
                string quantity = reader[2].ToString();
                string orderDate = reader[3].ToString();

                EntityProductOrders getProductOrder = new EntityProductOrders(orderID, productID, quantity, orderDate);
                command.Connection.Close();
                return getProductOrder;
            }
            command.Connection.Close();
            return null;
        }

        public EntityUserAccount SelectUseraccount(string searchID)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"SELECT * FROM useraccount WHERE userid = {searchID}"
            };
            command.ExecuteNonQuery();
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string userid = reader[0].ToString();
                string password = reader[1].ToString();
                string accounttype = reader[2].ToString();

                EntityUserAccount getUser = new EntityUserAccount(userid, password, accounttype);
                command.Connection.Close();
                return getUser;
            }
            command.Connection.Close();
            return null;
        }

        public bool UpdateProduct(EntityProduct tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"update product SET category = '{tableKey.Category}', price = {tableKey.Price}, quantity = {tableKey.Quantity} WHERE productname = '{tableKey.ProductName}'"
            };
            int check = command.ExecuteNonQuery();
            command.Connection.Close();
            return check != 0;
        }

        public bool UpdateOrder(EntityOrder tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"update orders SET orderdate = '{tableKey.Date}' " +
                    $"WHERE orderid = {tableKey.Order}"
            };
            int check = command.ExecuteNonQuery();
            command.Connection.Close();
            return check != 0;
        }

        public bool UpdateProductOrders(EntityProductOrders tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"update product_orders SET quantity = '{tableKey.Quantity}'" +
                    $" WHERE productid = {tableKey.ProductID} AND orderid = {tableKey.OrderID}"
            };
            int check = command.ExecuteNonQuery();
            command.Connection.Close();
            return check != 0;
        }

        public bool UpdateUserAccount(EntityUserAccount tableKey)
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                CommandText = $"update useraccount SET password = '{tableKey.Password}', " +
                    $"accounttype = '{tableKey.AccountType}' WHERE userid = {tableKey.UserID}"
            };
            int check = command.ExecuteNonQuery();
            command.Connection.Close();
            return check != 0;
        }

        public ArrayList WeeklyTimePeriod()
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                //command.CommandText = $"SELECT * FROM product_orders";
                CommandText = $"SELECT * FROM product_orders WHERE orderdate > current_date - interval '7 days'"
            };
            NpgsqlCommand command = npgsqlCommand;
            command.ExecuteNonQuery();
            NpgsqlDataReader reader = command.ExecuteReader();

            ArrayList weeklyList = new ArrayList();

            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    string productID = reader[0].ToString();
                    string orderID = reader[1].ToString();
                    string orderQuantity = reader[2].ToString();
                    string orderDate = reader[3].ToString();

                    EntityProductOrders test = new EntityProductOrders(productID, orderID, orderQuantity, orderDate);
                    weeklyList.Add(test);
                }
                reader.NextResult();
            }

            command.Connection.Close();
            return weeklyList.Count != 0 ? weeklyList : null;
        }
        
        public ArrayList MonthlyTimePeriod()
        {
            NpgsqlConnection connection = null;
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand
            {
                Connection = DbConnection(connection),
                //command.CommandText = $"SELECT * FROM product_orders";
                CommandText = $"SELECT * FROM product_orders WHERE orderDate > current_date - interval '1' month"
            };
            NpgsqlCommand command = npgsqlCommand;
            command.ExecuteNonQuery();
            NpgsqlDataReader reader = command.ExecuteReader();

            ArrayList weeklyList = new ArrayList();

            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    string productID = reader[0].ToString();
                    string orderID = reader[1].ToString();
                    string orderQuantity = reader[2].ToString();
                    string orderDate = reader[3].ToString();

                    EntityProductOrders test = new EntityProductOrders(productID, orderID, orderQuantity, orderDate);
                    weeklyList.Add(test);
                }
                reader.NextResult();
            }

            command.Connection.Close();
            return weeklyList.Count != 0 ? weeklyList : null;
        }
    }
}

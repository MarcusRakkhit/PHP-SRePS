using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Npgsql;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PHP_SRePS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string userPrivileges;
        public MainWindow(string user, string userType)
        {
            InitializeComponent();
            LowStockWarning();
            userName.Content = $"USER: {user}";
            userPrivileges = userType;
        }

        private void DisplaySales(object sender, RoutedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (dropDown.Text == "Weekly")
                {
                    DisplaySales displaySales = new DisplaySales("Weekly");
                    displaySales.Show();
                }
                else if (dropDown.Text == "Monthly")
                {
                    DisplaySales displaySales = new DisplaySales("Monthly");
                    displaySales.Show();
                }
                Mouse.OverrideCursor = null;
            }
            else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PredictSales(object sender, RoutedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                if (dropDown.Text == "Weekly")
                {
                    PredictSales predictSales = new PredictSales("Weekly");
                    predictSales.Show();
                }
                else if (dropDown.Text == "Monthly")
                {
                    PredictSales predictSales = new PredictSales("Monthly");
                    predictSales.Show();
                }
                Mouse.OverrideCursor = null;
            }
            else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateCSV(object sender, RoutedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                try
                {
                    // Check Desktop for "Sales.csv" file
                    Mouse.OverrideCursor = Cursors.Wait;
                    Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    using (StreamWriter streamVar = File.CreateText("Sales.csv"))
                    {
                        Facade facade = new Facade();
                        EntityProduct getProduct;

                        ArrayList selectList = dropDown.Text == "Weekly" ? facade.WeeklyTimePeriod() : dropDown.Text == "Monthly" ? facade.MonthlyTimePeriod() : null;
                        if (selectList != null)
                        {
                            ArrayList OutputList = new ArrayList();
                            string outcome = "";
                            int Total = 0;
                            string csvWriter = string.Format("{0},{1},{2},{3},{4},{5}", "Order ID", "Product ID", "Quantity", "Date", "daily Total", "Total");
                            streamVar.WriteLine(csvWriter);
                            foreach (EntityProductOrders order in selectList)
                            {
                                getProduct = facade.SelectProduct(order.ProductID);
                                if (getProduct != null)
                                {
                                    int price = int.Parse(getProduct.Price);
                                    int quantity = int.Parse(order.Quantity);
                                    int dailyTotal = price * quantity;

                                    string getRecords = $"Retrieving: { order.OrderID}, " +
                                     $"{order.ProductID}, {order.Quantity}, {order.Date}, PRICE: ${dailyTotal}";
                                    OutputList.Add(getRecords);

                                    Total += dailyTotal;

                                    string col1 = order.OrderID;
                                    string col2 = order.ProductID;
                                    string col3 = order.Quantity;
                                    string col4 = order.Date;
                                    int col5 = dailyTotal;
                                    int col6 = Total;
                                    csvWriter = string.Format("{0},{1},{2},{3},{4},{5}", col1, col2, col3, col4, col5, col6);
                                    streamVar.WriteLine(csvWriter);
                                }
                                else
                                {
                                    MessageBox.Show("No such product exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                            double prediction = Total * 1.10;

                            csvWriter = string.Format("{0},{1}", "", "");
                            streamVar.WriteLine(csvWriter);
                            csvWriter = string.Format("{0},{1}", "Predicted TOTAL", prediction);
                            streamVar.WriteLine(csvWriter);

                            foreach (string listItem in OutputList)
                            {
                                outcome = outcome == "" ? listItem : outcome + "\n" + listItem;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Unable to receive product orders from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                    }
                    Mouse.OverrideCursor = null;
                    if (MessageBox.Show("Generated CSV File. Do you wish to open file?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        Process.Start("Sales.csv");
                    }
                }
                catch
                {
                    MessageBox.Show("Sorry, the database isn't avaliable at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalesPeriod(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddStock(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userPrivileges == "ADMIN")
                {
                    if (InternetAvailability.IsInternetAvailable())
                    {
                        if (CheckForValidStockInput())
                        {
                            Facade facade = new Facade();
                            EntityProduct product = new EntityProduct(item.Text, category.Text, price.Text, quantity.Text);
                            item.Text = string.Empty;
                            category.Text = string.Empty;
                            price.Text = string.Empty;
                            quantity.Text = string.Empty;
                            facade.AddProduct(product);
                            // Implement database interactions to add stock here
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                else
                {
                    MessageBox.Show("Sorry, you're not authorized to access this function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch
            {
                MessageBox.Show("Sorry, the database isn't avaliable at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void RemoveStock(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userPrivileges == "ADMIN")
                {
                    if (InternetAvailability.IsInternetAvailable())
                    {
                        if (!IsInputEmpty(item.Text))
                        {
                            Facade facade = new Facade();
                            facade.RemoveProduct(item.Text);
                        }
                        else
                        {
                            MessageBox.Show("Please enter the name of the product you wish to delete", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, you're not authorized to access this function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Sorry, the database isn't avaliable at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }

        private void UpdateStock(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userPrivileges == "ADMIN")
                {
                    if (InternetAvailability.IsInternetAvailable())
                    {
                        if (CheckForValidStockInput())
                        {
                            // Implement database interactions to update stock here
                            Facade facade = new Facade();
                            EntityProduct product = new EntityProduct(item.Text, category.Text, price.Text, quantity.Text);
                            item.Text = string.Empty;
                            category.Text = string.Empty;
                            price.Text = string.Empty;
                            quantity.Text = string.Empty;
                            facade.UpdateProduct(product);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, you're not authorized to access this function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Sorry, the database isn't avaliable at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void ShowStock(object sender, RoutedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                ShowStock showStock = new ShowStock();
                showStock.Show();
                Mouse.OverrideCursor = null;
            } else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InventoryItem(object sender, TextChangedEventArgs e)
        {

        }

        private void InventoryQuantity(object sender, TextChangedEventArgs e)
        {

        }
        private void InventoryCategory(object sender, TextChangedEventArgs e)
        {

        }

        private void InventoryPrice(object sender, TextChangedEventArgs e)
        {

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            
            foreach (Window close in Application.Current.Windows)
            {
                close.Hide();  
            }
            Login login = new Login();
            login.Show();

        }

        private bool IsInputEmpty(string msg)
        {
            return msg == "";
        }

        private bool CheckForValidStockInput()
        {
            if (IsInputEmpty(item.Text))
            {
                MessageBox.Show("Item name required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (IsInputEmpty(quantity.Text))
            {
                MessageBox.Show("Item quantity required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.Match(quantity.Text, "^[1-9][0-9]*$").Success)
            {
                MessageBox.Show("Quantity must be a integer greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (IsInputEmpty(category.Text))
            {
                MessageBox.Show("Item category required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (IsInputEmpty(price.Text))
            {
                MessageBox.Show("Item price required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            // TODO: Make regex accept decimals
            else if (!Regex.Match(price.Text, "^[1-9][0-9]*$").Success)
            {
                MessageBox.Show("Price must be a integer greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void LowStockWarning()
        {

            if (InternetAvailability.IsInternetAvailable())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                Login userN = Application.Current.Windows.OfType<Login>().FirstOrDefault(); // used to access the Username read only property in Login.xaml.cs

                NpgsqlConnection conn = new NpgsqlConnection("Server=bud5tqxpcjrxerlzivbs-postgresql.services.clever-cloud.com;Port=5432;Database=bud5tqxpcjrxerlzivbs;User Id=uke4h2td0yt4wxax0fty;Password=8PherTANQtquzJktM4pF");
                NpgsqlCommand comm = new NpgsqlCommand("select * from product where quantity < 15", conn); // we can use what eamon recommended instead of just '< 15'
                conn.Open();

                NpgsqlDataReader reader;
                reader = comm.ExecuteReader();

                StringBuilder productNames = new StringBuilder();

                int x = 0;
                while (reader.Read())
                {
                    productNames.Append(reader["productname"].ToString() + " - (" + reader["quantity"].ToString() + ") " + Environment.NewLine);
                    x++;
                }

                conn.Close();

                if (x == 0) // check messageboxes
                {
                    MessageBox.Show("Welcome " + userN.Username, "Login Success", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("Detected low stock quantity for these items:\n" + productNames, $"Welcome {userN.Username}", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                Mouse.OverrideCursor = null;
            }
            else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Closing the MainWindow should close the application
            Application.Current.Shutdown();
        }

        private void AddSale(object sender, RoutedEventArgs e)
        {
            if (userPrivileges == "ADMIN")
            {
                NewSale saleWindow = new NewSale();
                saleWindow.Show();
            }
            else
            {
                MessageBox.Show("Sorry, you're not authorized to access this function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}

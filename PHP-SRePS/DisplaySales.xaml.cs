using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PHP_SRePS
{
    /// <summary>
    /// Interaction logic for displaySales.xaml
    /// </summary>
    public partial class DisplaySales : Window
    {
        private readonly string time = "";
        public DisplaySales(string timePeriod)
        {
            time = timePeriod;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                Facade facade = new Facade();
                EntityProduct getProduct;
                try
                {
                    ArrayList selectList = time == "Weekly" ? facade.WeeklyTimePeriod() : time == "Monthly" ? facade.MonthlyTimePeriod() : null;
                    if (selectList != null)
                    {
                        ArrayList OutputList = new ArrayList();
                        string outcome = "";
                        int Total = 0;
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
                            }
                            else
                            {
                                MessageBox.Show("No such product exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        foreach (string listItem in OutputList)
                        {
                            outcome = outcome == "" ? listItem : outcome + "\n" + listItem;

                        }

                        salesItems.Text = time == "Weekly"
                            ? "Records in the past week:\n" + outcome + "\nWeekly Total: $" + Total
                            : time == "Monthly"
                                ? "Records in the past month:\n" + outcome + "\nMonthly Total: $" + Total
                                : "Sorry, unable to receive product orders from the database";

                    }
                    else
                    {
                        MessageBox.Show("Unable to receive product orders from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("Sorry, the database isn't avaliable at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

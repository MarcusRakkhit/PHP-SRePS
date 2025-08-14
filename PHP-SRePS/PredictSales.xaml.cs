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
    /// Interaction logic for predictSales.xaml
    /// </summary>
    public partial class PredictSales : Window
    {
        private readonly string time = "";
        public PredictSales(string timePeriod)
        {
            time = timePeriod;
            InitializeComponent();
        }

        private void PredictItems_TextChanged(object sender, TextChangedEventArgs e)
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
                        int Total = 0;
                        foreach (EntityProductOrders order in selectList)
                        {
                            getProduct = facade.SelectProduct(order.ProductID);
                            if (getProduct != null)
                            {
                                int price = int.Parse(getProduct.Price);
                                int quantity = int.Parse(order.Quantity);
                                int dailyTotal = price * quantity;

                                Total += dailyTotal;
                            }
                            else
                            {
                                MessageBox.Show("No such product exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                        double prediction = Total * 1.10;

                        predictItems.Text = time == "Weekly"
                            ? "Last weeks total:\n$" + Total + "\nNext Weeks total: $" + prediction
                            : time == "Monthly"
                                ? "Last Months total:\n$" + Total + "\nNext Months Total: $" + prediction
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

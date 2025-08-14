using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace PHP_SRePS
{
    /// <summary>
    /// Interaction logic for predictSales.xaml
    /// </summary>
    public partial class PredictCategory : Window
    {
        private readonly string time = "";
        private readonly string category = "";
        public PredictCategory(string timePeriod, string productCategory)
        {
            time = timePeriod;
            category = productCategory;
            InitializeComponent();
        }

        private void PredictCategory_TextChanged(object sender, TextChangedEventArgs e)
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
                            getProduct = facade.SelectProductCategory(order.ProductID, category);
                            if (getProduct != null)
                            {
                                int price = int.Parse(getProduct.Price);
                                int quantity = int.Parse(order.Quantity);
                                int dailyTotal = price * quantity;

                                Total += dailyTotal;
                            }
                        }

                        double prediction = Total * 1.10;

                        predictCategory.Text = time == "Weekly"
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
            }
            else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

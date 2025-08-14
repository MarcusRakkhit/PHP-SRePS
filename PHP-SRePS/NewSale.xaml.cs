using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace PHP_SRePS
{
    /// <summary>
    /// Interaction logic for NewSale.xaml
    /// </summary>
    public partial class NewSale : Window
    {
        public NewSale()
        {
            InitializeComponent();
        }

        private void AddSale(object sender, RoutedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                if (CheckForValidSaleInput())
                {
                    Facade facade = new Facade();
                    DateTime currentDate = DateTime.Today;
                    string date = currentDate.ToString();
                    EntityProductOrders productOrder = new EntityProductOrders(productID.Text, orderID.Text, quantity.Text, date);
                    EntityProduct getProduct;

                    try
                    {
                        getProduct = facade.SelectProduct(productID.Text);
                        if (getProduct != null)
                        {
                            if (int.Parse(quantity.Text) > int.Parse(getProduct.Quantity))
                            {
                                MessageBox.Show("Sorry, we're low on stock at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                facade.AddProductOrders(productOrder);
                                MessageBox.Show($"Added {getProduct.ProductName} to the orders list", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                                int subtractQuantity = int.Parse(getProduct.Quantity) - int.Parse(quantity.Text);

                                if (subtractQuantity < 0)
                                {
                                    subtractQuantity = 0;
                                }
                                getProduct.Quantity = subtractQuantity.ToString();
                                facade.UpdateProduct(getProduct);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sorry, the product does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Sorry, the database isn't avaliable at the moment", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            } else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsInputEmpty(string msg)
        {
            return msg == "";
        }

        private bool CheckForValidSaleInput()
        {
            if (IsInputEmpty(productID.Text))
            {
                MessageBox.Show("Product ID required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.Match(productID.Text, "^[1-9][0-9]*$").Success)
            {
                MessageBox.Show("Product ID must be an integer greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (IsInputEmpty(orderID.Text))
            {
                MessageBox.Show("Order ID required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.Match(orderID.Text, "^[1-9][0-9]*$").Success)
            {
                MessageBox.Show("Order ID must be an integer greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (IsInputEmpty(quantity.Text))
            {
                MessageBox.Show("Quantity required", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (!Regex.Match(quantity.Text, "^[1-9][0-9]*$").Success)
            {
                MessageBox.Show("Quantity must be a integer greater than 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

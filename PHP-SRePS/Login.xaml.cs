using System;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                try
                {
                    Facade facade = new Facade();
                    EntityUserAccount userObject;

                    
                    if(user.Text == "")
                    {
                        MessageBox.Show("Please insert your username", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                    else
                    {
                        userObject = facade.SelectUseraccount(user.Text);
                        if (userObject != null)
                        {
                            if (Username == userObject.UserID && passwordBox.Password == userObject.Password)
                            {
                                MainWindow mainWindow = new MainWindow(userObject.UserID, userObject.AccountType);
                                Visibility = Visibility.Hidden;
                                mainWindow.Tag = "currentlogin";
                                mainWindow.Show();
                                passwordBox.Clear();
                                user.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Sorry, your username or password is INCORRECT", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);

                            }

                        }
                        else
                        {
                            MessageBox.Show("Sorry, your username or password is INCORRECT", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                    }
                    
                }
                catch
                {
                    MessageBox.Show("There's no connection or no such username","Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Unable to access the internet. Please check your connection settings", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string Username
        {
            get
            {
                return user.Text;
            }
        }
    }
}

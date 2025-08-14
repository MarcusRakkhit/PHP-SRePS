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
using System.Collections.ObjectModel;
using Npgsql;
using System.Data;

namespace PHP_SRePS
{
    /// <summary>
    /// Interaction logic for ShowStock.xaml
    /// </summary>
    /// 
    public partial class ShowStock : Window
    {
        private readonly DataTable dt = new DataTable("product");
        public ShowStock()
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                InitializeComponent();
                try
                {
                    NpgsqlConnection conn = new NpgsqlConnection("Server=bud5tqxpcjrxerlzivbs-postgresql.services.clever-cloud.com;Port=5432;Database=bud5tqxpcjrxerlzivbs;User Id=uke4h2td0yt4wxax0fty;Password=8PherTANQtquzJktM4pF");
                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand
                    {
                        Connection = conn,
                        CommandType = CommandType.Text,
                        CommandText = "select * from product"
                    };
                    NpgsqlDataReader dr = comm.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dt.Load(dr);
                        dataGrid1.ItemsSource = dt.DefaultView;
                    }
                    comm.Dispose();
                    conn.Close();
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

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) 
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                try
                {
                    DataView search = dt.DefaultView;
                    search.RowFilter = string.Format("productname like '%{0}%'", SearchBox.Text);
                    dataGrid1.ItemsSource = search;
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

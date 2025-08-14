using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHP_SRePS
{
    internal class EntityProductOrders
    {
        public EntityProductOrders(string product, string order, string number, string date)
        {
            ProductID = product;
            OrderID = order;
            Quantity = number;
            Date = date;
        }

        public string ProductID { get; set; }

        public string OrderID { get; set; }

        public string Quantity { get; set; }

        public string Date { get; set; }
    }
}

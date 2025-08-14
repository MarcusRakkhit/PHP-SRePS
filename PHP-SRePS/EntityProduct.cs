using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHP_SRePS
{
    internal class EntityProduct
    {
        public EntityProduct(string name,
            string type, string cost, string number)
        {        
            ProductName = name;
            Category = type;
            Price = cost;
            Quantity = number;
        }

      //  public string ProductID { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }

        public string Price { get; set; }

        public string Quantity { get; set; }
    }
}
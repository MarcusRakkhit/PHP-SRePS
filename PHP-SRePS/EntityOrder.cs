using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHP_SRePS
{
    internal class EntityOrder
    {
        public EntityOrder(string order, string date)
        {
            Order = order;
            Date = date;
        }

        public string Order { get; set; }

        public string Date { get; set; }
    }
}
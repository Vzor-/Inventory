using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{

    public class OrderPart
    {
        public string OrderID { get; set; }
        public string PartID { get; set; }
        public int Count { get; set; }

        public OrderPart(string orderID, string partID, int count)
        {
            this.OrderID = orderID;
            this.PartID = partID;
            this.Count = count;
        }

    }
}

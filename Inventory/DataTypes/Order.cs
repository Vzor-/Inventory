using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Order
    {
        public string orderID { get; }
        public bool Approved { get; }
        public bool Ordered { get; }
        public bool Recived { get; }
        public string orderedDate { get; }
        public string expectedDate { get; }

        public Order(string orderID, bool approved, bool ordered, bool recived, string orderedDate, string expectedDate)
        {
            this.orderID = orderID;
            this.Approved = approved;
            this.Ordered = ordered;
            this.Recived = recived;
            this.orderedDate = orderedDate;
            this.expectedDate = expectedDate;
        }
    }
}

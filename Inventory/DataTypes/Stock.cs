using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Stock
    {

        public string PartID { get; set; }
        public int Count { get; set; }
        public int Location { get; set; }

        public Stock(string PartID, int Count, int Location)
        {
            this.PartID = PartID;
            this.Count = Count;
            this.Location = Location;
        }

        public Stock(Stock item)
        {
            this.PartID = item.PartID;
            this.Count = item.Count;
            this.Location = item.Location;
        }
        
    }
}

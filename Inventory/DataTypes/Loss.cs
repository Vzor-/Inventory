using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Loss
    {
        public string PartID { get; }
        public int? InternalId { get; }
        public string Reason { get; }
        public int NumberLost { get; }
        public string Date { get; }

        public Loss(string partID, string reason, int numberLost, string date, int? internalId)
        {
            this.PartID = partID;
            this.InternalId = internalId;
            this.Reason = reason;
            this.NumberLost = numberLost;
            this.Date = date;
        }

        public Loss(string partID, string reason, int numberLost, string date): this(partID, reason, numberLost, date, null) {   }
    }
}

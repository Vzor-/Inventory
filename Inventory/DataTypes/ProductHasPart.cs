﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class ProductHasPart
    {
        public string PartName { get; set; }
        public string PartID { get; set; }
        public int Count { get; set; }

        public ProductHasPart(string PartName, string PartID, int Count)
        {
            this.PartName = PartName;
            this.PartID = PartID;
            this.Count = Count;
        }
    }
}

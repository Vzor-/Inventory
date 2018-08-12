﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class RecipeHasPart
    {
        public string PartName { get; set; }
        public string PartID { get; set; }
        public int Count { get; set; }

        // TODO: fix this as the recipe has part is different then table?
        public RecipeHasPart(string PartName, string PartID, int Count)
        {
            this.PartName = PartName;
            this.PartID = PartID;
            this.Count = Count;
        }
    }
}

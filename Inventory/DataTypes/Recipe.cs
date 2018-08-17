using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Recipe
    {
        public string EnglishName { get; set; }
        public string InternalID { get; set; }
        public bool IsActive { get; set; }

        public Recipe(string EnglishName, string InternalID, bool IsActive)
        {
            this.EnglishName = EnglishName;
            this.InternalID = InternalID;
            this.IsActive = IsActive;
        }

        public override string ToString()
        {
            return this.EnglishName;
        }
    }
}

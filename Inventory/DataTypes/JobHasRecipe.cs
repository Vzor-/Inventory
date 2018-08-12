using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class JobHasRecipe
    {
        public string JobID { get; set; }
        public string RecipeID { get; set; }
        public int Count { get; set; }
        
        public JobHasRecipe(string JobID, string RecipeID, int Count)
        {
            this.JobID = JobID;
            this.RecipeID = RecipeID;
            this.Count = Count;
        }
    }
}

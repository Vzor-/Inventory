using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class JobConsumption
    {
        public int JobID { get; set; }
        public string RecipesID { get; set; }
        public int LocationID { get; set; }
        public int? InternalID { get; set; }
        public string PartsID { get; set; }
        public int PartsCount { get; set; }
        public int RecipesCount { get; set; }
        public int State { get; set; }

        public readonly static int[] validStates = { -2, 7, 4 }; 

        public JobConsumption(int JobID, string RecipesID, int LocationID, int? InternalID, string PartsID, int PartsCount, int RecipesCount, int State)
        {
            this.JobID = JobID;
            this.RecipesID = RecipesID;
            this.LocationID = LocationID;
            this.InternalID = InternalID;
            this.PartsID = PartsID;
            this.PartsCount = PartsCount;
            this.RecipesCount = RecipesCount;

            if (Array.FindAll(validStates, x => x == State).Length == 0) {
                throw new ArgumentException("State not of valid value");
            }
            this.State = State;
        }

        public JobConsumption(int JobID, string RecipesID, int LocationID, string PartsID, int PartsCount, int RecipesCount, int State) : this(JobID, RecipesID, LocationID, null, PartsID, PartsCount, RecipesCount, State)
        {

        }
    }
}

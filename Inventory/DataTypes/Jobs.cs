using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Job
    {
        public int InternalId { get; set; }
        public string EnglishName { get; }
        public StateLookup State { get; }

        public Job(int InternalId, string EnglishName, int State)
        {
            this.InternalId = InternalId;
            this.EnglishName = EnglishName;
            this.State = new StateLookup(State);
        }

        public Job(string EnglishName, int State) : this( -1,  EnglishName,  State) { }
    }
}

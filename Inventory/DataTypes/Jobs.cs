using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    class Job
    {
        public int InternalId { get; set; }
        public string EnglishName { get; }
        public bool Approved { get; }
        public bool Created { get; }

        public Job(int InternalId, string EnglishName, bool Approved, bool Created)
        {
            this.InternalId = InternalId;
            this.EnglishName = EnglishName;
            this.Approved = Approved;
            this.Created = Created;
        }

        public Job(string EnglishName, bool Approved, bool Created): this( -1,  EnglishName,  Approved, Created) { }
    }
}

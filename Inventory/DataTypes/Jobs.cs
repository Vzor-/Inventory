using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Inventory.DataTypes.StateLookup;

namespace Inventory.DataTypes
{
    public class Job
    {
        public int InternalId { get; set; }
        public string EnglishName { get; }
        public StateLookup State { get; }

        public readonly static StateEnum[] validJobStates = { StateEnum.Canceled, StateEnum.Proposed, StateEnum.Approved, StateEnum.Allocated, StateEnum.In_Progress, StateEnum.Created, StateEnum.Stocked };

        public Job(int InternalId, string EnglishName, int State)
        {
            this.InternalId = InternalId;
            this.EnglishName = EnglishName;
            if (isValidJobState(State))
            {
                this.State = new StateLookup(State);
            }
            else
            {
                throw new ArgumentException("State not of valid value");
            }
        }

        public Job(string EnglishName, int State) : this(-1, EnglishName, State) { }

        private bool isValidJobState(int state)
        {
            if (Array.FindAll(validJobStates, x => (int)x == state).Length == 0)
            {
                return false;
            }
            return true;
        }

        public bool canBeCanceled()
        {
            return true;
        }
        
        public bool canBePending()
        {
            return false;    // Pending by default. as of now will not be able to move back to this state
        }

        public bool canBeApproved()
        {
            if (State.stateValue == StateEnum.Proposed) return true;
            return false;
        }

        public bool canBeAllocated()
        {
            if (State.stateValue == StateEnum.Approved) return true;
            return false;
        }

        public bool canBeIn_Progress()
        {
            if (State.stateValue == StateEnum.Allocated) return true;
            return false;
        }

        public bool canBeCreated()
        {
            if ((State.stateValue == StateEnum.Allocated)|| (State.stateValue == StateEnum.In_Progress)) return true;
            return false;
        }

        public bool canBeStocked()
        {
            if (State.stateValue == StateEnum.Created) return true;
            return false;
        }

    }
    
}

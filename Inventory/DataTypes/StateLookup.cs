using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class StateLookup
    {
        public enum StateEnum
        {
            Canceled    = -2,
            UnKnown     = -1,
            Proposed    = 0,
            In_Progress = 1,
            Created     = 2,
            Shipped     = 3,
            Allocated   = 4,
            Approved    = 5,
            Stocked     = 6,
            Pending     = 7
        }

        public StateEnum stateValue;
        String stateName;

        public StateLookup(int value)
        {
            if (StateEnum.IsDefined(typeof(StateEnum), value))
            {
                stateValue = (StateEnum)value;
            }
            else {
                throw new InvalidCastException();
            }
        }

        public string getStateName() {
            return stateValue.ToString();
        }

        public override string ToString()
        {
            return stateValue.ToString();
        }


    }
}

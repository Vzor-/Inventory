using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Inventory.DataTypes.StateLookup;

namespace Inventory.DataTypes
{
    public class JobConsumption
    {
        public int JobID { get; set; }
        public string RecipesID { get;}
        public int LocationID { get; set; }
        public int? InternalID { get;}
        public string PartsID { get; set; }
        public int PartsCount { get; set; }
        public int RecipesCount { get; set; }
        public StateLookup State { get; }
        //public int State { get; set; }
        public int isOutput { get; }

        public readonly static StateEnum[] validStatesInput = { StateEnum.Canceled, StateEnum.Pending, StateEnum.Allocated, StateEnum.Created };

        public readonly static StateEnum[] validStatesOutput = { StateEnum.Canceled, StateEnum.Pending, StateEnum.Created, StateEnum.Stocked };


        public JobConsumption(int JobID, string RecipesID, int LocationID, int? InternalID, string PartsID, int PartsCount, int RecipesCount, int State, int isOutput)
        {
            this.JobID = JobID;
            this.RecipesID = RecipesID;
            this.LocationID = LocationID;
            this.InternalID = InternalID;
            this.PartsID = PartsID;
            this.PartsCount = PartsCount;
            this.RecipesCount = RecipesCount;

            if (isOutput == 0)
            {
                if (isValidStateInput(State))
                {

                }
                else
                {
                    throw new ArgumentException("State not of valid value");
                }
            }
            else if(isOutput == 1){

                if (isValidStateOutput(State))
                {

                }
                else
                {
                    throw new ArgumentException("State not of valid value");
                }
            }

            this.State = new StateLookup(State);
            this.isOutput = isOutput;
        }



        public JobConsumption(int JobID, string RecipesID, int LocationID, string PartsID, int PartsCount, int RecipesCount, int State, int isOutput) : this(JobID, RecipesID, LocationID, null, PartsID, PartsCount, RecipesCount, State, isOutput)
        {

        }


        /// <summary>
        /// Is Input allowed to be this State at all
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool isValidStateInput(int state)
        {
            if (Array.FindAll(validStatesInput, x => (int)x == state).Length == 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Is Output allowed to be this State at all
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private bool isValidStateOutput(int state)
        {
            if (Array.FindAll(validStatesOutput, x => (int)x == state).Length == 0)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// State Machine Check
        /// Input can be set to Canceled
        /// </summary>
        /// <returns></returns>
        public bool iCanBeCanceled()
        {
            return true;
        }


        /// <summary>
        /// State Machine Check
        /// Input can be set to Pending
        /// </summary>
        /// <returns></returns>
        public bool iCanBePending()
        {
            return false;  // Pending by default. as of now will not be able to move back to this state
        }


        /// <summary>
        /// State Machine Check
        /// Input can be set to Allocated
        /// </summary>
        /// <returns></returns>
        public bool iCanBeAllocated()
        {
            if (State.stateValue == StateEnum.Pending) return true;
            return false;
        }


        /// <summary>
        /// State Machine Check
        /// Input can be set to Created
        /// </summary>
        /// <returns></returns>
        public bool icanBeCreated()
        {
            if (State.stateValue == StateEnum.Allocated) return true;
            return false;
        }


        /// <summary>
        /// State Machine Check
        /// Output can be set to Canceled
        /// </summary>
        /// <returns></returns>
        public bool OCanBeCanceled()
        {
            return true;
        }


        /// <summary>
        /// State Machine Check
        /// Output can be set to Pending
        /// </summary>
        /// <returns></returns>
        public bool OCanBePending()
        {
            return false;  // Pending by default. as of now will not be able to move back to this state
        }


        /// <summary>
        /// State Machine Check
        /// Output can be set to Created
        /// </summary>
        /// <returns></returns>
        public bool OcanBeCreated()
        {
            if (State.stateValue == StateEnum.Allocated) return true;
            return false;
        }


        /// <summary>
        /// State Machine Check
        /// Output can be set to Stocked
        /// </summary>
        /// <returns></returns>
        public bool OcanBeStocked()
        {
            if (State.stateValue == StateEnum.Created) return true;
            return false;
        }
    }
}

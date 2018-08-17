using Inventory.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.JobPages
{

    /// <summary>
    /// Calculates if a Job may have a ConsumptionAllocation
    /// Perform ConsumptionAllocation operation
    /// </summary>
    class JobConsumptionAllocationLogic
    {
        List<DataTypes.Stock> allocatedStock = new List<DataTypes.Stock>();
        List<DataTypes.Stock> allStockPristine;
        List<DataTypes.Stock> allStock = new List<DataTypes.Stock>();
        internal List<JobConsumption> jobConsumption;
        public Boolean IsAllocated { get; private set; }
        public Boolean CanAllocate { get; private set; }
        public Location SourceBuilding { get; private set; }
        private Job localJob;

        public JobConsumptionAllocationLogic(Job job)
        {
            localJob = job;
            jobConsumption = new List<JobConsumption>();
            jobConsumption = DatabaseConnection.findJobConsumption(localJob);
            allStockPristine = (List<DataTypes.Stock>)DatabaseConnection.getStock();

            IsAllocated = checkIfAllocated(jobConsumption);
            CanAllocate = checkIfAllocatable(allStockPristine, jobConsumption);
        }

        private bool checkIfAllocatable(List<DataTypes.Stock> allStock, List<JobConsumption> jobConsumption)
        {
            if (jobConsumption.Count == 0) return false;
            // copy all Stock of consern to temp location
            List<DataTypes.Stock> tempStock = new List<DataTypes.Stock>();
            // TODO: allow filter based in Builder
            foreach (var jobConsItem in jobConsumption) {
                if (tempStock.Find(x => x.PartID == jobConsItem.PartsID) == null) { // not already in list
                    tempStock.Add(new DataTypes.Stock(jobConsItem.PartsID, allStock.FindAll(x => x.PartID == jobConsItem.PartsID).Sum(y => y.Count), -2));
                }
            }

            // pre-check
            if (tempStock.Find(x => x.Count < 0) != null) return false;

            // remove count from temp stock for each JobConsumption
            foreach (var jobConsItem in jobConsumption)
            {
                tempStock.Find(x => x.PartID == jobConsItem.PartsID).Count -= jobConsItem.PartsCount;
            }




            if (tempStock.Find(x => x.Count < 0) != null) return false;
            return true;
        }

        public Boolean autoAllocation(){
            if (IsAllocated) return false;
            if (!CanAllocate) return false;
            List<DataTypes.Stock> allocatedStock = new List<DataTypes.Stock>();
            List<DataTypes.Stock> allStock = allStockPristine.ConvertAll(stock => new DataTypes.Stock(stock));
            List<JobConsumption> finalJobConsumption = new List<JobConsumption>();
            List<DataTypes.Stock> stockToUpdate = new List<DataTypes.Stock>();

            allStock.Sort(delegate (DataTypes.Stock x, DataTypes.Stock y)
            {
                return x.Count.CompareTo(y.Count);
            });


            foreach (JobConsumption item in jobConsumption)
            {
                if (item.State == (int)StateLookup.StateEnum.Allocated)
                {
                    
                    finalJobConsumption.Add(new JobConsumption(item.JobID, item.RecipesID, item.LocationID, item.PartsID, item.PartsCount, item.RecipesCount, (int)StateLookup.StateEnum.Allocated));
                    continue;
                }
                var carton = allStock.FindAll(eggs => eggs.PartID == item.PartsID); // filter this part

                foreach (var egg in carton)
                { // allocate until item count is zero

                    if (egg.Count > item.PartsCount)
                    {
                        var amount = item.PartsCount;
                        //take all
                        finalJobConsumption.Add(new JobConsumption(item.JobID, item.RecipesID, egg.Location, item.PartsID, amount, item.RecipesCount, (int)StateLookup.StateEnum.Allocated));
                        
                        egg.Count -= amount;
                        item.PartsCount -= amount;
                        stockToUpdate.Add(egg);
                    }
                    else if (egg.Count <= item.PartsCount)
                    {
                        var amount = egg.Count;

                        //take some
                        finalJobConsumption.Add(new JobConsumption(item.JobID, item.RecipesID, egg.Location, item.PartsID, amount, item.RecipesCount, (int)StateLookup.StateEnum.Allocated));
                        
                        egg.Count -= amount;
                        item.PartsCount -= amount;
                        stockToUpdate.Add(egg);
                    }
                    if (item.PartsCount == 0) break;
                }
            }
            // remove current allocation
            DatabaseConnection.delJobAllocation(jobConsumption);

            // add new allocation
            DatabaseConnection.addJobConsumptionToJob(finalJobConsumption);

            // update Stock table & Losses with allocation changes.
            foreach (var item in finalJobConsumption)
            {
                DatabaseConnection.addStockLoss(new Loss(item.PartsID, "Allocation: " + localJob.EnglishName, item.PartsCount, DateTime.UtcNow.ToString()));

            }
            foreach (var item in stockToUpdate)
            {
                DatabaseConnection.updateStockCount(item);
            }

            //update job State
            DatabaseConnection.updateJob(localJob, new StateLookup((int)StateLookup.StateEnum.Allocated));

            return true;
        }



        public void SetAllocationBuilding(Location loc) {

            List<Location> validBuildings = DatabaseConnection.getLocations(Location.LocationType.LocationBuilding);

            if (validBuildings.FindAll(y => y.ID == loc.ID).Count > 0) {
                allStockPristine = (List<DataTypes.Stock>)DatabaseConnection.getStockInLocationRecursive(loc);
                SourceBuilding = loc;
            }
        }

        private Boolean checkIfAllocated(List<JobConsumption> jobConsumption)
        {
            if (jobConsumption.Count == 0) return false;
            return (jobConsumption.Find(x => x.State == (int)StateLookup.StateEnum.Allocated) != null);
        }
    }
}

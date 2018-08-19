using Inventory.DataTypes;
using Inventory.Popups;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Inventory.JobPages
{
    /// <summary>
    /// Interaction logic for SourcingOverview.xaml
    /// </summary>
    public partial class JobsOverview : UserControl
    {
        public JobsOverview()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception
                jobsDataGrid.ItemsSource = DatabaseConnection.getJobs();
            }
            
        }

        private void OrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void JobsOverview_ViewAllocation_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;
            if (item != null)
            {
                //if (!item.Created && item.Approved)
                ////{
                //    List<JobConsumption> jobConsumption = new List<JobConsumption>();

                //    jobConsumption = DatabaseConnection.findJobConsumption(item);

                JobConsumptionView window = new JobConsumptionView(item);

                window.Show();

                updatePage();
                //}
            }
        }
        

        public void updateJobsOverview()
        {
            updatePage();
        }


        private void JobsOverview_Created_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;
            if (item != null)
            {
                var items = DatabaseConnection.findJobConsumption(item).FindAll(x => x.isOutput == 1);

                if (items.Count > 0)
                {
                    List<DataTypes.Stock> tempStock = new List<DataTypes.Stock>();
                    foreach (var egg in items)
                    {
                        tempStock.Add(new DataTypes.Stock(egg.PartsID, egg.PartsCount, -1));
                    }
                    SourcingStockIntake win = new SourcingStockIntake(tempStock);
                    win.Show();
                    if (win.isAllAllocated)
                    {

                        setJobToStocked(item);
                    }
                    else
                    {
                        setJobToCreated(item);
                    }
                }
                else
                {
                    setJobToCreated(item);
                }
                updatePage();
            }
        }


        private void JobsOverview_Approved_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;
            if (item != null)
            {
                setJobToApproved(item);
                updatePage();
            }
        }


        private void setJobToCreated(Job item)
        {
            DatabaseConnection.updateJob(item, new StateLookup((int)StateLookup.StateEnum.Created));
            DatabaseConnection.updateJobConsumptionInputs(item, new StateLookup((int)StateLookup.StateEnum.Created));
            DatabaseConnection.updateJobConsumptionOutputs(item, new StateLookup((int)StateLookup.StateEnum.Created));
        }


        private void setJobToApproved(Job item)
        {
            DatabaseConnection.updateJob(item, new StateLookup((int)StateLookup.StateEnum.Approved));
        }


        private void setJobToStocked(Job item)
        {
            DatabaseConnection.updateJob(item, new StateLookup((int)StateLookup.StateEnum.Stocked));
            DatabaseConnection.updateJobConsumptionOutputs(item, new StateLookup((int)StateLookup.StateEnum.Stocked));
        }

        private void updatePage()
        {
            jobsDataGrid.ItemsSource = DatabaseConnection.getJobs();
            jobsDataGrid.Items.Refresh();
        }
        

        private void jobsDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;
            MenuItem[] menuitems = new MenuItem[10];
            jobsDataGrid_ContextMenu.Items.CopyTo(menuitems, 0);
            
            if (item != null)
            {
                if (item.canBeAllocated())
                {
                    if (!(new JobConsumptionAllocationLogic(item)).CanAllocate)
                    {
                        menuitems[2].IsEnabled = false;
                    }
                    else
                    {
                        menuitems[2].IsEnabled = true;
                    }
                }
                else
                {
                    menuitems[2].IsEnabled = false;
                }
                menuitems[0].IsEnabled = item.canBeApproved();
                menuitems[1].IsEnabled = item.canBeCreated();
                menuitems[3].IsEnabled = true;


            }
            else
            {
                menuitems[0].IsEnabled = false;
                menuitems[1].IsEnabled = false;
                menuitems[2].IsEnabled = false;
                menuitems[3].IsEnabled = false;
            }
        }

        private void jobsDatagrid_ContextMenu_Click_AutoAllocate(object sender, RoutedEventArgs e)
        {

            BuildingLocationDialog window = new BuildingLocationDialog();

            window.ShowDialog();
            Location loc = (Location)window.selectedLoc;
            if (loc != null)
            {
                JobConsumptionAllocationLogic allocate = new JobConsumptionAllocationLogic((DataTypes.Job)jobsDataGrid.CurrentItem);
                allocate.SetAllocationBuilding(loc);

                if (allocate.IsAllocated) throw new ArgumentException("Job already allocated in some way at \"jobsDatagrid_ContextMenu_Click_AutoAllocate\"");
                if (!allocate.CanAllocate) throw new ArgumentException("Job can not be allocated in some way at \"jobsDatagrid_ContextMenu_Click_AutoAllocate\"");

                allocate.autoAllocation();

                updatePage();
            }
        }
    }
}

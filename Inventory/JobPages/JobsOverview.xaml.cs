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
            //if (!item.Created && item.Approved)
            ////{
            //    List<JobConsumption> jobConsumption = new List<JobConsumption>();

            //    jobConsumption = DatabaseConnection.findJobConsumption(item);

                JobConsumptionView window = new JobConsumptionView(item);
                
                window.Show();

                updatePage();
            //}
        }

        public void updateJobs()
        {
            updatePage();
        }
        private void JobsOverview_Created_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;

            var items = DatabaseConnection.findJobConsumption(item).FindAll(x => x.isOutput == 1);
            
            if (items.Count > 0)
            {
                List<DataTypes.Stock> tempStock = new List<DataTypes.Stock>();
                foreach (var egg in items)
                {
                    tempStock.Add(new DataTypes.Stock(egg.PartsID, egg.PartsCount, -1));
                }
                Window win = new SourcingStockIntake(tempStock);
                win.Show();

            }
            setJobToCreated(item);
            updatePage();
        }

        private void JobsOverview_Approved_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;

            setJobToApproved(item);
            updatePage();
        }

        private void setJobToCreated(Job item)
        {
            DatabaseConnection.updateJob(item, item.State);
        }

        private void setJobToApproved(Job item)
        {
            DatabaseConnection.updateJob(item, item.State);
        }

        private void updatePage()
        {
            jobsDataGrid.ItemsSource = DatabaseConnection.getJobs();
            jobsDataGrid.Items.Refresh();
        }
        

        private void jobsDataGrid_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)jobsDataGrid.CurrentItem;
            if (!(new JobConsumptionAllocationLogic(item)).CanAllocate)
            {
                MenuItem menuitem = (MenuItem)jobsDataGrid_ContextMenu.Items[2];
                menuitem.IsEnabled = false;
            }
            else
            {
                MenuItem menuitem = (MenuItem)jobsDataGrid_ContextMenu.Items[2];
                menuitem.IsEnabled = true;
            }
        }

        private void jobsDatagrid_ContextMenu_Click_AutoAllocate(object sender, RoutedEventArgs e)
        {

            BuildingLocationDialog window = new BuildingLocationDialog();

            window.ShowDialog();
            Location loc = (Location)window.selectedLoc;
            JobConsumptionAllocationLogic allocate = new JobConsumptionAllocationLogic((DataTypes.Job)jobsDataGrid.CurrentItem);
            allocate.SetAllocationBuilding(loc);

            if (allocate.IsAllocated) throw new ArgumentException("Job already allocated in some way in \"jobsDatagrid_ContextMenu_Click_AutoAllocate\"");
            if (!allocate.CanAllocate) throw new ArgumentException("Job can not be allocated in some way in \"jobsDatagrid_ContextMenu_Click_AutoAllocate\"");

            allocate.autoAllocation();

            updatePage();
        }
    }
}

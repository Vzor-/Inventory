using Inventory.DataTypes;
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

                JobsDataGrid.ItemsSource = DatabaseConnection.getJobs();
            }
            
        }

        private void OrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void JobsOverview_Created_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)JobsDataGrid.CurrentItem;

            setJobToCreated(item);
            updatePage();
        }

        private void JobsOverview_Approved_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Job item = (DataTypes.Job)JobsDataGrid.CurrentItem;

            setJobToApproved(item);
            updatePage();
        }

        private void setJobToCreated(Job item)
        {
            DatabaseConnection.updateJob(item, nameof(item.Created), true);
        }

        private void setJobToApproved(Job item)
        {
            DatabaseConnection.updateJob(item, nameof(item.Approved), true);
        }
        

        //private void intakeOrderStock(Job item)
        //{
        //    List<OrderPart> parts = new List<OrderPart>();

        //    parts = DatabaseConnection.findPartsInOrder(item);

        //    SourcingStockIntake window = new SourcingStockIntake(parts);
        //    window.Show();
            

        //}

        private void updatePage()
        {
            JobsDataGrid.ItemsSource = DatabaseConnection.getJobs();
            JobsDataGrid.Items.Refresh();
        }

        private void JobsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}

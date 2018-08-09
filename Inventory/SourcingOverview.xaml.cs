using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Inventory.DataTypes;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for SourcingOverview.xaml
    /// </summary>
    public partial class SourcingOverview : UserControl
    {
        public SourcingOverview()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception

                OrderDataGrid.ItemsSource = DatabaseConnection.getOrders();
            }
            
        }

        private void OrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SourcingOverview_Ordered_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Order item = (DataTypes.Order)OrderDataGrid.CurrentItem;

            setOrderToOrdered(item);
            updatePage();
        }

        private void SourcingOverview_Approved_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Order item = (DataTypes.Order)OrderDataGrid.CurrentItem;

            setOrderToApproved(item);
            updatePage();
        }

        private void SourcingOverview_Received_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Order item = (DataTypes.Order)OrderDataGrid.CurrentItem;

            setOrderToRecived(item);
            updatePage();
        }

        private void setOrderToOrdered(Order item)
        {
            DatabaseConnection.updateOrder(item, nameof(item.Ordered), true);
        }

        private void setOrderToApproved(Order item)
        {
            DatabaseConnection.updateOrder(item, nameof(item.Approved), true);
        }

        private void setOrderToRecived(Order item)
        {
            DatabaseConnection.updateOrder(item, nameof(item.Recived), true);
            intakeOrderStock(item);
        }

        private void intakeOrderStock(Order item)
        {
            List<OrderPart> parts = new List<OrderPart>();

            parts = DatabaseConnection.findPartsInOrder(item);

            SourcingStockIntake window = new SourcingStockIntake(parts);
            window.Show();
            

        }

        private void updatePage()
        {
            OrderDataGrid.ItemsSource = DatabaseConnection.getOrders();
            OrderDataGrid.Items.Refresh();
        }
    }
}

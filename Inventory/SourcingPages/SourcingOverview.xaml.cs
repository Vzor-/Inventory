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
using Inventory.Popups;

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
            if (item != null)
            {
                setOrderToOrdered(item);
                updatePage();
            }
        }

        private void SourcingOverview_Approved_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Order item = (DataTypes.Order)OrderDataGrid.CurrentItem;
            if (item != null)
            {
                setOrderToApproved(item);
                updatePage();
            }
        }

        private void SourcingOverview_Received_Item_Click(object sender, RoutedEventArgs e)
        {
            DataTypes.Order item = (DataTypes.Order)OrderDataGrid.CurrentItem;
            if (item != null)
            {
                setOrderToRecived(item);
                updatePage();
            }
        }

        private void setOrderToOrdered(Order item)
        {
            if (!item.Ordered)
            {
                DatabaseConnection.updateOrder(item, nameof(item.Ordered), true);
            }
        }

        private void setOrderToApproved(Order item)
        {
            if (!item.Approved)
            {
                DatabaseConnection.updateOrder(item, nameof(item.Approved), true);
            }
        }

        private void setOrderToRecived(Order item)
        {
            if (!item.Recived)
            {
                DatabaseConnection.updateOrder(item, nameof(item.Recived), true);
                intakeOrderStock(item);
            }
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

        private void OrderDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void OrderDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (OrderDataGrid.CurrentCell.Column.Header.ToString() == "orderedDate" || OrderDataGrid.CurrentCell.Column.Header.ToString() == "expectedDate")
            {
                CalendarDialog window;
                DateTime curDate;
                String tempDate = "";
                if (OrderDataGrid.CurrentCell.Column.Header.ToString() == "orderedDate")
                {
                    tempDate = ((DataTypes.Order)OrderDataGrid.CurrentItem).orderedDate;
                }
                else if (OrderDataGrid.CurrentCell.Column.Header.ToString() == "expectedDate")
                {
                    tempDate = ((DataTypes.Order)OrderDataGrid.CurrentItem).expectedDate;
                }
                if (DateTime.TryParse(tempDate, out curDate))
                {
                    window = new CalendarDialog(curDate);
                }
                else
                {
                    window = new CalendarDialog();
                }
                DateTime? date = null;
                window.selectButton.Click += delegate
                {
                    date = window.selectedDate;
                    window.Close();
                };
                Boolean? a = window.ShowDialog();

                if (date != null)
                {
                    DataTypes.Order item = (DataTypes.Order)OrderDataGrid.CurrentItem;
                    if (OrderDataGrid.CurrentCell.Column.Header.ToString() == "orderedDate") {
                        item.SetOrderedDate(date.Value.ToShortDateString());
                    } else if (OrderDataGrid.CurrentCell.Column.Header.ToString() == "expectedDate")
                    {
                        item.SetExpectedDate(date.Value.ToShortDateString());
                    }
                    DatabaseConnection.updateOrder(item);
                }
            }
            updatePage();
        }
    }
}

using Inventory.DataTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace Inventory
{
    /// <summary>
    /// Interaction logic for SourcingAdd.xaml
    /// </summary>
    public partial class SourcingAdd : UserControl
    {
        List<DataTypes.OrderPart> pendingStockOrder;
        String orderID = "0";
        Boolean aproved = false;
        Boolean ordered = false;
        Boolean recived = false;
        String orderedDate = "";
        String expectedDate = "";

        public SourcingAdd()
        {
            InitializeComponent();
            avalibleParts.ItemsSource = DatabaseConnection.getParts();

            pendingStockOrder = new List<DataTypes.OrderPart>();
            pendingParts.ItemsSource = pendingStockOrder;

        }
        
        private void AvalibleParts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid q = (DataGrid)sender;
            AddToOrder((Part)q.CurrentItem);
        }


        private void AddToOrder(Part sender)
        {
            DataTypes.OrderPart item = new DataTypes.OrderPart(orderID, sender.InternalID, 0);
            if (pendingStockOrder.Find(x => x.PartID == item.PartID) == null)
            {
                Debug.WriteLine("New Item added to sourcing List:" + sender.EnglishName);
                pendingStockOrder.Add(item);
                pendingParts.Items.Refresh();
            }
            else
            {
                Debug.WriteLine("New Item rejected in sourcing List:" + sender.EnglishName);

            }

        }

        private void avalibleParts_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataGrid q = (DataGrid)sender;
                AddToOrder((Part)q.CurrentItem);
            }
        }

        private void addParts_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in avalibleParts.SelectedItems) {
                AddToOrder((Part)item);
            }
        }

        private void pendingParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void clearOrder_Click(object sender, RoutedEventArgs e)
        {
            clearOrder();
        }

        private void clearOrder()
        {
            pendingStockOrder.Clear();
            pendingParts.Items.Refresh();
            orderID = "";

        }

        private void applyOrder_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order(orderID, aproved, ordered, recived, orderedDate, expectedDate);
            string entryInfo = DatabaseConnection.addOrder(order);
            if (entryInfo != "Passed") { // there was an error in the SQL statement!!!
                string messageBoxText = entryInfo;
                string caption = "Inventory Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon);
                return; // do not continue to change tables!
            }
            // add parts to OrderHasParts tables for the order
            DatabaseConnection.addPartsToOrder(order, pendingStockOrder);

            clearOrder();

        }

        private void orderNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (orderNameTextBox.Text != orderID) {
                orderID = orderNameTextBox.Text;

                foreach (var item in pendingStockOrder) {
                    item.OrderID = orderID;
                }
                pendingParts.Items.Refresh();
            }
        }
    }
}

using Inventory.DataTypes;
using Inventory.Popups;
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

namespace Inventory
{
    /// <summary>
    /// Interaction logic for Fulfillment.xaml
    /// </summary>
    public partial class Fulfillment : UserControl
    {

        List<DataTypes.Stock> allocatedStock = new List<DataTypes.Stock>(); // store allocated stock for operation
        List<DataTypes.Stock> stockToUpdate = new List<DataTypes.Stock>(); // store new values of stock
        List<DataTypes.Stock> allStockPristine = new List<DataTypes.Stock>();
        List<DataTypes.Stock> allStock = new List<DataTypes.Stock>();
        List<Part> parts;
        Part lastSelectedPart = null;
        List<Location> buildingList { get; set; }

        public Fulfillment()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception
                parts = DatabaseConnection.getParts();
                partsListGrid.ItemsSource = parts;
                datePicker.SelectedDate = DateTime.UtcNow;

                buildingList = DatabaseConnection.getLocations(Location.LocationType.LocationBuilding);
                locationFilterComboBox.ItemsSource = buildingList;
                locationFilterComboBox.SelectedItem = buildingList.Find(x => x.Name == "ChicagoAve");
                allStockPristine = (List<DataTypes.Stock>)DatabaseConnection.getStock();
                //allStock = allStockPristine.FindAll(x => x.Location == ((Location)locationFilterComboBox.SelectedItem).ID).ConvertAll(stock => new DataTypes.Stock(stock));
                allStock = allStockPristine.ConvertAll(stock => new DataTypes.Stock(stock));
            }
        }

        private void locationFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void partsListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sourceGrid = (DataGrid)(e.Source);

            if (sourceGrid != null)
            {
                stockLocationsUpdateFromPart((DataTypes.Part)sourceGrid.CurrentItem);
                lastSelectedPart = (DataTypes.Part)sourceGrid.CurrentItem;
            }
        }

        private void stockLocationsUpdateFromPart(DataTypes.Part partItem)
        {
            //var loc = DatabaseConnection.getLocationsOfStock(partItem);
            var loc = allStock.FindAll(x => x.PartID == partItem.InternalID);
            stockLocations.ItemsSource = loc;
        }
        

        private int NewItemQty(string partID)
        {
            int returnPart = 0;

            PartQtyDialog dialog = new PartQtyDialog(partID);
            dialog.ShowDialog();
            if (dialog.isClean)
            {
                returnPart = dialog.partQty;
            }

            return returnPart;
        }
        

        private void stockAllocation_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sourceGrid = (DataGrid)(e.Source);
            var item = (DataTypes.Stock)sourceGrid.CurrentItem;
            if (item != null)
            {

                var tempItem = allStock.Find(x => x.Location == item.Location && x.PartID == item.PartID);

                if (tempItem == null)
                {
                    allStock.Add(new DataTypes.Stock(item.PartID, item.Count, item.Location));
                }
                else {
                    tempItem.Count += item.Count;
                }
                stockToUpdate.Remove(item);
                allocatedStock.Remove(item);
            }

            refreshTheBoxes();
            checkCanCreateFulfillment();
        }
        

        private void stockLocations_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var sourceGrid = (DataGrid)(e.Source);
            var item = (DataTypes.Stock)sourceGrid.CurrentItem;
            if (item != null)
            {
                var count = NewItemQty(item.PartID);
                if (count > 0)
                {
                    if (item.Count == 0) return;

                    if (count >= item.Count)
                    {

                        allocatedStock.Add(new DataTypes.Stock(item.PartID, item.Count, item.Location));
                        stockToUpdate.Add(new DataTypes.Stock(item.PartID, 0, item.Location));
                        allStock.Remove(item);
                    }
                    else
                    {
                        allocatedStock.Add(new DataTypes.Stock(item.PartID, count, item.Location));
                        item.Count -= count;
                        stockToUpdate.Add(new DataTypes.Stock(item.PartID, item.Count, item.Location));
                        
                    }
                }
                refreshTheBoxes();
            }
            checkCanCreateFulfillment();
        }

        private void refreshTheBoxes()
        {
            stockLocationsUpdateFromPart(lastSelectedPart);
            stockAllocation.ItemsSource = allocatedStock;
            stockLocations.Items.Refresh();
            stockAllocation.Items.Refresh();
        }

        private void checkCanCreateFulfillment()
        {
            Boolean ready = true;
            if (allocatedStock.Count == 0) ready = false;
            if (datePicker.SelectedDate.HasValue == false) ready = false;
            if (poNameTextBox.Text == "") ready = false;
            
             applyFulfillment.IsEnabled = ready;
            
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            // is date good?

            //
            checkCanCreateFulfillment();
        }

        private void applyFulfillment_Click(object sender, RoutedEventArgs e)
        {
            if (applyFulfillment.IsEnabled) {
                foreach (var item in allocatedStock) {
                    DatabaseConnection.addStockLoss(new Loss(item.PartID, "Fulfillment: " + poNameTextBox.Text, item.Count, datePicker.SelectedDate.ToString()));

                }
                foreach (var item in stockToUpdate) {
                    DatabaseConnection.updateStockCount(item);
                }

                allStockPristine = (List<DataTypes.Stock>)DatabaseConnection.getStock();
                allStock = allStockPristine.ConvertAll(stock => new DataTypes.Stock(stock));
                stockToUpdate.Clear();
                allocatedStock.Clear();
                applyFulfillment.IsEnabled = false;
                poNameTextBox.Text = "";
                refreshTheBoxes();
            }
        }

        private void poNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkCanCreateFulfillment();
        }
    }
}

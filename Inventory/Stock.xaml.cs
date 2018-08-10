using Inventory.DataTypes;
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
    public partial class Stock : UserControl
    {
        List<Part> parts;
        List<DataTypes.Stock> allStock;
        List<DataTypes.Stock> summaryStock = new List<DataTypes.Stock>();

        public Stock()
        { 
            InitializeComponent();
            locationsViewer.selectioChangedLocation += newSelectedLocation;

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception
                parts = DatabaseConnection.getParts();
                allStock = (List<DataTypes.Stock>)DatabaseConnection.getStock();
                foreach (Part item in parts)
                {
                    var count = allStock.FindAll(x => x.PartID == item.InternalID).Sum(y => y.Count);
                    if (count != 0)
                    {
                        summaryStock.Add(new DataTypes.Stock(item.InternalID, count, -1));
                    }
                }
                stockSummaryGrid.ItemsSource = summaryStock;
            }
            

        }
        

        private void newSelectedLocation(object sender, SelectionChangedEventArgs e)
        {
            var data = (DataGrid)(e.Source);
            Location locItem = (Location)(data.CurrentItem);

            var items = allStock.FindAll(x => x.Location == locItem.ID);
            stockInLocation.ItemsSource = items;
        }

        private void stockSummaryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sourceGrid = (DataGrid)(e.Source);

            if (sourceGrid != null)
            {
                stockLocationsUpdateFromSummary((DataTypes.Stock)sourceGrid.CurrentItem);
            }
        }


        private void SetSelectedLocation(int location) {
            locationsViewer.externalLocationSet(location);
        }

        private void stockInLocationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sourceGrid = (DataTypes.Stock)((DataGrid)(e.Source)).CurrentItem;
            if (sourceGrid != null)
            {
                // update stockLocations to see all location of selected stock
                stockLocations.ItemsSource = allStock.FindAll(x => x.PartID == sourceGrid.PartID);
            }
        }

        private void stockLocationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sourceGrid = (DataTypes.Stock)((DataGrid)(e.Source)).CurrentItem;
            if (sourceGrid != null)
            {
                // update stockInLocations to view all items with selected stock
                stockInLocation.ItemsSource = allStock.FindAll(x => x.Location == sourceGrid.Location);
                // update LocationView for current stock location
                SetSelectedLocation(sourceGrid.Location);

            }
        }


        private void stockLocationsUpdateFromSummary(DataTypes.Stock stockItem) {

            var loc = DatabaseConnection.getLocationsOfStock(stockItem);

            stockLocations.ItemsSource = loc;
        }

    }
}

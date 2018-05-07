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
        List<DataGrid> gridList;
        public Stock()
        { 
            InitializeComponent();
            BuildingGrid.ItemsSource = DatabaseConnection.getLocations(Location.LocationType.LocationBuilding);
            gridList = new List<DataGrid>();
            gridList.Add(BuildingGrid);
            gridList.Add(ShelfGrid);
            gridList.Add(BinGrid);
            gridList.Add(PartitionGrid);

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            //Get refences to stuff we need
            var sourceButton = (Button)(e.Source);
            int index = Grid.GetColumn(sourceButton);
            int parentID = getParentID(index);
            if (parentID == -2) return;
            var target = gridList[index];
            //Make and add new item
            var newLocation = Location.NewItem((Location.LocationType)index , parentID);
            if (newLocation != null) DatabaseConnection.addLocation(newLocation);
            //Update the table and clear all lower tables
            target.ItemsSource = DatabaseConnection.getLocationsInLocation((Location.LocationType)index, parentID);
            for (index++; index < 4; index++)
            {
                target = gridList[index];
                target.ItemsSource = null;
            }
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            //Get refences to stuff we need
            var sourceButton = (Button)(e.Source);
            int index = Grid.GetColumn(sourceButton);
            var target = gridList[index];
            var locationToDelete = (Location)target.SelectedItem;
            int parentID = getParentID(index);
            if (parentID == -2) return;
            //Delete the item
            if (locationToDelete != null)
            {
                DatabaseConnection.delLocation(locationToDelete);
                target.ItemsSource = DatabaseConnection.getLocationsInLocation((Location.LocationType)index, parentID);
            }
            //Clear lower tables
            for (index++; index < 4; index++)
            {
                target = gridList[index];
                target.ItemsSource = null;
            }
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get refences to stuff we need
            var sourceGrid = (DataGrid)(e.Source);
            int index = Grid.GetColumn(sourceGrid) + 1;
            //This means it is a partList
            if (index >= 4) return;
            var target = (DataGrid)gridList[index];
            var locationToShow = (Location)sourceGrid.SelectedItem;
            //Delete the item
            if (locationToShow != null)
            {
                target.ItemsSource = DatabaseConnection.getLocationsInLocation((Location.LocationType)index, locationToShow.ID);
            }
            //Clear lower tables
            for (index++; index < 4; index++)
            {
                target = gridList[index];
                target.ItemsSource = null;
            }
        }
        
        private int getParentID(int currentGridIndex)
        {
            currentGridIndex--;
            if ((currentGridIndex < 0) || (currentGridIndex >= gridList.Count)) return -1;
            if (gridList[currentGridIndex].SelectedItem == null) return -2;
            return ((Location)gridList[currentGridIndex].SelectedItem).ID;
        }
    }
}

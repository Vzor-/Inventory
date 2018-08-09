using Inventory.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for LocationsView.xaml
    /// </summary>
    public partial class LocationsView : UserControl
    {
        public int lastDroppedLocation { get; private set; }

        public event SelectionChangedEventHandler selectioChangedLocation;
        
        private static void GridIsAvailableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public int lastSelectedLocation
        {
            get { return (int)this.GetValue(lastSelLocDP); }
            set { this.SetValue(lastSelLocDP, value); }
        }


        public static readonly DependencyProperty lastSelLocDP = DependencyProperty.Register(
          "LastSelectedLocation", typeof(int), typeof(LocationsView), new PropertyMetadata());

        public Boolean EditState
        {
            get { return (Boolean)this.GetValue(EditProperty); }
            set { this.SetValue(EditProperty, value); }
        }

        public static DependencyProperty ItemsSourceListenerProperty { get; private set; }

        public static readonly DependencyProperty EditProperty = DependencyProperty.Register(
          "EditState", typeof(Boolean), typeof(LocationsView), new PropertyMetadata(true));

        List<DataGrid> gridList;
        Location lastSelected;

        public LocationsView()
        {
            this.InitializeComponent();

            (this.Content as FrameworkElement).DataContext = this;

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception

                BuildingGrid.ItemsSource = DatabaseConnection.getLocations(Location.LocationType.LocationBuilding);

            }

            gridList = new List<DataGrid>();
            gridList.Add(BuildingGrid);
            gridList.Add(ShelfGrid);
            gridList.Add(BinGrid);
            gridList.Add(PartitionGrid);
        }

        public int currentBin()
        {
            return lastSelected.ID;
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
            var newLocation = Location.NewItem((Location.LocationType)index, parentID);
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

        public Boolean externalLocationSet(int location) {
            Boolean ret = false;
            if(lastSelected != null) if (location == lastSelected.ID) return true;
            
            


            return ret;
        }

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get refences to stuff we need
            var sourceGrid = (DataGrid)(e.Source);
            int index = Grid.GetColumn(sourceGrid) + 1;

            //Store lastSelected Item
            if (sourceGrid.SelectedItem != null)
            {
                lastSelected = (Location)sourceGrid.SelectedItem;
                selectioChangedLocation?.Invoke(this, e);
                Debug.WriteLine("Locations lastSelected: " + lastSelected.Name);
            }
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


        public void BuildingGrid_Drop(object sender, DragEventArgs e)
        {
            ReportItemDropped(((Location)((DataGrid)sender).SelectedItem).ID);
        }


        public void ShelfGrid_Drop(object sender, DragEventArgs e)
        {
            ReportItemDropped(((Location)((DataGrid)sender).SelectedItem).ID);
        }

        public void BinGrid_Drop(object sender, DragEventArgs e)
        {
            ReportItemDropped(((Location)((DataGrid)sender).SelectedItem).ID);
        }

        public void PartitionGrid_Drop(object sender, DragEventArgs e)
        {
            ReportItemDropped(((Location)((DataGrid)sender).SelectedItem).ID);
        }


        private void ReportItemDropped(int iD)
        {
            lastDroppedLocation = iD;
            OnItemDropped(new EventArgs());
        }

        public event EventHandler droppedItem;

        protected virtual void OnItemDropped(EventArgs e)
        {
            droppedItem?.Invoke(this, e);
        }



    }
}

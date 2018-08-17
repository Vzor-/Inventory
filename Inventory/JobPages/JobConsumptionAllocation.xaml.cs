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
using System.Windows.Shapes;

namespace Inventory.JobPages
{
    /// <summary>
    /// Interaction logic for JobRecipeStockAllocation.xaml
    /// </summary>
    public partial class JobConsumptionAllocation : Window
    {
        Brush originalBackgroundColor;
        JobConsumptionAllocationLogic jobConsumptionAllocationLogic;
        public JobConsumptionAllocation()
        {
            InitializeComponent();
            originalBackgroundColor = this.Background;
            jobConsumptionAllocationLogic =null;
        }

        public JobConsumptionAllocation(Job job) : this()
        {
            jobConsumptionAllocationLogic = new JobConsumptionAllocationLogic(job);
            jobsConsumptionDataGrid.ItemsSource = jobConsumptionAllocationLogic.jobConsumption;

            //foreach (var item in parts)
            //{
            //    stock.Add(new DataTypes.Stock(item.PartID, item.Count, deafultStockLocation));
            //}
            //stockGrid.Items.Refresh();
            if (jobConsumptionAllocationLogic.CanAllocate == false) {
                this.Background = Brushes.Red;
            }
        }

        private Boolean CanJobConsumptionBeAllocated() {
            Boolean ret = false;


            return ret;
        }

        private void recipePartsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sourceGrid = (DataGrid)(e.Source);

            //if (sourceGrid.CurrentItem != null)
            //{
            //    RecipeHasPart recipeHasPart = (RecipeHasPart)sourceGrid.CurrentItem;
            //    stockLocations.ItemsSource = allStock.FindAll(x => x.PartID == recipeHasPart.PartID);
            //}
        }

        private void recipeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (allocatingOrder == false)
            //{
            //    var sourceGrid = (DataGrid)(e.Source);
            //    currentCake = (Recipe)sourceGrid.CurrentItem;
            //    recipePartsGrid.ItemsSource = DatabaseConnection.getRecipeHasPart(currentCake.InternalID);
            //}
        }

        private void stockLocationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void removeFromPartAllocation(DataTypes.Stock item)
        {

        }
        private void addToPartAllocation(DataTypes.Stock item)
        {
            //var carton = allocatedStock.FindAll(eggs => eggs.PartID == item.PartID); // filter this part
            //var shells = carton.Find(eggs => eggs.Count < 0); // need ()
            //var yolks = carton.FindAll(eggs => eggs.Count > 0).Sum(y => y.Count); // allocated

            //shells.Count *= -1;

            //if (shells.Count > item.Count)
            //{
            //    var amount = item.Count;
            //    //take all
            //    allocatedStock.Add(new DataTypes.Stock(item));
            //    shells.Count -= amount;
            //    item.Count -= amount;
            //    allStock.Remove(item);
            //}
            //else if (shells.Count <= item.Count)
            //{
            //    var amount = item.Count;

            //    //take some
            //    allocatedStock.Add(new DataTypes.Stock(item.PartID, shells.Count, item.Location));

            //    item.Count -= amount;

            //    allocatedStock.Remove(shells);
            //}

            //shells.Count *= -1;

        }

        private void stockInLocationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void selectRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (allocatingOrder == false)
            //{
            //    if (currentCake != null)
            //    {
            //        count = -1;
            //        int.TryParse(RecipeQuantity.Text, out count);
            //        if ((RecipeQuantity.Text != "") && (count > 0))
            //        {
            //            // update needed part allocation
            //            allocatingOrder = true;
            //            selectRecipeButton.Content = "Cancel";
            //            recipeGrid.IsEnabled = false;
            //            calcNeededParts(currentCake, count);
            //        }
            //    }
            //}
            //else
            //{
            //    // cancel part allocation
            //    allocatingOrder = false;
            //    selectRecipeButton.Content = "Select";
            //    recipeGrid.IsEnabled = true;
            //    allocatedStock.Clear();
            //    allStock = allStockPristine.ConvertAll(stock => new DataTypes.Stock(stock));
            //    count = -1;

            //    stockLocations.Items.Refresh();
            //    stockAllocation.Items.Refresh();
            //}
        }

        private void calcNeededParts(Recipe cake, int count)
        {
            //var carton = DatabaseConnection.getRecipeHasPart(cake.InternalID);
            //allocatedStock.Clear();
            //foreach (var egg in carton)
            //{
            //    allocatedStock.Add(new DataTypes.Stock(egg.PartID, -egg.Count * count, -1));
            //}
            //stockAllocation.ItemsSource = allocatedStock;
        }

        private void RecipeQuantity_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = char.IsDigit(Convert.ToChar(e.Key)) && !char.IsControl(Convert.ToChar(e.Key));
        }

        private void stockLocations_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (allocatingOrder == true)
            //{
            //    var sourceGrid = (DataGrid)(e.Source);
            //    var item = (DataTypes.Stock)sourceGrid.CurrentItem;

            //    if (allocatedStock.Find(egg => egg.PartID == item.PartID) != null) // was selected in needed parts list??
            //    {
            //        if (item.Count > 0) addToPartAllocation(item);
            //    }
            //}
            //stockLocations.Items.Refresh();
            //stockAllocation.Items.Refresh();
            //checkJobNeeds();
        }

        private void checkJobNeeds()
        {
            //if (allocatedStock.Find(egg => egg.Count <= 0) == null)
            //{
            //    // no need or zero qty. items in part allocation list

            //}
        }

        private void applyRecipeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

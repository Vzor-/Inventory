using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Inventory.DataTypes;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for SourcingStockIntake.xaml
    /// </summary>
    public partial class SourcingStockIntake : Window
    {
        List<DataTypes.Stock> stock = new List<DataTypes.Stock>();
        DataTypes.Stock curSelectedStock;
        int deafultStockLocation = 0;
        

        public SourcingStockIntake()
        {
            InitializeComponent();
            stockGrid.ItemsSource = stock;
        }

        public SourcingStockIntake(List<DataTypes.Stock> parts) : this()
        {
            foreach (var item in parts)
            {
                stock.Add(new DataTypes.Stock(item.PartID, item.Count, deafultStockLocation));
            }
            stockGrid.Items.Refresh();
        }

        public SourcingStockIntake(List<OrderPart> parts) : this()
        {
            foreach (var item in parts)
            {
                stock.Add(new DataTypes.Stock(item.PartID, item.Count, deafultStockLocation));
            }
            stockGrid.Items.Refresh();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataTypes.Stock item in stock)
            {
                DatabaseConnection.AddStock(item);
            }

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void stockGrid_DragLeave(object sender, DragEventArgs e)
        {
            curSelectedStock = (DataTypes.Stock)((DataGrid)sender).CurrentItem;

        }
        

        private void assignButton_Click(object sender, RoutedEventArgs e)
        {
            curSelectedStock.Location = locationsStockView.currentBin();
            stockGrid.Items.Refresh();
            checkStockLocations();
        }

        private void checkStockLocations()
        {
            Boolean locationsValid = true;

            foreach (DataTypes.Stock item in stock) {
                if (deafultStockLocation == item.Location) locationsValid = false;
            }

            if (locationsValid) applyButton.IsEnabled = true;
        }

        private void locationsStockView_droppedItem(object sender, EventArgs e)
        {

        }

        private void stockGrid_PreviewDragLeave(object sender, DragEventArgs e)
        {

        }

        private void stockGrid_DragOver(object sender, DragEventArgs e)
        {

        }

        private void stockGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curSelectedStock = (DataTypes.Stock)((DataGrid)sender).CurrentItem;
            Debug.WriteLine("curSelectedStock: "+ curSelectedStock);
        }
    }
}

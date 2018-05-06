using Inventory.DataTypes;
using Inventory.Popups;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class Products : UserControl
    {
        public Products()
        {
            InitializeComponent();
            productGrid.ItemsSource = DatabaseConnection.getProducts();
            partsGrid.ItemsSource = DatabaseConnection.getParts();
        }

        private void addProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductAddDialog dialog = new ProductAddDialog();
            dialog.ShowDialog();
            if (dialog.isClean)
            {
                DatabaseConnection.addProduct(new DataTypes.Product(
                    dialog.EnglishName,
                    dialog.InternalID,
                    dialog.IsActive));
                productGrid.ItemsSource = DatabaseConnection.getProducts();
            }
        }

        private void productGrid_MouseDown(object sender, SelectionChangedEventArgs e)
        {
            if (productGrid.SelectedIndex != -1)
            {
                delProductButton.IsEnabled = true;
                editProductButton.IsEnabled = true;
                updateHasParts();
            }
        }

        private void delProductButton_Click(object sender, RoutedEventArgs e)
        {
            var s = (DataTypes.Product)productGrid.SelectedItem;

            ProductDelDialog dialog = new ProductDelDialog(s.EnglishName, s.InternalID);
            dialog.ShowDialog();
            if (dialog.isDelete)
            {
                delProductButton.IsEnabled = false;
                editProductButton.IsEnabled = false;
                DatabaseConnection.delProduct(s);
                productGrid.ItemsSource = DatabaseConnection.getProducts();
            }
        }

        private void editProductButton_Click(object sender, RoutedEventArgs e)
        {
            var s = (DataTypes.Product)productGrid.SelectedItem;

            ProductEditDialog dialog = new ProductEditDialog(s.EnglishName, s.InternalID, s.IsActive);
            dialog.ShowDialog();
            if (dialog.isClean)
            {
                s.EnglishName = dialog.EnglishName;
                s.IsActive = dialog.IsActive;
                DatabaseConnection.editProduct(s);
                productGrid.ItemsSource = DatabaseConnection.getProducts();
            }
        }

        private void partsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (partsGrid.SelectedIndex != -1)
            {
                delPartButton.IsEnabled = true;
                editPartButton.IsEnabled = true;
            }
        }

        private void addPartButton_Click(object sender, RoutedEventArgs e)
        {
            Console.Out.WriteLine( Location.LocationType.LocationPartition.ToString());
            Part part = Part.NewItem();
            if (part != null)
            {
                DatabaseConnection.addPart(part);
                partsGrid.ItemsSource = DatabaseConnection.getParts();
            }
        }

        private void delPartButton_Click(object sender, RoutedEventArgs e)
        {
            var s = (DataTypes.Part)partsGrid.SelectedItem;

            PartDelDialog dialog = new PartDelDialog(s.EnglishName, s.InternalID);
            dialog.ShowDialog();
            if (dialog.isDelete)
            {
                delPartButton.IsEnabled = false;
                editPartButton.IsEnabled = false;
                delFromProduct.IsEnabled = false;
                DatabaseConnection.delPart(s);
                partsGrid.ItemsSource = DatabaseConnection.getParts();
            }
        }

        private void editPartButton_Click(object sender, RoutedEventArgs e)
        {
            var s = (DataTypes.Part)partsGrid.SelectedItem;

            PartEditDialog dialog = new PartEditDialog(s.EnglishName, s.InternalID, s.IsEOL);
            dialog.ShowDialog();
            if (dialog.isClean)
            {
                s.EnglishName = dialog.EnglishName;
                s.IsEOL = dialog.IsActive;
                DatabaseConnection.editPart(s);
                partsGrid.ItemsSource = DatabaseConnection.getParts();
            }
        }

        private void updateHasParts()
        {
            string partName = ((DataTypes.Product)productGrid.SelectedItem).EnglishName;
            HasPartText.Text = "\"" + partName + "\" Has Parts";
            HasPartsGrid.ItemsSource = DatabaseConnection.getProductHasPart(getProductID());
        }

        private void addToProduct_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection.addProductHasPart(
                (DataTypes.Product)productGrid.SelectedItem, 
                (DataTypes.Part)partsGrid.SelectedItem);
            HasPartsGrid.ItemsSource = DatabaseConnection.getProductHasPart(getProductID());
        }

        private void delFromProduct_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection.delProductHasPart(
                (DataTypes.Product)productGrid.SelectedItem,
                (DataTypes.Part)partsGrid.SelectedItem);
            HasPartsGrid.ItemsSource = DatabaseConnection.getProductHasPart(getProductID());
            delFromProduct.IsEnabled = false;
        }

        private string getProductID()
        {
            return ((DataTypes.Product)productGrid.SelectedItem).InternalID;
        }

        private void HasPartsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            delFromProduct.IsEnabled = true;
            HasPartsGrid.Columns[0].IsReadOnly = true;
            HasPartsGrid.Columns[1].IsReadOnly = true;
        }

        private void HasPartsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var s = (DataTypes.ProductHasPart)HasPartsGrid.Items.GetItemAt(e.Row.GetIndex());
                try
                {
                    int count = Int32.Parse(((TextBox)e.EditingElement).Text);
                    DatabaseConnection.editProductHasPart(getProductID(), s.PartID, count);
                } catch (Exception)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

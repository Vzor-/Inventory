using Inventory.DataTypes;
using Inventory.Popups;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory
{
    public partial class Recipes : UserControl
    {
        public Recipes()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception
                recipeGrid.ItemsSource = DatabaseConnection.getRecipes();
                partsGrid.ItemsSource = DatabaseConnection.getParts();
            }
        }

        private void addRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            RecipeAddDialog dialog = new RecipeAddDialog();
            dialog.ShowDialog();
            if (dialog.isClean)
            {
                DatabaseConnection.addRecipe(new DataTypes.Recipe(
                    dialog.EnglishName,
                    dialog.InternalID,
                    dialog.IsActive));
                recipeGrid.ItemsSource = DatabaseConnection.getRecipes();
            }
        }

        private void RecipeGrid_MouseDown(object sender, SelectionChangedEventArgs e)
        {
            if (recipeGrid.SelectedIndex != -1)
            {
                delRecipeButton.IsEnabled = true;
                editRecipeButton.IsEnabled = true;
                updateHasParts();
            }
        }

        private void delRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            var s = (DataTypes.Recipe)recipeGrid.SelectedItem;

            RecipeDelDialog dialog = new RecipeDelDialog(s.EnglishName, s.InternalID);
            dialog.ShowDialog();
            if (dialog.isDelete)
            {
                delRecipeButton.IsEnabled = false;
                editRecipeButton.IsEnabled = false;
                DatabaseConnection.delRecipe(s);
                recipeGrid.ItemsSource = DatabaseConnection.getRecipes();
            }
        }

        private void editRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            var s = (DataTypes.Recipe)recipeGrid.SelectedItem;

            RecipeEditDialog dialog = new RecipeEditDialog(s.EnglishName, s.InternalID, s.IsActive);
            dialog.ShowDialog();
            if (dialog.isClean)
            {
                s.EnglishName = dialog.EnglishName;
                s.IsActive = dialog.IsActive;
                DatabaseConnection.editRecipe(s);
                recipeGrid.ItemsSource = DatabaseConnection.getRecipes();
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
                delFromRecipe.IsEnabled = false;
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
            string partName = ((DataTypes.Recipe)recipeGrid.SelectedItem).EnglishName;
            HasPartText.Text = "\"" + partName + "\" Has Parts";
            HasPartsGrid.ItemsSource = DatabaseConnection.getRecipeHasPart(getRecipeID());
        }

        private void addToRecipe_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection.addRecipeHasPart(
                (DataTypes.Recipe)recipeGrid.SelectedItem, 
                (DataTypes.Part)partsGrid.SelectedItem);
            HasPartsGrid.ItemsSource = DatabaseConnection.getRecipeHasPart(getRecipeID());
        }

        private void delFromRecipe_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection.delRecipeHasPart(
                (DataTypes.Recipe)recipeGrid.SelectedItem,
                (DataTypes.Part)partsGrid.SelectedItem);
            HasPartsGrid.ItemsSource = DatabaseConnection.getRecipeHasPart(getRecipeID());
            delFromRecipe.IsEnabled = false;
        }

        private string getRecipeID()
        {
            return ((DataTypes.Recipe)recipeGrid.SelectedItem).InternalID;
        }

        private void HasPartsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            delFromRecipe.IsEnabled = true;
            HasPartsGrid.Columns[0].IsReadOnly = true;
            HasPartsGrid.Columns[1].IsReadOnly = true;
        }

        private void HasPartsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var s = (DataTypes.RecipeHasPart)HasPartsGrid.Items.GetItemAt(e.Row.GetIndex());
                try
                {
                    int count = Int32.Parse(((TextBox)e.EditingElement).Text);
                    DatabaseConnection.editRecipeHasPart(getRecipeID(), s.PartID, count);
                } catch (Exception)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}

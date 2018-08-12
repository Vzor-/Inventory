using Inventory.DataTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.JobPages
{
    /// <summary>
    /// Interaction logic for JobAdd.xaml
    /// </summary>
    public partial class JobAdd : UserControl
    {
        List<JobHasRecipe> pendingRecipes;
        string jobName;
        public JobAdd()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception

                avalibleRecipes.ItemsSource = DatabaseConnection.getRecipes();
                pendingRecipes = new List<JobHasRecipe>();
                pendingRecipesDataGrid.ItemsSource = pendingRecipes;
            }
        }


        private void AvalibleParts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid q = (DataGrid)sender;
            AddToJob((Recipe)q.CurrentItem);
        }


        private void AddToJob(Recipe sender)
        {
            DataTypes.JobHasRecipe item = new DataTypes.JobHasRecipe(jobName, sender.InternalID, 0);
            if (pendingRecipes.Find(x => x.RecipeID == item.RecipeID) == null)
            {
                Debug.WriteLine("New Item added to sourcing List:" + sender.EnglishName);
                pendingRecipes.Add(item);
                pendingRecipesDataGrid.Items.Refresh();
            }
            else
            {
                Debug.WriteLine("New Item rejected in sourcing List:" + sender.EnglishName);

            }
            checkCanCreateJob();

        }
             
        private void pendingParts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void clearJob_Click(object sender, RoutedEventArgs e)
        {
            clearJob();
        }

        private void clearJob()
        {
            pendingRecipes.Clear();
            pendingRecipesDataGrid.Items.Refresh();
            jobName = "";
            jobNameTextBox.Text = jobName;
            checkCanCreateJob();

        }


        private void checkCanCreateJob()
        {
            Boolean ready = true;
            if (pendingRecipes.Find(x=> x.Count <= 0) != null) ready = false;
            if (jobName == "") ready = false;

            applyJobButton.IsEnabled = ready;

        }

        private void addRecipe_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in avalibleRecipes.SelectedItems)
            {
                AddToJob((Recipe)item);
            }
        }

        private void AvalibleRecipes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid q = (DataGrid)sender;
            AddToJob((Recipe)q.CurrentItem);
        }
        private void applyJob_Click(object sender, RoutedEventArgs e)
        {
            checkCanCreateJob();
            if (applyJobButton.IsEnabled == false) return;
            Job newJob = new Job(jobName, false, false);
            string entryInfo = DatabaseConnection.addJob(newJob);
            if (entryInfo != "Passed")
            { // there was an error in the SQL statement!!!
                string messageBoxText = entryInfo;
                string caption = "Inventory Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBox.Show(messageBoxText, caption, button, icon);
                return; // do not continue to change tables!
            }
            
            // add Recipes to JobHasRecipes tables for the Job
            DatabaseConnection.addRecipesToJob(newJob, pendingRecipes);

            clearJob();

        }

        private void avalibleRecipes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataGrid q = (DataGrid)sender;
                AddToJob((Recipe)q.CurrentItem);
            }
        }

        private void jobNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (jobNameTextBox.Text != jobName)
            {
                jobName = jobNameTextBox.Text;

                foreach (var item in pendingRecipes)
                {
                    item.JobID = jobName;
                }
                pendingRecipesDataGrid.Items.Refresh();
            }
            checkCanCreateJob();
        }

        private void avalibleRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        

        private void pendingRecipesDataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            checkCanCreateJob();
        }
    }
}


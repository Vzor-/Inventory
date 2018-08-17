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
        List<JobRecipeCount> pendingRecipes;
        string jobName;
        public JobAdd()
        {
            InitializeComponent();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                //Code that throws the exception

                avalibleRecipes.ItemsSource = DatabaseConnection.getRecipes();
                pendingRecipes = new List<JobRecipeCount>();
                pendingRecipesDataGrid.ItemsSource = pendingRecipes;
            }
        }


        //private void AvalibleParts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DataGrid q = (DataGrid)sender;
        //    AddToJob((Recipe)q.CurrentItem);
        //}


        private void AddToJob(Recipe sender)
        {

            JobRecipeCount item = new JobRecipeCount(sender, 0);
            if (pendingRecipes.Find(x => x.recipe.InternalID == item.recipe.InternalID) == null)
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
            if (pendingRecipes.Find(x=> x.count <= 0) != null) ready = false;
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
            Job newJob = new Job(jobName, 0);
            checkCanCreateJob();
            if (applyJobButton.IsEnabled == false) return;
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


            // create JobConsumption instances
            List<JobConsumption> jobConsumptions = new List<JobConsumption>();

            foreach (var consumer in pendingRecipes) {
                List<RecipeHasPart> recipeHasPart = DatabaseConnection.getRecipeHasPart(consumer.recipe.InternalID);
                foreach (var part in recipeHasPart) {
                    jobConsumptions.Add(new JobConsumption(newJob.InternalId, consumer.recipe.InternalID, -1, part.PartID, (part.Count * consumer.count), consumer.count, 7));
                }
            }

            // add Recipes to JobConsumption tables for the Job
            DatabaseConnection.addJobConsumptionToJob(jobConsumptions);

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

    public class JobRecipeCount
    {

        public JobRecipeCount(Recipe sender, int v)
        {
            this.recipe = sender;
            this.count = v;
        }
        

        public Recipe recipe { get; set; }
        public int count { get; set; }
    }
}


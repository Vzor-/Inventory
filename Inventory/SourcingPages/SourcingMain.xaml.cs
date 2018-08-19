using Inventory.DataTypes;
using Inventory.Popups;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Inventory.SourcingPages
{
    public partial class SourcingMain : UserControl
    {
        public SourcingMain()
        {
            InitializeComponent();
            //orderGrid.ItemsSource = DatabaseConnection.getRecipes();
        }

        private void sourcingTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                String i = e.AddedItems[0].ToString();
                if (i.Contains("Overview"))
                {
                    ((SourcingOverview)((TabItem)sourcingTabControl.Items[0]).Content).updateSourcing();
                }
            }
        }
    }
}

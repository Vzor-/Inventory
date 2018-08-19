using Inventory.DataTypes;
using Inventory.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Inventory.JobPages
{
    public partial class JobsMain : UserControl
    {
        public JobsMain()
        {
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
            }
        }

        private void jobsTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                String i = e.AddedItems[0].ToString();
                if (i.Contains("Overview"))
                {
                    ((JobsOverview)((TabItem)jobsTabControl.Items[0]).Content).updateJobsOverview();
                }
            }

            //}
        }
    }
}

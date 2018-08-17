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
    public partial class JobConsumptionView : Window
    {
        Brush originalBackgroundColor;
        JobConsumptionAllocationLogic jobConsumptionAllocationLogic;
        public JobConsumptionView()
        {
            InitializeComponent();
            originalBackgroundColor = this.Background;
            jobConsumptionAllocationLogic =null;
        }

        public JobConsumptionView(Job job) : this()
        {
            jobConsumptionAllocationLogic = new JobConsumptionAllocationLogic(job);
            jobsConsumptionDataGrid.ItemsSource = jobConsumptionAllocationLogic.jobConsumption;
            
        }
        
    }
}

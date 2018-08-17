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

namespace Inventory.Popups
{
    public partial class BuildingLocationDialog : Window
    {
        public DataTypes.Location selectedLoc;
        public Boolean isClean = false;

        public BuildingLocationDialog()
        {
            InitializeComponent();
            selectionLocComboBox.ItemsSource = DatabaseConnection.getLocations(DataTypes.Location.LocationType.LocationBuilding);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if ((selectedLoc != null)) isClean = true;
            this.Close();
        }

        private void selectionLoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedLoc = (DataTypes.Location)selectionLocComboBox.SelectedItem;
            selectButton.IsEnabled = true;
        }
    }
}

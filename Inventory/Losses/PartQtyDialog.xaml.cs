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
    public partial class PartQtyDialog : Window
    {
        public int partQty = -1;
        public string EnglishName;
        public bool isClean = false;
        public PartQtyDialog(string EnglishName)
        {
            InitializeComponent();
            partNameField.Text = EnglishName;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(partQtyField.Text, out partQty);
            if ((partQty > 0)) isClean = true;
            this.Close();
        }
    }
}

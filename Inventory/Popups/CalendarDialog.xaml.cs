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
    public partial class CalendarDialog : Window
    {
        public DateTime selectedDate;
        public Boolean isClean = false;

        public CalendarDialog()
        {
            InitializeComponent();
            selectionCalendar.SelectionMode = CalendarSelectionMode.SingleDate;
        }

        public CalendarDialog(DateTime curDate) : this()
        {
            selectionCalendar.SelectedDate = curDate;
            selectButton.IsEnabled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            selectedDate = selectionCalendar.SelectedDate.Value;
            if ((selectedDate != null)) isClean = true;
            this.Close();
        }

        private void selectionCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            selectButton.IsEnabled = true;
        }
    }
}

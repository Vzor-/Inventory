using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Inventory.DataTypes
{
    public class Order
    {
        public string orderID { get; }
        public bool Approved { get; }
        public bool Ordered { get; }
        public bool Recived { get; }
        public string orderedDate { get; internal set; }
        public string expectedDate { get; internal set; }

        //public DatePicker orderedDateCal { get; }
        //public DatePicker expectedDateCal { get; }

        public Order(string orderID, bool approved, bool ordered, bool recived, string orderedDate, string expectedDate)
        {
            this.orderID = orderID;
            this.Approved = approved;
            this.Ordered = ordered;
            this.Recived = recived;
            this.orderedDate = orderedDate;
            this.expectedDate = expectedDate;
            //if (orderedDate != "")
            //{
            //    orderedDateCal = new DatePicker
            //    {
            //        SelectedDate = DateTime.Parse(orderedDate)
            //    };
            //}
            //else
            //{
            //    expectedDateCal = new DatePicker();
            //}
            //if (orderedDate != "")
            //{
            //    expectedDateCal = new DatePicker
            //    {
            //        SelectedDate = DateTime.Parse(expectedDate)
            //    };
            //}
            //else
            //{
            //    expectedDateCal = new DatePicker();
            //}

        }

        public void SetOrderedDate(string date) {
            this.orderedDate = date;
        }

        public void SetExpectedDate(string date)
        {
            this.expectedDate = date;
        }
    }
}

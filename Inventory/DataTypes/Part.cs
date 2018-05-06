using Inventory.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Part
    {
        public string EnglishName { get; set; }
        public string InternalID { get;}
        public bool IsEOL { get; set; }

        public Part (string EnglishName, string InternalID, bool IsEOL)
        {
            this.EnglishName = EnglishName;
            this.InternalID = InternalID;
            this.IsEOL = IsEOL;
        }

        public static Part NewItem()
        {
            Part returnPart = null;

            GeneralDialog gd = new GeneralDialog();
            gd.setTitle("Add Part");
            var name = gd.addSimpleField("Part Name");
            var id = gd.addSimpleField("Internal ID");
            var eol = gd.addCheckbox("Is part at End of Life", false);
            var button = gd.addSimpleButtons("Add");

            button.Click += delegate {
                returnPart = new Part(name.Text, id.Text, (bool)eol.IsChecked);
                gd.Close();
            };
            gd.ShowDialog();

            return returnPart;
        }
    }
}

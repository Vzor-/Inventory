using Inventory.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataTypes
{
    public class Location
    {
        public enum LocationType
        {
            LocationBuilding = 0,
            LocationShelf = 1,
            LocationBin = 2,
            LocationPartition = 3
        }

        public string Name { get; set; }
        public LocationType Type { get; set; }
        public int ID { get; }
        public int ParentID { get; set; }

        public Location(string name, LocationType type, int id, int parentID)
        {
            this.Name = name;
            this.Type = type;
            this.ID = id;
            this.ParentID = parentID;
        }

        public static Location NewItem(LocationType locationType, int parentID)
        {
            Location returnPart = null;

            GeneralDialog gd = new GeneralDialog();
            gd.setTitle("Add " + locationType);
            var name = gd.addSimpleField(locationType + " name");
            var button = gd.addSimpleButtons("Add");

            button.Click += delegate {
                returnPart = new Location(name.Text, locationType, -1, parentID);
                gd.Close();
            };
            gd.ShowDialog();

            return returnPart;
        }
    }
}

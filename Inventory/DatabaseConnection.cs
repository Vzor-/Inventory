using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Inventory.DataTypes;

namespace Inventory
{
    public static class DatabaseConnection
    {
        private static SQLiteConnection dbConnection;

        public static bool connectToDB()
        {
            try
            {
                dbConnection = new SQLiteConnection("data source=./Parts.db;Version=3;");
                dbConnection.Open();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public static void closeDB()
        {
            dbConnection.Close();
        }

        #region products
        public static bool addProduct(Product product)
        {
            return doProductCommand(product,
            "INSERT INTO Products (EnglishName, InternalID, IsActive) " +
                "VALUES (@EnglishName, @InternalID, @IsActive)");
        }

        public static bool editProduct(Product product)
        {
            return doProductCommand(product,
            "UPDATE Products SET " +
                "EnglishName = @EnglishName, " +
                "IsActive = @IsActive " +
                "WHERE InternalID = @InternalID");
        }

        public static bool delProduct(Product product)
        {
            return doProductCommand(product, 
                "DELETE FROM Products " +
                    "WHERE InternalID = @InternalID");
        }

        private static bool doProductCommand(Product product, string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, dbConnection);
            command.Parameters.AddWithValue("EnglishName", product.EnglishName);
            command.Parameters.AddWithValue("InternalID", product.InternalID);
            command.Parameters.AddWithValue("IsActive", product.IsActive);
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return false;
        }

        public static List<Product> getProducts()
        {
            List<Product> p = new List<Product>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT EnglishName, InternalID, IsActive FROM Products",
                dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                p.Add(new Product(reader.GetString(0), reader.GetString(1), reader.GetBoolean(2)));
            }
            return p;
        }
        #endregion

        #region Parts
        public static bool addPart(Part part)
        {
            return doPartCommand(part,
            "INSERT INTO Parts (EnglishName, InternalID, IsEOL) " +
                "VALUES (@EnglishName, @InternalID, @IsEOL)");
        }

        public static bool editPart(Part part)
        {
            return doPartCommand(part,
            "UPDATE Parts SET " +
                "EnglishName = @EnglishName, " +
                "IsEOL = @IsEOL " +
                "WHERE InternalID = @InternalID");
        }

        public static bool delPart(Part part)
        {
            return doPartCommand(part,
                "DELETE FROM Parts " +
                    "WHERE InternalID = @InternalID");
        }

        private static bool doPartCommand(Part part, string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, dbConnection);
            command.Parameters.AddWithValue("EnglishName", part.EnglishName);
            command.Parameters.AddWithValue("InternalID", part.InternalID);
            command.Parameters.AddWithValue("IsEOL", part.IsEOL);
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return false;
        }

        public static List<Part> getParts()
        {
            List<Part> p = new List<Part>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT EnglishName, InternalID, IsEOL FROM Parts",
                dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                p.Add(new Part(reader.GetString(0), reader.GetString(1), reader.GetBoolean(2)));
            }
            return p;
        }
        #endregion

        #region product has part
        public static bool addProductHasPart(Product Product, Part part)
        {
            SQLiteCommand command = new SQLiteCommand(
                "INSERT INTO ProductHasPart " +
                "(PartID, ProductID, Count) " +
                "VALUES (@PartID, @ProductID, @Count)",
                dbConnection);
            command.Parameters.AddWithValue("PartID", part.InternalID);
            command.Parameters.AddWithValue("ProductID", Product.InternalID);
            command.Parameters.AddWithValue("Count", 1);
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return false;
        }

        public static bool delProductHasPart(Product Product, Part part)
        {
            SQLiteCommand command = new SQLiteCommand(
                "DELETE FROM ProductHasPart " +
                "WHERE PartID = @PartID AND " +
                "ProductID = @ProductID",
                dbConnection);
            command.Parameters.AddWithValue("PartID", part.InternalID);
            command.Parameters.AddWithValue("ProductID", Product.InternalID);
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return false;
        }

        public static bool editProductHasPart(string ProductID, string partID, int Count)
        {
            SQLiteCommand command = new SQLiteCommand(
                "UPDATE ProductHasPart SET " +
                "Count = @Count " +
                "WHERE PartID = @PartID AND " +
                "ProductID = @ProductID",
                dbConnection);
            command.Parameters.AddWithValue("PartID", partID);
            command.Parameters.AddWithValue("ProductID", ProductID);
            command.Parameters.AddWithValue("Count", Count);
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return false;
        }

        public static List<ProductHasPart> getProductHasPart(string ProductID)
        {
            List<ProductHasPart> p = new List<ProductHasPart>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT Parts.EnglishName, Parts.InternalID, ProductHasPart.Count FROM ProductHasPart " +
                "INNER JOIN Parts ON Parts.InternalID = PartID " +
                "WHERE ProductID = @ProductID",
                dbConnection);
            command.Parameters.AddWithValue("ProductID", ProductID);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                p.Add(new ProductHasPart(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
            }
            return p;
        }
        #endregion

        #region locations
        public static bool addLocation(Location location)
        {
            return doLocationCommand(location,
            "INSERT INTO " + location.Type.ToString() + " (Name, ParentID) " +
                "VALUES (@Name, @ParentID)");
        }

        public static bool editLocation(Location location)
        {
            return doLocationCommand(location,
            "UPDATE " + location.Type.ToString() + " SET " +
                "Name = @Name, " +
                "ParentID = @ParentID, " +
                "WHERE ID = @ID");
        }

        public static bool delLocation(Location location)
        {
            return doLocationCommand(location,
                "DELETE FROM " + location.Type.ToString() + " " +
                    "WHERE ID = @ID");
        }

        private static bool doLocationCommand(Location location, string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, dbConnection);
            command.Parameters.AddWithValue("LocationType", location.Type.ToString());
            command.Parameters.AddWithValue("Name", location.Name);
            command.Parameters.AddWithValue("ParentID", location.ParentID);
            command.Parameters.AddWithValue("ID", location.ID);
            try
            {
                return (command.ExecuteNonQuery() > 0);
            }
            catch (Exception ex)
            {
                Console.Out.Write(ex.Message);
            }
            return false;
        }

        public static List<Location> getLocations(Location.LocationType type)
        {
            List<Location> p = new List<Location>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT name, ID, ParentID FROM " + type.ToString(),
                dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            System.Console.Out.WriteLine(command.ToString());
            while (reader.Read())
            {
                p.Add(new Location(reader.GetString(0), type, reader.GetInt32(1), reader.GetInt32(2)));
            }
            return p;
        }

        public static List<Location> getLocationsInLocation(Location.LocationType type, int parentID)
        {
            List<Location> p = new List<Location>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT name, ID, ParentID FROM " + type.ToString() + " " +
                " WHERE ParentID = @ParentID",
                dbConnection);
            command.Parameters.AddWithValue("ParentID", parentID);
            SQLiteDataReader reader = command.ExecuteReader();
            System.Console.Out.WriteLine(command.ToString());
            while (reader.Read())
            {
                p.Add(new Location(reader.GetString(0), type, reader.GetInt32(1), reader.GetInt32(2)));
            }
            return p;
        }
        #endregion
    }
}

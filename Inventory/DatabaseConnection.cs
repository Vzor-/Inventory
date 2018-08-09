using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Inventory.DataTypes;
using System.Collections;

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


        #region Recipes
        public static bool addRecipe(Recipe Recipe)
        {
            return doRecipeCommand(Recipe,
            "INSERT INTO Recipes (EnglishName, InternalID, IsActive) " +
                "VALUES (@EnglishName, @InternalID, @IsActive)");
        }

        internal static IEnumerable getStock()
        {
            List<DataTypes.Stock> p = new List<DataTypes.Stock>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT PartID, OrderID, Count, Location FROM Stock ",
                dbConnection);
            //SQLiteCommand command = new SQLiteCommand(
            //    "SELECT Parts.EnglishName, Stock.Count, Parts.InternalID, Parts.IsEOL " +
            //    "FROM Parts " +
            //    "Left JOIN Stock ON Parts.InternalID = Stock.PartID " +
            //    "ORDER BY Parts.EnglishName; ",
            //    dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                p.Add(new DataTypes.Stock(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }
            return p;
        }


        public static bool editRecipe(Recipe Recipe)
        {
            return doRecipeCommand(Recipe,
            "UPDATE Recipes SET " +
                "EnglishName = @EnglishName, " +
                "IsActive = @IsActive " +
                "WHERE InternalID = @InternalID");
        }

        public static bool delRecipe(Recipe Recipe)
        {
            return doRecipeCommand(Recipe, 
                "DELETE FROM Recipes " +
                    "WHERE InternalID = @InternalID");
        }

        private static bool doRecipeCommand(Recipe Recipe, string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, dbConnection);
            command.Parameters.AddWithValue("EnglishName", Recipe.EnglishName);
            command.Parameters.AddWithValue("InternalID", Recipe.InternalID);
            command.Parameters.AddWithValue("IsActive", Recipe.IsActive);
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


        public static List<Recipe> getRecipes()
        {
            List<Recipe> p = new List<Recipe>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT EnglishName, InternalID, IsActive FROM Recipes",
                dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                p.Add(new Recipe(reader.GetString(0), reader.GetString(1), reader.GetBoolean(2)));
            }
            return p;
        }


        public static List<DataTypes.Stock> getLocationsOfStock(DataTypes.Stock item)
        {
            List<DataTypes.Stock> s = new List<DataTypes.Stock>();
            String id = item.PartID;
            SQLiteCommand command = new SQLiteCommand(
                "SELECT PartID, OrderID, Count, Location FROM Stock " +
                "WHERE PartID = @id",
                dbConnection);
            command.Parameters.AddWithValue("@id", id);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                s.Add(new DataTypes.Stock(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3)));
            }

            return s;
        }

        #endregion

        #region Stock

        internal static void AddStock(DataTypes.Stock item)
        {
            SQLiteCommand command = new SQLiteCommand(
            "INSERT INTO Stock (PartID, OrderID, Location, Count) " +
                "VALUES (@partID, @orderID, @location, @count)",
                dbConnection);
            command.Parameters.AddWithValue("@partID", item.PartID);
            command.Parameters.AddWithValue("@orderID", item.OrderID);
            command.Parameters.AddWithValue("@location", item.Location);
            command.Parameters.AddWithValue("@count", item.Count);
            
            SQLiteDataReader reader = command.ExecuteReader();
        }

        internal static void updateStockLocation(DataTypes.Stock item)
        {
            String commandText =
            "UPDATE Stock SET " +
                "Location = @Location "+
                "WHERE PartID = @PartID AND " +
                "OrderID = @OrderID";
            SQLiteCommand command = new SQLiteCommand(commandText, dbConnection);
            command.Parameters.AddWithValue("PartID", item.PartID);
            command.Parameters.AddWithValue("OrderID", item.OrderID);
            command.Parameters.AddWithValue("Location", item.Location);

            SQLiteDataReader reader = command.ExecuteReader();
        }

        #endregion

        #region Orders

        internal static List<OrderPart> findPartsInOrder(Order order)
        {
            List<OrderPart> parts = new List<OrderPart>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT * FROM OrderHasPart " +
                "WHERE OrderID = @orderID",
                dbConnection);
            command.Parameters.AddWithValue("@orderID", order.orderID);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                parts.Add(new OrderPart(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
            }
            return parts;
        }

        internal static void updateOrder(Order item, string parameter, bool v)
        {
            // check that Order has parameter, protect from SQL injection like parameterization??
            if (item.GetType().GetProperty(parameter) != null)
            {
                String commandText = String.Format("UPDATE Orders SET {0} = @state WHERE InternalID = @InternalID", parameter);
                SQLiteCommand command = new SQLiteCommand(commandText, dbConnection);
                command.Parameters.AddWithValue("@InternalID", item.orderID);
                command.Parameters.AddWithValue("@state", v);

                SQLiteDataReader reader = command.ExecuteReader();

            }
        }

        public static List<Order> getOrders()
        {
            List<Order> orders = new List<Order>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT * FROM Orders",
                dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            //System.Console.Out.WriteLine(command.ToString());
            while (reader.Read())
            {
                orders.Add(new Order(reader.GetString(0), reader.GetBoolean(1), reader.GetBoolean(2), reader.GetBoolean(3), reader.GetString(4), reader.GetString(5)));
            }
            return orders;
        }

        public static string addOrder(Order order)
        {
            String returnMsg = "Passed";
            SQLiteCommand command = new SQLiteCommand(
            "INSERT INTO Orders (InternalID, approved, ordered, recived, orderedDate, expectedDate) " +
                "VALUES (@InternalID, @approved, @ordered, @recived, @orderedDate, @expectedDate)",
                dbConnection);
            command.Parameters.AddWithValue("@InternalID", order.orderID);
            command.Parameters.AddWithValue("@approved", order.Approved);
            command.Parameters.AddWithValue("@ordered", order.Ordered);
            command.Parameters.AddWithValue("@recived", order.Recived);
            command.Parameters.AddWithValue("@orderedDate", order.orderedDate);
            command.Parameters.AddWithValue("@expectedDate", order.expectedDate);
            try
            {
                SQLiteDataReader reader = command.ExecuteReader();

            }
            catch (SQLiteException ex)
            {
                returnMsg = ex.Message;
                if (ex.Message.Contains("UniqueConstraint"))
                {
                    //returnMsg = "UniqueConstraintError";
                }
                //throw new UniqueConstraintException();

            }
            finally
            {

            }

            return returnMsg;
        }

        internal static void addPartsToOrder(Order order, List<DataTypes.OrderPart> pendingStockOrder)
        {
            foreach (var item in pendingStockOrder)
            {
                SQLiteCommand command = new SQLiteCommand(
                "INSERT INTO OrderHasPart (OrderID, PartID, Count) " +
                    "VALUES (@OrderID, @PartID, @Count)",
                    dbConnection);
                command.Parameters.AddWithValue("@OrderID", order.orderID);
                command.Parameters.AddWithValue("@PartID", item.PartID);
                command.Parameters.AddWithValue("@Count", item.Count);
                SQLiteDataReader reader = command.ExecuteReader();

            }


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

        #region Recipe has part
        public static bool addRecipeHasPart(Recipe Recipe, Part part)
        {
            SQLiteCommand command = new SQLiteCommand(
                "INSERT INTO RecipeHasPart " +
                "(PartID, RecipeID, Count) " +
                "VALUES (@PartID, @RecipeID, @Count)",
                dbConnection);
            command.Parameters.AddWithValue("PartID", part.InternalID);
            command.Parameters.AddWithValue("RecipeID", Recipe.InternalID);
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

        public static bool delRecipeHasPart(Recipe Recipe, Part part)
        {
            SQLiteCommand command = new SQLiteCommand(
                "DELETE FROM RecipeHasPart " +
                "WHERE PartID = @PartID AND " +
                "RecipeID = @RecipeID",
                dbConnection);
            command.Parameters.AddWithValue("PartID", part.InternalID);
            command.Parameters.AddWithValue("RecipeID", Recipe.InternalID);
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

        public static bool editRecipeHasPart(string RecipeID, string partID, int Count)
        {
            SQLiteCommand command = new SQLiteCommand(
                "UPDATE RecipeHasPart SET " +
                "Count = @Count " +
                "WHERE PartID = @PartID AND " +
                "RecipeID = @RecipeID",
                dbConnection);
            command.Parameters.AddWithValue("PartID", partID);
            command.Parameters.AddWithValue("RecipeID", RecipeID);
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

        public static List<RecipeHasPart> getRecipeHasPart(string RecipeID)
        {
            List<RecipeHasPart> p = new List<RecipeHasPart>();
            SQLiteCommand command = new SQLiteCommand(
                "SELECT Parts.EnglishName, Parts.InternalID, RecipeHasPart.Count FROM RecipeHasPart " +
                "INNER JOIN Parts ON Parts.InternalID = PartID " +
                "WHERE RecipeID = @RecipeID",
                dbConnection);
            command.Parameters.AddWithValue("RecipeID", RecipeID);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                p.Add(new RecipeHasPart(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
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

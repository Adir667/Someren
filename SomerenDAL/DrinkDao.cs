using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using SomerenModel;
using System.Configuration;

namespace SomerenDAL
{
    public class DrinkDao : BaseDao
    {
        public List<Drink> NoSpecial()
        {
            string query = "SELECT drinkID, drinkName, price, stock, alcoholic, sold " +
                       " FROM Drink " +
                       "WHERE stock > 1 AND price > 1" +
                       "AND drinkName <> 'Water' AND drinkName <> 'Orangade' AND drinkName <> 'Cherry juice'" +
                       "ORDER BY stock DESC, price DESC, sold DESC";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public void Delete(Drink d)
        {
            string query = "DELETE FROM Drink WHERE drinkId = @Id";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Id", d.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void Update(Drink d)
        {
            string query = "UPDATE Drink " +
                "SET drinkName = @Name, stock = @Stock, sold = @Sold, price = @Price " +
                "WHERE drinkId = @Id; ";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Name", d.Name),
                new SqlParameter("Stock", d.Stock),
                new SqlParameter("Price", d.Price),
                new SqlParameter("Sold", d.Sold),
                new SqlParameter("Id", d.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void Add(Drink d)
        {
            string query = "INSERT INTO Drink (drinkName, price, stock, alcoholic) " +
                           "VALUES(@Name, @Price, @Stock, @Alcoholic) " +
                           "SELECT SCOPE_IDENTITY();";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Name", d.Name),
                new SqlParameter("Price", d.Price),
                new SqlParameter("Stock", d.Stock),
                new SqlParameter("Alcoholic", d.Alcoholic),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public int CheckExistingName(string drinkName)
        {
            string query = "SELECT COUNT (drinkName) AS [result] FROM Drink WHERE drinkName = @Name";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@Name", drinkName),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Drink> ReadTables(DataTable dataTable)
        {
            List<Drink> drinks = new List<Drink>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Drink drink = new Drink()
                {
                    Number = (int)dr["drinkId"],
                    Name = (string)(dr["drinkName"].ToString()),
                    Price = (int)dr["price"],
                    Stock = (int)dr["stock"],
                    Sold = (int)dr["Sold"],
                    Alcoholic = (bool)dr["alcoholic"],
                };
                drinks.Add(drink);
            }
            return drinks;
        }

    }
}

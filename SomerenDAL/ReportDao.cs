using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using SomerenModel;

namespace SomerenDAL
{
    public class ReportDao : BaseDao
    {
        public void UpdateOrder(Student s, Drink d, int revenue)
        {
            string query = "INSERT INTO Report ( drinkID, studentID, [date], revenue) " +
                "VALUES( @DrinkID, @StudentID, @Time, @Revenue)";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@DrinkID", d.Number),
                new SqlParameter("@StudentID", s.Number),
                new SqlParameter("@Time", DateTime.Now),
                new SqlParameter("@Revenue", revenue),
            };
            ExecuteEditQuery(query, sqlParameters);
        }

        public int Units(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT COUNT (drinkID) AS result " +
                       " FROM Report " +
                        "WHERE [date] BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.AddDays(1).ToString("yyyy-MM-dd") + "'";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        public int Revenue(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT COALESCE(SUM(revenue), 0) AS result " +
                       " FROM Report " +
                        "WHERE [date] BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.AddDays(1).ToString("yyyy-MM-dd") + "'";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        public int Students(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT COUNT (DISTINCT studentID) AS result " +
                       " FROM Report " +
                        "WHERE [date] BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.AddDays(1).ToString("yyyy-MM-dd") + "'";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

    }
}

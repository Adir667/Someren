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
    public class UserDao : BaseDao
    {

        public User GetByName(string username)
        {
            string query = "SELECT userID, username, hashedpassword, salt, secretQuestion, answer " +
                "FROM Users " +
                "WHERE username = @Username; ";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Username", username),
                };
            return ReadUser(ExecuteSelectQuery(query, sqlParameters));
        }

        public void Add(User u)
        {
            string query = "INSERT INTO Users (username, hashedpassword, salt, secretQuestion, answer) " +
                           "VALUES(@Username, @Password, @Salt, @Question, @Answer) " +
                           "SELECT SCOPE_IDENTITY();";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Username", u.Username),
                new SqlParameter("Password", u.Password),
                new SqlParameter("Salt", u.Salt),
                new SqlParameter("Question", u.Question),
                new SqlParameter("Answer", u.Answer),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public int CheckExistingName(string username)
        {
            string query = "SELECT COUNT (username) AS [result] FROM Users WHERE username = @Username";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@Username", username),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        private User ReadUser(DataTable dataTable)
        {
            User user = new User();
            foreach (DataRow dr in dataTable.Rows)
            {
                {
                    user.Number = (int)dr["userId"];
                    user.Username = (string)(dr["username"].ToString());
                    user.Password = (string)dr["hashedpassword"];
                    user.Salt = (string)dr["salt"];
                    user.Question = (string)dr["secretQuestion"];
                    user.Answer = (string)dr["answer"];
                };
            }
            return user;
        }

        public void UpdatePassword(User u)
        {
            string query = "UPDATE Users SET hashedPassword = @Password, salt = @Salt WHERE username = @Username; ";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Password", u.Password),
                new SqlParameter("Salt", u.Salt),
                new SqlParameter("Username", u.Username),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

    }
}

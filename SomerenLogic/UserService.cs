using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenLogic
{
    public class UserService

    {
        UserDao userdb;

        public UserService()
        {
            userdb = new UserDao();
        }
        public void AddUser (User u)
        {
            userdb.Add(u);
        }

        public int CheckExistingUser (string username)
        {
            return userdb.CheckExistingName(username);
        }

        public void UpdatePassword (User u)
        {
            userdb.UpdatePassword(u);
        }

        public User GetByName (string username)
        {
            return userdb.GetByName(username);
        }
    }
}

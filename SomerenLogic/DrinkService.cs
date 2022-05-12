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
    public class DrinkService
    {
        DrinkDao drinkdb;

        public DrinkService()
        {
            drinkdb = new DrinkDao();
        }

        public List<Drink> GetNotSpecialDrinks()
        {
            return drinkdb.NoSpecial();
        }

        public void DeleteDrink (Drink d)
        {
            drinkdb.Delete(d);
        }

        public void UpdateDrink(Drink d)
        {
            drinkdb.Update(d);
        }

        public void AddDrink(Drink d)
        {
            drinkdb.Add(d);
        }

        public int CheckExistingName (string drinkName)
        {
            return drinkdb.CheckExistingName(drinkName);
        }
    }
}

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
    public class RoomDao : BaseDao
    {
        public List<Room> GetAll()
        {
            string query = "SELECT roomId, nrOfBeds, type FROM [Room]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Room> ReadTables(DataTable dataTable)
        {
            List<Room> rooms = new List<Room>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Room room = new Room()
                {
                    Number = (int)dr["roomId"],
                    Capacity = (int)(dr["nrOfBeds"]),
                    Type = (bool)(dr["type"]),
                };
                rooms.Add(room);
            }
            return rooms;
        }
    }
}

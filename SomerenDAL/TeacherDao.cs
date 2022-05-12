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
    public class TeacherDao : BaseDao
    {
        public List<Teacher> GetAll()
        {
            string query = "SELECT teacherId, firstName, lastName, supervisor FROM [Teacher]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Teacher> ReadTables(DataTable dataTable)
        {
            List<Teacher> teachers = new List<Teacher>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Teacher teacher = new Teacher()
                {
                    Number = (int)dr["teacherId"],
                    FirstName = (string)(dr["firstName"].ToString()),
                    LastName = (string)(dr["lastName"].ToString()),
                    Supervisor = (bool)dr["supervisor"]
                };
                teachers.Add(teacher);
            }
            return teachers;
        }

        public void IsSupervisor(Teacher t)
        {
            string query = "UPDATE Teacher SET supervisor = @Supervisor WHERE teacherID = @Id; ";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Supervisor", t.Supervisor),
                new SqlParameter("Id", t.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public int RemainsSupervisor(Teacher t)
        {
            string query = "SELECT COUNT (lecturerID) AS [result] FROM ActivitySupervisor WHERE lecturerID = @Id";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@Id", t.Number),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

    }
}

using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;


namespace SomerenDAL
{
    public class ActivityDao : BaseDao
    {
        public List<Activity> GetAll()
        {
            string query = " SELECT activityID, activityName, activityDate" +
                           " FROM Activity ";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Activity> ReadTables(DataTable dataTable)
        {
            List<Activity> activities = new List<Activity>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Activity activity = new Activity()
                {
                    Number = (int)dr["activityID"],
                    Name = (string)(dr["activityName"]),
                    Date = (DateTime)(dr["activityDate"]),
                };
                activities.Add(activity);
            }
            return activities;
        }

        public void Delete(Activity a)
        {
            string query = "DELETE FROM Activity WHERE activityId = @Id";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Id", a.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void Update(Activity a, DateTime d)
        {
            string query = "UPDATE Activity " +
                "SET activityName = @Name, activityDate = '" + d.ToString("yyyy-MM-dd") + "' " +
                "WHERE activityId = @Id; ";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Name", a.Name),
                new SqlParameter("Id", a.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void Add(Activity a)
        {
            string query = "INSERT INTO Activity (activityName, activityDate) " +
                           "VALUES (@Name, '" + a.Date.ToString("yyyy-MM-dd") + "')" +
                           "SELECT SCOPE_IDENTITY();";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("Name", a.Name),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public int CheckExistingActivityName(string activityName)
        {
            string query = "SELECT COUNT (activityName) AS [result] FROM Activity WHERE activityName = @Name";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("@Name", activityName),
                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        public int CheckExistingSupervisor(Activity a, Teacher t)
        {
            string query = "SELECT COUNT (lecturerId) AS [result] FROM ActivitySupervisor WHERE activityID = @ActivityID AND lecturerId = @LecturerID";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("ActivityID", a.Number),
                new SqlParameter("LecturerID", t.Number),

                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        public int CheckExistingParticipant(Activity a, Student s)
        {
            string query = "SELECT COUNT (studentId) AS [result] FROM ActivityStudent WHERE activityID = @ActivityID AND studentID = @StudentID";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("ActivityID", a.Number),
                new SqlParameter("StudentID", s.Number),

                };
            return ReadCountData(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Student> ReadParticipants(DataTable dataTable)
        {
            List<Student> students = new List<Student>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Student student = new Student()
                {
                    Number = (int)dr["studentID"],
                    FirstName = (string)(dr["FirstName"]),
                    LastName = (string)(dr["LastName"]),
                };
                students.Add(student);
            }
            return students;
        }

        private List<Teacher> ReadSupervisors(DataTable dataTable)
        {
            List<Teacher> teachers = new List<Teacher>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Teacher teacher = new Teacher()
                {
                    Number = (int)dr["teacherID"],
                    FirstName = (string)(dr["FirstName"]),
                    LastName = (string)(dr["LastName"]),
                };
                teachers.Add(teacher);
            }
            return teachers;
        }

        public List<Student> ShowParticipants(Activity a)
        {
            string query = "SELECT Student.StudentID, firstName, lastName FROM Student JOIN ActivityStudent AS A ON Student.studentID = A.studentID WHERE activityID = @ID";

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("ID", a.Number),
                };
            return ReadParticipants(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Teacher> ShowSupervisors(Activity a)
        {
            string query = "SELECT Teacher.teacherID, firstName, lastName FROM Teacher JOIN ActivitySupervisor AS A ON Teacher.teacherID = A.lecturerID WHERE activityID = @ID";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("ID", a.Number),
                };
            return ReadSupervisors(ExecuteSelectQuery(query, sqlParameters));
        }

        public void AddSupervisor(Activity a, Teacher t)
        {
            string query = "INSERT INTO ActivitySupervisor (lecturerID, activityID) " +
               "VALUES (@LecturerID, @ActivityID);";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("LecturerID", t.Number),
                new SqlParameter("ActivityID", a.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void AddParticipant(Activity a, Student s)
        {
            string query = "INSERT INTO ActivityStudent (studentID, activityID) " +
               "VALUES (@StudentID, @ActivityID);";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("StudentID", s.Number),
                new SqlParameter("ActivityID", a.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void DeleteSupervisor(Activity a, Teacher t)
        {
            string query = "DELETE FROM ActivitySupervisor " +
            "WHERE LecturerID = @LecturerID AND ActivityID = @ActivityID;";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("LecturerID", t.Number),
                new SqlParameter("ActivityID", a.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

        public void DeleteParticipant(Activity a, Student s)
        {
            string query = "DELETE FROM ActivityStudent " +
            "WHERE studentID = @StudentID AND ActivityID = @ActivityID;";
            SqlParameter[] sqlParameters =
            {
                new SqlParameter("StudentID", s.Number),
                new SqlParameter("ActivityID", a.Number),
                };
            ExecuteEditQuery(query, sqlParameters);
        }

    }
}

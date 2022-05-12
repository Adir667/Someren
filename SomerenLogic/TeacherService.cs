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
    public class TeacherService
    {
        TeacherDao teacherdb;

        public TeacherService()
        {
            teacherdb = new TeacherDao();
        }

        public List<Teacher> GetTeachers()
        {
            return teacherdb.GetAll();
        }

        public void IsSupervisor (Teacher t)
        {
            teacherdb.IsSupervisor(t);
        }

        public int RemainsSupervisor(Teacher t)
        {
            return teacherdb.RemainsSupervisor(t);
        }
    }
}

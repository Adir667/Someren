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
    public class ActivityService
    {
        ActivityDao activitydb;

        public ActivityService()
        {
            activitydb = new ActivityDao();
        }

        public List<Activity> GetAll()
        {
            return activitydb.GetAll();
        }

        public void DeleteActivity(Activity a)
        {
            activitydb.Delete(a);
        }

        public void UpdateActivity(Activity a, DateTime d)
        {
            activitydb.Update(a, d);
        }

        public void AddActivity(Activity a)
        {
            activitydb.Add(a);
        }

        public int CheckExistingName(string activityName)
        {
            return activitydb.CheckExistingActivityName(activityName);
        }

        public int CheckExistingSupervisor(Activity a, Teacher t)
        {
            return activitydb.CheckExistingSupervisor(a, t);
        }

        public int CheckExistingParticipant(Activity a, Student s)
        {
            return activitydb.CheckExistingParticipant(a, s);
        }

        public List<Teacher> Supervisors(Activity a)
        {
            return activitydb.ShowSupervisors(a);
        }

        public List<Student> Participants(Activity a)
        {
            return activitydb.ShowParticipants(a);
        }

        public void AddSupervisor (Activity a, Teacher t)
        {
            activitydb.AddSupervisor(a, t);
        }

        public void AddParticipant(Activity a, Student s)
        {
            activitydb.AddParticipant(a, s);
        }

        public void DeleteSupervisor(Activity a, Teacher t)
        {
            activitydb.DeleteSupervisor(a, t);
        }

        public void DeleteParticipant(Activity a, Student s)
        {
            activitydb.DeleteParticipant(a, s);
        }
    }
}

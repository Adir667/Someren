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
    public class ReportService
    {
        ReportDao reportdb;

        public ReportService()
        {
            reportdb = new ReportDao();
        }

        public void UpdateOrders(Drink d, Student s, int revenue)
        {
            reportdb.UpdateOrder(s, d, revenue);
        }

        public int UnitsSold (DateTime startDate, DateTime endDate)
        {
            return reportdb.Units(startDate, endDate);
        }

        public int Revenue(DateTime startDate, DateTime endDate)
        {
            return reportdb.Revenue(startDate, endDate);
        }

        public int Students(DateTime startDate, DateTime endDate)
        {
            return reportdb.Students(startDate, endDate);
        }
    }
}

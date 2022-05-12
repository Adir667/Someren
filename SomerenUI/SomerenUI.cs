using SomerenLogic;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SomerenUI
{
    public partial class SomerenUI : Form
    {
        public SomerenUI()
        {
            InitializeComponent();
        }

        #region Service

        DrinkService drinkService = new DrinkService();
        StudentService studentService = new StudentService();
        TeacherService teacherService = new TeacherService();
        ReportService reportService = new ReportService();
        RoomService roomService = new RoomService();
        ActivityService activityService = new ActivityService();

        #endregion

        private void SomerenUI_Load(object sender, EventArgs e)
        {
            showPanel("Dashboard");
        }

        private void showPanel(string panelName)
        {
            HideAllPanels();

            if (panelName == "Dashboard")
            {
                pnlDashboard.Show();
                imgDashboard.Show();
            }
            else if (panelName == "Students")
            {
                pnlStudents.Show();
                ShowStudentsList(listViewStudents);
            }
            else if (panelName == "Lecturers")
            {
                pnlLecturers.Show();
                ShowAllTeachers();
            }
            else if (panelName == "Rooms")
            {
                pnlRooms.Show();
                ShowRoomsList();
            }
            else if (panelName == "Supplies")
            {
                pnlDrinksSupplies.Show();
                ShowDrinksListSupplies();

                //Edit buttons are disabled untill drink is selected

                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                rdoBtnAlcoholic.Checked = false;
            }
            else if (panelName == "Register")
            {
                pnlRegister.Show();
                ShowRegister();
            }
            else if (panelName == "Report")
            {
                pnlReport.Show();

                // show today's report as default

                DateTime today = DateTime.Now;
                lblShowStartDate.Text = today.ToShortDateString();
                lblShowEndDate.Text = today.ToShortDateString();
                ShowRevenueReport(today, today);
            }
            else if (panelName == "Activities list")
            {
                pnlActivities.Show();

                //Edit buttons are disabled untill activity is selected
                btnDeleteActivity.Enabled = false;
                btnUpdateActivity.Enabled = false;

                ShowActivitiesList(listViewActivitiesList);
            }
            else if (panelName == "Supervisors")
            {
                pnlSupervisors.Show();

                //Edit buttons are disabled untill activity is selected

                btnDeleteSupervisor.Enabled = false;
                btnAddSupervisor.Enabled = false;
                List<Teacher> teacherList = teacherService.GetTeachers();

                comLecturers.Items.Clear(); //Load Combobox for teachers
                foreach (Teacher t in teacherList)
                {
                    comLecturers.Items.Add(t);
                }
                ShowActivitiesList(listViewActivitiesSupervisors);
            }
            else if (panelName == "Participants")
            {
                pnlParticipants.Show();

                //Edit buttons are disabled untill activity is selected
                btnDeleteStudent.Enabled = false;
                btnAddStudent.Enabled = false;
                List<Student> studentsList = studentService.GetStudents();

                comStudents.Items.Clear(); //Load Combobox for students
                foreach (Student s in studentsList)
                {
                    comStudents.Items.Add(s);
                }
                ShowActivitiesList(listViewActivitiesParticipants);
            }
        }

        #region StripMune
        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dashboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            showPanel("Dashboard");
        }

        private void imgDashboard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("What happens in Someren, stays in Someren!");
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Students");
        }

        private void lecturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Lecturers");
        }

        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Rooms");
        }

        private void suppliesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Supplies");
        }

        private void cashierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Register");
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Report");
        }

        private void activityListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Activities list");
        }

        private void supervisorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Supervisors");
        }
        private void participantsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Participants");
        }

        #endregion

        #region Students

        private void ShowStudentsList(ListView list)
        {
            try
            {
                List<Student> studentsList = studentService.GetStudents();
                list.Items.Clear();

                foreach (Student s in studentsList)
                {
                    ListViewItem li = new ListViewItem(s.Number.ToString());
                    li.SubItems.Add(s.FirstName);
                    li.SubItems.Add(s.LastName);
                    li.Tag = s;
                    list.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the students");
            }
        }

        private void ShowParticipants(Activity activity)
        {
            try
            {
                List<Student> participants = activityService.Participants(activity);
                listViewParticipantsActivities.Items.Clear();

                foreach (Student s in participants)
                {
                    ListViewItem li = new ListViewItem(s.Number.ToString());
                    li.SubItems.Add(s.FirstName);
                    li.SubItems.Add(s.LastName);
                    li.Tag = s;
                    listViewParticipantsActivities.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the participants");
            }
        }

        #endregion

        #region Teachers

        private void ShowAllTeachers()
        {
            try
            {
                List<Teacher> teacherList = teacherService.GetTeachers();
                listViewLecturers.Items.Clear();

                foreach (Teacher t in teacherList)
                {
                    ListViewItem li = new ListViewItem(t.Number.ToString());
                    li.SubItems.Add(t.FirstName);
                    li.SubItems.Add(t.LastName);
                    if (t.Supervisor)
                    {
                        li.SubItems.Add("Yes");
                    }
                    else
                    {
                        li.SubItems.Add("No");
                    }
                    listViewLecturers.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the teachers");
            }
        }

        private void ShowSupervisors(Activity activity)
        {
            try
            {
                List<Teacher> supervisors = activityService.Supervisors(activity);
                listViewSuprvisorsActivities.Items.Clear();

                foreach (Teacher t in supervisors)
                {
                    ListViewItem li = new ListViewItem(t.Number.ToString());
                    li.SubItems.Add(t.FirstName);
                    li.SubItems.Add(t.LastName);
                    li.Tag = t;
                    listViewSuprvisorsActivities.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the supervisors");
            }
        }

        #endregion

        #region Bar

        private void ShowDrinksListSupplies()
        {
            try
            {
                List<Drink> drinksList = drinkService.GetNotSpecialDrinks();
                listViewDrinks.Items.Clear();

                foreach (Drink d in drinksList)
                {
                    ListViewItem li = new ListViewItem(d.Name);
                    if (d.Stock < 10)
                    {
                        li.SubItems.Add($"{d.Stock} ⚠️");
                    }
                    else
                    {
                        li.SubItems.Add($"{d.Stock} ✔️");
                    }
                    li.SubItems.Add(d.Price.ToString());
                    li.Tag = d;
                    listViewDrinks.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the drinks");
            }
        }

        private void ShowDrinksListRegister()
        {
            try
            {
                List<Drink> drinksList = drinkService.GetNotSpecialDrinks();
                listViewRegisterDrink.Items.Clear();

                foreach (Drink d in drinksList)
                {
                    ListViewItem li = new ListViewItem(d.Number.ToString());
                    li.SubItems.Add(d.Name);
                    li.SubItems.Add(d.Price.ToString());
                    li.SubItems.Add(d.Stock.ToString());
                    li.Tag = d;
                    listViewRegisterDrink.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the drinks");
            }
        }

        private void listViewDrinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = (listViewDrinks.SelectedItems.Count > 0);
            btnUpdate.Enabled = (listViewDrinks.SelectedItems.Count > 0);

            if (listViewDrinks.SelectedItems.Count > 0)
            {
                Drink d = (Drink)listViewDrinks.SelectedItems[0].Tag;
                txtName.Text = d.Name;
                txtStock.Text = d.Stock.ToString();
                txtPrice.Text = d.Price.ToString();
                rdoBtnAlcoholic.Checked = d.Alcoholic;
            }
            else
            {
                txtName.Text = "";
                txtStock.Text = "";
                txtPrice.Text = "";
                rdoBtnAlcoholic.Checked = false;
            }
        }

        private void ShowRegister()
        {
            try
            {
                lblAmountResult.Text = "0.00";
                ShowDrinksListRegister();
                ShowStudentsList(listViewRegisterStudent);
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the cash register");
            }
        }

        private void ShowRevenueReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                int sold = reportService.UnitsSold(startDate, endDate);
                int revunue = reportService.Revenue(startDate, endDate);
                int students = reportService.Students(startDate, endDate);

                listViewReport.Items.Clear();

                ListViewItem li = new ListViewItem(sold.ToString());
                li.SubItems.Add(revunue.ToString());
                li.SubItems.Add(students.ToString());
                li.SubItems.Add(startDate.ToShortDateString());
                li.SubItems.Add(endDate.ToShortDateString());
                listViewReport.Items.Add(li);
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while generating the report");
            }
        }

        private void listViewRegisterDrink_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Drink> order = new List<Drink>();
            for (int i = 0; i < listViewRegisterDrink.SelectedItems.Count; i++)
            {
                Drink d = new Drink();
                d = (Drink)listViewRegisterDrink.SelectedItems[i].Tag;
                order.Add(d);
            }
            int pay = 0;
            foreach (Drink d in order)
            {
                pay += d.Price;
            }
            lblAmountResult.Text = pay.ToString("0.00");
        }

        private void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            lblShowStartDate.Text = calStart.SelectionStart.Date.ToShortDateString();
        }

        private void calEnd_DateChanged(object sender, DateRangeEventArgs e)
        {
            lblShowEndDate.Text = calEnd.SelectionStart.Date.ToShortDateString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewDrinks.SelectedItems.Count > 0)
                {
                    Drink d = (Drink)listViewDrinks.SelectedItems[0].Tag;
                    drinkService.DeleteDrink(d);
                    MessageBox.Show($"Drink {d.Name} was sucssesfully removed", "Changes saved");
                    ShowDrinksListSupplies();
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while deleting a drink");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewDrinks.SelectedItems.Count > 0)
                {
                    Drink d = (Drink)listViewDrinks.SelectedItems[0].Tag;
                    d.Name = txtName.Text;
                    d.Stock = int.Parse(txtStock.Text);
                    d.Price = int.Parse(txtPrice.Text);
                    drinkService.UpdateDrink(d);
                    MessageBox.Show($"Drink {d.Name} was updated sucssesfully", "Changes saved");
                    ShowDrinksListSupplies();
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while updating a drink");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtStock.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Please fill in all fields to add a drink", "Drink was not added");
                    return;
                }

                int checkExist = drinkService.CheckExistingName(txtName.Text);
                if (checkExist != 0)
                {
                    MessageBox.Show($"Drink {txtName.Text} already exists", "Drink name error");
                    return;
                }
                else
                {
                    Drink d = new Drink();
                    d.Name = txtName.Text;
                    d.Stock = int.Parse(txtStock.Text);
                    d.Price = int.Parse(txtPrice.Text);
                    d.Alcoholic = (rdoBtnAlcoholic.Checked);
                    drinkService.AddDrink(d);
                    MessageBox.Show($"Drink {d.Name} was added sucssesfully", "Changes saved");
                    ShowDrinksListSupplies();
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while adding a drink");
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                if ((listViewRegisterStudent.SelectedItems.Count == 0) || listViewRegisterDrink.SelectedItems.Count == 0)
                {
                    MessageBox.Show($"Please select at least one drink from the list, and also one student", "Order was not completed");
                    return;
                }

                Student s = (Student)listViewRegisterStudent.SelectedItems[0].Tag;

                List<Drink> order = new List<Drink>();
                for (int i = 0; i < listViewRegisterDrink.SelectedItems.Count; i++)
                {
                    Drink d = new Drink();
                    d = (Drink)listViewRegisterDrink.SelectedItems[i].Tag;
                    order.Add(d);
                }
                int pay = 0;
                foreach (Drink d in order)
                {
                    pay += d.Price;
                    d.Stock--;
                    d.Sold++;
                    drinkService.UpdateDrink(d);
                    reportService.UpdateOrders(d, s, d.Price);
                }
                MessageBox.Show($"Total amount: {pay.ToString("0.00")}\nStudent: {s.FirstName}", "Oreder completed suceesfully");
                ShowRegister();
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while making an order");
            }
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = calStart.SelectionRange.Start;
                DateTime endDate = calEnd.SelectionRange.Start;

                if (endDate < startDate)
                {
                    string error = $"End date {endDate:dd/MM/yy} cannot be before start date {startDate:dd/MM/yy}!";
                    MessageBox.Show(error, "Dates Error");
                    return;
                }
                else if (endDate > DateTime.Now)
                {
                    string error = $"Cannot generate report with data from the future ({endDate:dd/MM/yy})";
                    MessageBox.Show(error, "Dates Error");
                    return;
                }
                ShowRevenueReport(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the report");
            }
        }

        #endregion

        #region Rooms
        private void ShowRoomsList()
        {
            try
            {
                List<Room> roomsList = roomService.GetRooms(); ;
                listViewRooms.Items.Clear();

                foreach (Room r in roomsList)
                {
                    ListViewItem li = new ListViewItem(r.Number.ToString());
                    li.SubItems.Add(r.Capacity.ToString());
                    if (r.Type)
                    {
                        li.SubItems.Add("Lecturer's Room");
                    }
                    else
                    {
                        li.SubItems.Add("Student's Room");
                    }
                    listViewRooms.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the rooms");
            }
        }

        #endregion

        #region Activities

        // Activities list Panel

        private void ShowActivitiesList(ListView list)
        {
            try
            {
                List<Activity> activities = activityService.GetAll(); ;
                list.Items.Clear();

                foreach (Activity a in activities)
                {
                    ListViewItem li = new ListViewItem(a.Name);
                    li.SubItems.Add(a.Date.Date.ToShortDateString());
                    li.SubItems.Add(a.Date.Date.DayOfWeek.ToString());
                    li.Tag = a;
                    list.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while loading the activities");
            }
        }

        private void listViewActivitiesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteActivity.Enabled = (listViewActivitiesList.SelectedItems.Count > 0);
            btnUpdateActivity.Enabled = (listViewActivitiesList.SelectedItems.Count > 0);

            DateTime selectedFromCal = calStart.SelectionStart.Date;

            if (listViewActivitiesList.SelectedItems.Count > 0)
            {
                Activity a = (Activity)listViewActivitiesList.SelectedItems[0].Tag;
                txtActivityName.Text = a.Name;
            }

        }

        private void calActivity_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedFromCal = calActivity.SelectionStart.Date;
            lblChooseNewDate.Text = $"Selected new date: {selectedFromCal.ToShortDateString()}";
        }

        private void btnDeleteActivity_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove this activity?", "Delete an activity", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Activity a = (Activity)listViewActivitiesList.SelectedItems[0].Tag;
                    List<Teacher> supervisors = activityService.Supervisors(a);
                    foreach (Teacher t in supervisors)
                    {
                        activityService.DeleteSupervisor(a, t);

                        int nr = teacherService.RemainsSupervisor(t); // check if is still a supervisor
                        if (nr == 0)
                        {
                            t.Supervisor = false;
                            teacherService.IsSupervisor(t);
                        }
                    }
                    activityService.DeleteActivity(a);
                    MessageBox.Show($"Activity {a.Name} was sucssesfully removed", "Changes saved");
                    txtActivityName.Text = "";
                    ShowActivitiesList(listViewActivitiesList);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while deleting an activity");
            }
        }

        private void btnUpdateActivity_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewActivitiesList.SelectedItems.Count > 0)
                {
                    Activity a = (Activity)listViewActivitiesList.SelectedItems[0].Tag;
                    DateTime date = calActivity.SelectionRange.Start.Date;
                    a.Name = txtActivityName.Text;
                    activityService.UpdateActivity(a, date);
                    MessageBox.Show($"Activity {a.Name} was updated sucssesfully", "Changes saved");
                    txtActivityName.Text = "";
                    ShowActivitiesList(listViewActivitiesList);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while updating a activity");
            }
        }

        private void btnAddActivity_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtActivityName.Text == "")
                {
                    MessageBox.Show($"New activity must have a name", "Activity was not created");
                    return;
                }
                int checkExist = activityService.CheckExistingName(txtActivityName.Text);
                if (checkExist != 0)
                {
                    MessageBox.Show($"Activity {txtActivityName.Text} already exists", "Activity name error");
                    return;
                }
                Activity a = new Activity();
                a.Name = txtActivityName.Text;
                a.Date = calActivity.SelectionRange.Start;
                activityService.AddActivity(a);
                MessageBox.Show($"Activity {a.Name} on {a.Date.ToShortDateString()} was added sucssesfully", "Changes saved");
                txtActivityName.Text = "";
                ShowActivitiesList(listViewActivitiesList);
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while adding an activity");
            }
        }

        //Supervisors Panel

        private void listViewSuprvisorsActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteSupervisor.Enabled = (listViewActivitiesSupervisors.SelectedItems.Count > 0);
        }

        private void listViewActivitiesSupervisors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewActivitiesSupervisors.SelectedItems.Count > 0)
            {
                Activity a = (Activity)listViewActivitiesSupervisors.SelectedItems[0].Tag;
                ShowSupervisors(a);
            }
        }

        private void comLecturers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddSupervisor.Enabled = (listViewActivitiesSupervisors.SelectedItems.Count > 0);
        }

        private void btnAddSupervisor_Click(object sender, EventArgs e)
        {
            try
            {
                Teacher t = (Teacher)comLecturers.SelectedItem;
                Activity a = (Activity)listViewActivitiesSupervisors.SelectedItems[0].Tag;

                int checkExist = activityService.CheckExistingSupervisor(a, t);
                if (checkExist != 0)
                {
                    MessageBox.Show($"Supervisor {t.FirstName} already supervises {a.Name}", "Supervisor exists");
                    return;
                }
                else
                {
                    activityService.AddSupervisor(a, t);
                    t.Supervisor = true;
                    teacherService.IsSupervisor(t);
                    MessageBox.Show($"Supervisor {t.FirstName} was sucssesfully added to activity {a.Name}", "Changes saved");
                    ShowActivitiesList(listViewActivitiesSupervisors);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while adding a supervisor");
            }
        }

        private void btnDeleteSupervisor_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove this supervisor?", "Delete supervisor", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Teacher t = (Teacher)listViewSuprvisorsActivities.SelectedItems[0].Tag;
                    Activity a = (Activity)listViewActivitiesSupervisors.SelectedItems[0].Tag;
                    activityService.DeleteSupervisor(a, t);

                    int nr = teacherService.RemainsSupervisor(t); // check if is still a supervisor
                    if (nr == 0)
                    {
                        t.Supervisor = false;
                        teacherService.IsSupervisor(t);
                    }

                    MessageBox.Show($"Supervisor {t.FirstName} was sucssesfully removed from activity {a.Name}", "Changes saved");
                    btnDeleteSupervisor.Enabled = false;
                    listViewSuprvisorsActivities.Items.Clear();
                    ShowActivitiesList(listViewActivitiesSupervisors);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while deleting a supervisor");
            }
        }

        //Participants Panel

        private void comStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddStudent.Enabled = (listViewActivitiesParticipants.SelectedItems.Count > 0);
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            try
            {
                Student s = (Student)comStudents.SelectedItem;
                Activity a = (Activity)listViewActivitiesParticipants.SelectedItems[0].Tag;

                int checkExist = activityService.CheckExistingParticipant(a, s);
                if (checkExist != 0)
                {
                    MessageBox.Show($"Student {s.FirstName} already participates in {a.Name}", "Participant exists");
                    return;
                }
                else
                {
                    activityService.AddParticipant(a, s);
                    MessageBox.Show($"Student {s.FirstName} was sucssesfully added to activity {a.Name}", "Changes saved");
                    btnAddStudent.Enabled = false;
                    ShowActivitiesList(listViewActivitiesParticipants);
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while adding a participant");
            }
        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove this participant?", "Delete participant", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Student s = (Student)listViewParticipantsActivities.SelectedItems[0].Tag;
                    Activity a = (Activity)listViewActivitiesParticipants.SelectedItems[0].Tag;

                    activityService.DeleteParticipant(a, s);
                    MessageBox.Show($"Student {s.FirstName} was sucssesfully removed from activity {a.Name}", "Changes saved");
                    btnDeleteStudent.Enabled = false;
                    ShowActivitiesList(listViewActivitiesParticipants);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorProcess(ex, "Something went wrong while deleting a participant");
            }
        }

        private void listViewActivitiesParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewActivitiesParticipants.SelectedItems.Count > 0)
            {
                Activity a = (Activity)listViewActivitiesParticipants.SelectedItems[0].Tag;
                ShowParticipants(a);
            }
        }

        private void listViewParticipantsActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteStudent.Enabled = (listViewParticipantsActivities.SelectedItems.Count > 0);
        }

        #endregion

        private void HideAllPanels()
        {
            pnlDashboard.Hide();
            imgDashboard.Hide();
            pnlStudents.Hide();
            pnlLecturers.Hide();
            pnlDrinksSupplies.Hide();
            pnlRooms.Hide();
            pnlRegister.Hide();
            pnlReport.Hide();
            pnlActivities.Hide();
            pnlSupervisors.Hide();
            pnlParticipants.Hide();
        }

        private void ErrorProcess(Exception ex, string messege)
        {
            MessageBox.Show(messege, "Error occured");

            // write to error log file
            StreamWriter sw = File.AppendText("..\\..\\..\\Error Log.txt");
            sw.WriteLine($"Error occured at: {DateTime.Now}:");
            sw.WriteLine(messege);
            Console.WriteLine(ex);
            Console.WriteLine();
            sw.Close();
        }

    }
}

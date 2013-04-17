using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FikrPos.Modules.Dashboard
{
    public partial class AuditRollBoard : Form
    {
        int oldRow;
        int oldCol;

        public AuditRollBoard()
        {
            InitializeComponent();
            refreshSessionStatus(); 
            readData();
            
        }

        private void refreshSessionStatus()
        {
            Program.activeRoll  = getUnendedSession();
            if (Program.activeRoll == null)
            {
                lblActiveSession.Text = "No active session";
                lblSessionElapsed.Text = "N/A";
                btnStartStop.Text = "Start session";
            }
            else
            {
                lblActiveSession.Text = "Started on " + Program.activeRoll.Time_Start + " by " + Program.activeRoll.AppUser.Username;
                timer1_Tick(null, null);
                timer1.Start();
                btnStartStop.Text = "Stop session";
            }
        }

        private AuditRoll getUnendedSession()
        {
            FikrPosDataContext db = Program.getDb();
            var unendedSession = (from p in db.AuditRolls
                                  where p.Time_Ended == null
                                  select p).SingleOrDefault();
            return unendedSession;
        }

        private void refreshElapsedTime()
        {
            TimeSpan ts = DateTime.Now - Program.activeRoll.Time_Start;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
            lblSessionElapsed.Text = elapsedTime;
        }

        private void readData()
        {
            FikrPosDataContext db = Program.getDb();
            var product = from p in db.AuditRolls
                          orderby p.id descending
                          select p;

            if (dataGridView1.SelectedCells.Count > 0)
            {
                oldRow = dataGridView1.SelectedCells[0].RowIndex;
                oldCol = dataGridView1.SelectedCells[0].ColumnIndex;
            }
            else
            {
                oldRow = 0;
                oldCol = 1;
            }

            dataGridView1.DataSource = product;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[dataGridView1.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            if (oldRow >= dataGridView1.Rows.Count)
            {
                oldRow = dataGridView1.Rows.Count - 1;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows[oldRow].Cells[oldCol].Selected = true;
            }
            dataGridView1.Focus();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (Program.activeRoll == null)
            {   SessionProperty prop = new SessionProperty();
                if (prop.ShowDialog() == DialogResult.OK)
                {
                    btnStartStop.Text = "Stop Session";
                    Program.activeRoll = new AuditRoll();
                    Program.activeRoll.Started_By = Program.userLogin.ID;
                    Program.activeRoll.Time_Start = DateTime.Now;
                    Program.activeRoll.Time_Ended = null;
                    Program.activeRoll.Start_of_Cash = prop.startOfCash;
                    Program.activeRoll.End_of_Cash = prop.endOfCash;
                    Program.activeRoll.Note = prop.note;

                    FikrPosDataContext db = Program.getDb();
                    db.AuditRolls.InsertOnSubmit(Program.activeRoll);
                    db.SubmitChanges();
                    timer1.Start();
                }
            }else
            {
                FikrPosDataContext db = Program.getDb();
                Program.activeRoll = (from p in db.AuditRolls
                                     where p.id==Program.activeRoll.id
                                     select p).SingleOrDefault();
                SessionProperty prop = new SessionProperty();
                prop.startOfCash = Program.activeRoll.Start_of_Cash;
                prop.endOfCash = Program.activeRoll.End_of_Cash;
                prop.note = Program.activeRoll.Note;
                prop.prepareForm();
                if (prop.ShowDialog() == DialogResult.OK)
                {                    
                    btnStartStop.Text = "Start Session";                    
                    Program.activeRoll.Start_of_Cash = prop.startOfCash;
                    Program.activeRoll.Time_Ended = DateTime.Now;
                    Program.activeRoll.End_of_Cash = prop.endOfCash;
                    Program.activeRoll.Note = prop.note;
                    db.SubmitChanges();
                    timer1.Stop();
                }
            }
            refreshSessionStatus();
            readData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            refreshElapsedTime();
        }
    }
}

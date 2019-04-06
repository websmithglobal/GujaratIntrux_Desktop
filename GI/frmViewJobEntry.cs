using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ENT = GI.ENTITY;
using DAL = GI.DAL;

namespace GI
{
    public partial class frmViewJobEntry : Form
    {
        SqlConnection conn;
        string conStr;

        public frmViewJobEntry()
        {
            InitializeComponent();
        }

        public void GetJobEntry()
        {
            try
            {
                ENT.JobEntry objENT = new ENT.JobEntry();
                List<ENT.JobEntry> lstENT = new List<ENT.JobEntry>();
                if (checkBox1.Checked)
                {
                    objENT.Mode = "GetAllJobEntryToday";
                }
                else
                {
                    objENT.Mode = "GetAllJobEntryByDate";
                    objENT.strJobStartTime = DAL.DBHelper.ChangeDate(dtpFromDate.Text);
                    objENT.strJobEndTime = DAL.DBHelper.ChangeDate(dtpToDate.Text);
                    objENT.JobNo= txtJobNo.Text.Trim();
                }

                lstENT = new DAL.JobEntry().GetJobEntry(objENT);
                dgvSales.DataSource = lstENT;

                dgvSales.Columns[0].HeaderText = "JobID";
                dgvSales.Columns[0].Visible = false;
                dgvSales.Columns[1].HeaderText = "Job No";
                dgvSales.Columns[2].HeaderText = "Job Furnace No";
                dgvSales.Columns[3].HeaderText = "Start Time";
                dgvSales.Columns[4].HeaderText = "End Time";
                dgvSales.Columns[5].HeaderText = "Pour1 Time";
                dgvSales.Columns[6].HeaderText = "Pour2 Time";
                dgvSales.Columns[7].HeaderText = "Pour3 Time";
                dgvSales.Columns[8].HeaderText = "Pour4 Time";
                dgvSales.Columns[9].HeaderText = "Pour5 Time";
                dgvSales.Columns[10].HeaderText = "Pour6 Time";
                dgvSales.Columns[11].Visible = false;
                dgvSales.Columns[12].Visible = false;
                dgvSales.Columns[13].Visible = false;
                dgvSales.Columns[14].HeaderText = "Job Status";
                dgvSales.Columns[15].Visible = false;
                dgvSales.Columns[16].Visible = false;
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(),"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmViewData_Load(object sender, EventArgs e)
        {
            try
            {
                checkBox1.Checked = true;
                GetJobEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                GetJobEntry();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

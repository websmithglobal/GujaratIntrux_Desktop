using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GI
{
    public partial class frmViewData : Form
    {
        System.Timers.Timer objTime = new System.Timers.Timer();
        SqlConnection conn;
        string conStr;

        public frmViewData()
        {
            InitializeComponent();
            conStr = GI.Properties.Settings.Default.ConnectionString;
            conn = new SqlConnection(conStr);
        }

        private void GetCurrentUnit()
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    txtCurrUnit.Text = "";
                    DataTable dtNew = new DataTable();
                    string strqry = "SELECT TOP 1 DataValue FROM MeterSlaveMaster ORDER BY EntryDate DESC, LineCount DESC;";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = strqry;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtNew);
                    if (dtNew.Rows.Count > 0)
                    {
                        txtCurrUnit.Text = dtNew.Rows[0][0].ToString();
                    }
                    else
                    {
                        txtCurrUnit.Text = "0";
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void frmViewData_Load(object sender, EventArgs e)
        {
            try
            {
                double interval = Convert.ToDouble(GI.Properties.Settings.Default.IntervalUnit);
                GetCurrentUnit();
                objTime = new System.Timers.Timer();
                objTime.Elapsed += new System.Timers.ElapsedEventHandler(ObjTime_Elapsed);
                objTime.Interval = ((1000 * 60) * interval);
                objTime.Enabled = true;
                objTime.AutoReset = true;
                objTime.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
           
        }

        private void ObjTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    GetCurrentUnit();
                });
            }
            catch (Exception)
            {
                objTime.Start();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetCurrentUnit();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string WHERE = "";
            if (checkBox1.Checked)
            {
                WHERE = " WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) BETWEEN DATEADD(d, DATEDIFF(d, 0, '" + DateTime.Now.ToString("dd/MMM/yyyy") + "'), 0) AND DATEADD(d, DATEDIFF(d, 0, '" + DateTime.Now.ToString("dd/MMM/yyyy") + "'), 0) ";
            }
            else
            {
                WHERE = " WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) BETWEEN DATEADD(d, DATEDIFF(d, 0, '" + dtpFromDate.Value.ToString("dd/MMM/yyyy") + "'), 0) AND DATEADD(d, DATEDIFF(d, 0, '" + dtpToDate.Value.ToString("dd/MMM/yyyy") + "'), 0) ";
            }

            DataTable dtData = new DataTable();
            string strqry = "SELECT [ID],CONVERT(VARCHAR(25), DataDate,103) AS [DataDate],CONVERT(VARCHAR(25), DataTime,14) AS [DataTime],[MeterID],[SlaveID],[Address],[Quantity],[DataValue],[FinalUnit],ISNULL(([DataValue]-[DataValue2]),0) AS DataDiff,[FileName],[LineCount],[EntryDate] FROM [dbo].[MeterSlaveMaster] " + WHERE + " ORDER BY EntryDate DESC, LineCount DESC;";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strqry;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtData);
            dgvSales.DataSource = dtData;

            dgvSales.Columns[0].HeaderText = "ID";
            dgvSales.Columns[0].Visible = false;
            dgvSales.Columns[1].HeaderText = "Date";
            dgvSales.Columns[2].HeaderText = "Time";
            dgvSales.Columns[3].Visible = false;
            dgvSales.Columns[4].Visible = false;
            dgvSales.Columns[5].Visible = false;
            dgvSales.Columns[6].Visible = false;
            dgvSales.Columns[7].Visible = false;
            dgvSales.Columns[8].Visible = false;
            dgvSales.Columns[9].HeaderText = "Unit";
            dgvSales.Columns[10].HeaderText = "File Name";
            dgvSales.Columns[11].HeaderText = "Line Count";
            dgvSales.Columns[12].Visible = false;

            dgvSales.Columns[1].Width = 150;
            dgvSales.Columns[2].Width = 150;
            dgvSales.Columns[7].Width = 150;
            dgvSales.Columns[8].Width = 150;
            dgvSales.Columns[9].Width = 150;
            dgvSales.Columns[10].Width = 150;
            dgvSales.Columns[11].Width = 150;
        }
    }
}

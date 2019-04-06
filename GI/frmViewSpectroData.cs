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
    public partial class frmViewSpectroData : Form
    {
        System.Timers.Timer objTime = new System.Timers.Timer();
        SqlConnection conn;
        string conStr;

        public frmViewSpectroData()
        {
            InitializeComponent();
            conStr = GI.Properties.Settings.Default.ConnectionString;
            conn = new SqlConnection(conStr);
        }
        
        private void frmViewData_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
           
        }
        
        private void btnShow_Click(object sender, EventArgs e)
        {
            dgvSales.Columns.Clear();
            //dgvSales.DataSource = null;
            string WHERE = "";
            if (checkBox1.Checked)
            {
                WHERE = " WHERE DATEADD(d, DATEDIFF(d, 0, [SpectroDate]), 0) BETWEEN DATEADD(d, DATEDIFF(d, 0, '" + DateTime.Now.ToString("dd/MMM/yyyy") + "'), 0) AND DATEADD(d, DATEDIFF(d, 0, '" + DateTime.Now.ToString("dd/MMM/yyyy") + "'), 0) ";
            }
            else
            {
                WHERE = " WHERE DATEADD(d, DATEDIFF(d, 0, [SpectroDate]), 0) BETWEEN DATEADD(d, DATEDIFF(d, 0, '" + dtpFromDate.Value.ToString("dd/MMM/yyyy") + "'), 0) AND DATEADD(d, DATEDIFF(d, 0, '" + dtpToDate.Value.ToString("dd/MMM/yyyy") + "'), 0) ";
            }

            DataTable dtData = new DataTable();
            string strqry = "SELECT [SpectroID],[SpectroNo],CONVERT(VARCHAR(25), [SpectroDate],103) AS [SpectroDate],[Quality],[Grade],[SampleNo] FROM [dbo].[SpectroMaster] " + WHERE + " ORDER BY SpectroNo DESC;";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strqry;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtData);
            dgvSales.DataSource = dtData;

            dgvSales.Columns[0].HeaderText = "SpectroID";
            dgvSales.Columns[0].Visible = false;

            dgvSales.Columns[1].HeaderText = "Spectro No.";
            dgvSales.Columns[2].HeaderText = "Spectro Date";
            dgvSales.Columns[3].HeaderText = "Quality";
            dgvSales.Columns[4].HeaderText = "Grade";
            dgvSales.Columns[5].HeaderText = "Sample No.";

            dgvSales.Columns[1].Width = 150;
            dgvSales.Columns[2].Width = 150;
            dgvSales.Columns[3].Width = 150;
            dgvSales.Columns[4].Width = 150;
            dgvSales.Columns[5].Width = 150;

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "View Elements";
            btn.Text = "View Elements";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;
            dgvSales.Columns.Add(btn);

        }

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                frmViewSpectroElements ele = new frmViewSpectroElements(Convert.ToInt64(dgvSales.Rows[e.RowIndex].Cells["SpectroNo"].Value));
                ele.ShowDialog();
            }
        }

        private void btnFileReading_Click(object sender, EventArgs e)
        {
            frmSpectroFileRead frmSFR = new frmSpectroFileRead();
            frmSFR.Hide();
            frmSFR.Show();
        }
    }
}

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
    public partial class frmViewSpectroElements : Form
    {
        Int64 SpecNo = 0;
        SqlConnection conn;
        string conStr;

        public frmViewSpectroElements()
        {
            InitializeComponent();
            conStr = GI.Properties.Settings.Default.ConnectionString;
            conn = new SqlConnection(conStr);
        }

        public frmViewSpectroElements(Int64 SpectroNo)
        {
            InitializeComponent();
            conStr = GI.Properties.Settings.Default.ConnectionString;
            conn = new SqlConnection(conStr);
            SpecNo = SpectroNo;
        }

        private void frmViewSpectroElements_Load(object sender, EventArgs e)
        {
            DataTable dtData = new DataTable();
            string strqry = "SELECT [SpectroNo],[KeyName],[KeyValue] FROM [dbo].[SpectroEntry] WHERE [SpectroNo] = "+ SpecNo + "";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strqry;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtData);
            dgvSales.DataSource = dtData;

            dgvSales.Columns[0].HeaderText = "Spectro No.";
            dgvSales.Columns[1].HeaderText = "Key Name";
            dgvSales.Columns[2].HeaderText = "Key Value";

            dgvSales.Columns[0].Width = 150;
            dgvSales.Columns[1].Width = 150;
            dgvSales.Columns[2].Width = 150;
         
        }
    }
}

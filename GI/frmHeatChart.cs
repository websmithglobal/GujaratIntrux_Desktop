using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GI
{
    public partial class frmHeatChart : Form
    {
        public frmHeatChart()
        {
            InitializeComponent();
        }

        private void frmHeatChart_Load(object sender, EventArgs e)
        {
            string query = @"SELECT convert(varchar(20), DataTime, 114) AS DATATIME, (DataValue-DataValue2) AS UNIT FROM MeterSlaveMaster
                                    WHERE DataTime BETWEEN '26-JUL-2018 08:00:00 AM' AND '26-JUL-2018 11:59:00 PM' AND ID<>'D5093FF3-D577-4E47-8B38-DC27C1FCF8F3'
                                    ORDER BY DataTime";

            DataTable dt = GetData(query);

            chart1.DataManipulator.IsStartFromFirst = true;

            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 20;

            chart1.Series[0].LegendText = "Every 2 Min Unit Consumption";
            chart1.Series[0].BorderWidth = 1;
            chart1.Series[0].XValueType = ChartValueType.Auto;
            chart1.Series[0].Points.DataBindXY(dt.DefaultView, "DATATIME", dt.DefaultView, "UNIT");
        }

        private static DataTable GetData(string query)
        {
            string constr = GI.Properties.Settings.Default.ConnectionString.ToString(); // @"Data Source=MAITRI-ANDROIDD;Initial Catalog=GI;User ID=sa;Password=abcd";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

    }
}

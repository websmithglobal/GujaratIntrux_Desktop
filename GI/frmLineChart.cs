using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;

namespace GI
{
    public partial class frmLineChart : Form
    {
        public frmLineChart()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Fetch the Statistical data from database.
            string query = @"select top 25 * from (
                select fur_id, fur_name,convert(varchar(20), fur_entry_time, 114) AS dttime, DATEDIFF(minute, fur_open_time, fur_close_time) AS [close],
                case when DATEDIFF(minute, fur_open_time, fur_close_time)>0 then
                (select isnull(SUM(DataValue-DataValue2),0) from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time)
                else 
                (select isnull(SUM(DataValue-DataValue2),0) from MeterSlaveMaster where DataTime between fur_close_time and fur_open_time)
                end as low,0 as [open], 0 AS high
                from FurnaceSwitch where fur_name='FURNACE1' and fur_no=1 and fur_status=0 
                union
                select fur_id, fur_name,convert(varchar(20), fur_entry_time, 114) AS dttime,0 as [close], 0 AS high, DATEDIFF(minute, fur_open_time, fur_close_time) AS [open],
                case when DATEDIFF(minute, fur_open_time, fur_close_time)>0 then
                (select isnull(SUM(DataValue-DataValue2),0) from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time)
                else 
                (select isnull(SUM(DataValue-DataValue2),0) from MeterSlaveMaster where DataTime between fur_close_time and fur_open_time)
                end as high
                from FurnaceSwitch where fur_name='FURNACE1' and fur_no=1 and fur_status=1
                ) as temp order by dttime";
           
            DataTable dt = GetData(query);

            query = @"select fur_id, fur_name,convert(varchar(20), fur_entry_time, 114) AS entrytime, ABS(DATEDIFF(minute, fur_open_time, fur_close_time)) AS tottime,
                case when DATEDIFF(minute, fur_open_time, fur_close_time)>0 then
                (select isnull(SUM(DataValue-DataValue2),0) from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time)
                else 
                (select isnull(SUM(DataValue-DataValue2),0) from MeterSlaveMaster where DataTime between fur_close_time and fur_open_time)
                end as totunit
                from FurnaceSwitch where fur_name='FURNACE1' and fur_no=1 order by fur_entry_time";

            DataTable dt2 = GetData(query);

            //Get the names of Cities.
            string[] x = (from p in dt2.AsEnumerable()
                          orderby p.Field<string>("entrytime") ascending
                          select p.Field<string>("entrytime")).ToArray();

            //Get the Total of Orders for each City.
            int[] y = (from p in dt2.AsEnumerable()
                       orderby p.Field<string>("entrytime") ascending
                       select p.Field<int>("tottime")).ToArray();

            decimal[] yy = (from p in dt2.AsEnumerable()
                       orderby p.Field<string>("entrytime") ascending
                       select p.Field<decimal>("totunit")).ToArray();

            crtUnitTime.DataManipulator.IsStartFromFirst = true;
            crtUnitTime.Series["unittime"].LegendText = "Unit Consumption Chart";
            crtUnitTime.Series["unittime"].ChartType = SeriesChartType.Spline;
            crtUnitTime.Series["unittime"].BorderWidth = 1;
            crtUnitTime.Series["unittime"].XValueType = ChartValueType.Auto;
            crtUnitTime.Series["unittime"].BorderColor = System.Drawing.Color.Red;
            crtUnitTime.Series["unittime"].Points.DataBindXY(dt2.DefaultView, "entrytime", dt2.DefaultView, "totunit");
            
            Chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            Chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

            Chart1.Series[0].LegendText = "Candle Stick Chart";
            Chart1.Series[0].ChartType = SeriesChartType.Candlestick;
            Chart1.Series[0].BorderWidth = 1;
            Chart1.Series[0].CustomProperties = "PriceDownColor=Red, PriceUpColor=Green";
            Chart1.Series[0].XValueType = ChartValueType.Time;

            Chart1.ChartAreas[0].AxisY.Minimum = -50;
            Chart1.ChartAreas[0].AxisY.Maximum = 50;

            Chart1.Series[0].XValueMember = "dttime";
            Chart1.Series[0].YValueMembers = "high,low,open,close";
            Chart1.Series[0]["OpenCloseStyle"] = "Triangle";
            Chart1.Series[0]["ShowOpenClose"] = "Both";
            Chart1.DataManipulator.IsStartFromFirst = true;
            Chart1.DataSource = dt;
            Chart1.DataBind();
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

        bool view = true;
        private void frmLineChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (view)
                {
                    Chart1.Visible = false;
                    crtUnitTime.Visible = true;
                    view = false;
                }
                else
                {
                    Chart1.Visible = true;
                    crtUnitTime.Visible = false;
                    view = true;
                }
            }
        }
    }
}

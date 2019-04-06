using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = WEB.ConsoleApp.Entity;

namespace WEB.ConsoleApp.DataLayer
{
    public class HeatReport
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        /// <summary>
        /// get heat report by date for displaying mobile app
        /// </summary>
        /// <param name="date">using this format 31-MAR-2019</param>
        /// <returns></returns>
        public List<ENT.HeatFurnaceReport> GetHeatReportByDate(string date)
        {
            List<ENT.HeatFurnaceReport> lstENT = new List<ENT.HeatFurnaceReport>();
            try
            {
                //string tempQuery = @"SELECT TOP 1 *, (DataValue-DataValue2) As UnitDifference, Convert(varchar(10),ISNULL(fur_second / 3600,0))  +':'+ Convert(varchar(10),ISNULL(fur_second % 3600/60,0)) +':'+ Convert(varchar(10),ISNULL(fur_second % 60,0)) As HrsMin FROM (
                //select fur_name,fur_no,fur_status,fur_open_time,fur_close_time, DATEDIFF(SECOND, fur_open_time, fur_close_time) AS fur_second,
                //ISNULL((select DataValue from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time),0) As DataValue,
                //ISNULL((select DataValue2 from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time),0) As DataValue2,fur_entry_time
                //FROM FurnaceSwitch  
                //WHERE DATEADD(d, DATEDIFF(d, 0, fur_entry_time), 0) = '22-JAN-2019' and fur_status = 1 
                //) AS temp ORDER BY fur_entry_time DESC";

                sqlCMD = new SqlCommand();
                string strQuery = @"SELECT TOP 1 *, Convert(varchar(10),ISNULL(fur_second / 3600,0))  +':'+ Convert(varchar(10),ISNULL(fur_second % 3600/60,0)) +':'+ Convert(varchar(10),ISNULL(fur_second % 60,0)) As HrsMin FROM (
                                            select fur_name,fur_no,fur_status,fur_open_time,fur_close_time, DATEDIFF(SECOND, fur_open_time, fur_close_time) AS fur_second,
                                            ISNULL((select top 1 DataValue from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time order by DataTime desc),0) As DataValue,
                                            ISNULL((select top 1 DataValue2 from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time order by DataTime asc),0) As DataValue2,fur_entry_time,
                                            ISNULL((select SUM(DataValue-DataValue2) from MeterSlaveMaster where DataTime between fur_open_time and fur_close_time),0) As UnitDifference
                                            FROM FurnaceSwitch  
                                            WHERE DATEADD(d, DATEDIFF(d, 0, fur_entry_time), 0) = @date and fur_status = 1 
                                            ) AS temp ORDER BY fur_entry_time DESC";
                sqlCMD.CommandText = strQuery;
                sqlCMD.Parameters.AddWithValue("@date", date);
                lstENT = DBHelper.GetEntityListByQuery<ENT.HeatFurnaceReport>(sqlCMD);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
                //throw ex;
            }
            return lstENT;
        }

    }
}

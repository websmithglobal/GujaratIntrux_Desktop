using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = WEB.ConsoleApp.Entity;

namespace WEB.ConsoleApp.DataLayer
{
    public class HeatStartStopReport
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        /// <summary>
        /// get heat report by date for displaying mobile app
        /// </summary>
        /// <param name="date">using this format 31-MAR-2019</param>
        /// <returns></returns>
        public List<ENT.HeatStartStopReportApi> GetHeatStartStopReportByDate(string date, Int64 ID)
        {
            List<ENT.HeatStartStopReportApi> lstENT = new List<ENT.HeatStartStopReportApi>();
            try
            {
                sqlCMD = new SqlCommand();
                string strQuery = @"SELECT TOP 1 *, Convert(varchar(10),ISNULL(fur_second / 3600,0))  +':'+ Convert(varchar(10),ISNULL(fur_second % 3600/60,0)) +':'+ Convert(varchar(10),ISNULL(fur_second % 60,0)) As HrsMin FROM (
                                            SELECT ID,StartDataTime,EndDataTime,DATEDIFF(SECOND, StartDataTime, EndDataTime) AS fur_second,DataValue, DataValue2,(DataValue-DataValue2) AS UnitDifference 
                                            FROM HeatStartStopReport WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date and ID=@ID and isupdated = 1 and issend = 1 
                                            ) AS HeatReport ORDER BY ID DESC";
                sqlCMD.CommandText = strQuery;
                sqlCMD.Parameters.AddWithValue("@ID", ID);
                sqlCMD.Parameters.AddWithValue("@date", date);
                lstENT = DBHelper.GetEntityListByQuery<ENT.HeatStartStopReportApi>(sqlCMD);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
                //throw ex;
            }
            return lstENT;
        }

        /// <summary>
        /// get heat dryrun report by date for displaying mobile app
        /// </summary>
        /// <param name="date">using this format 31-MAR-2019</param>
        /// <returns></returns>
        public List<ENT.DryrunHeatReportApi> GetHeatDryrunReportByDate(string date, Int64 ID)
        {
            List<ENT.DryrunHeatReportApi> lstENT = new List<ENT.DryrunHeatReportApi>();
            try
            {
                sqlCMD = new SqlCommand();

                //string strQuery = @"SELECT ID,StartDataTime, EndDataTime,DATEDIFF(SECOND, StartDataTime, EndDataTime) AS fur_second,DataValue,DataValue2,(DataValue-DataValue2) as UnitDifference, 
                //                            CONVERT(VARCHAR(10),ISNULL(DATEDIFF(SECOND, StartDataTime, EndDataTime) / 3600,0))  +':'+ CONVERT(VARCHAR(10),ISNULL(DATEDIFF(SECOND, StartDataTime, EndDataTime) % 3600/60,0)) +':'+ CONVERT(VARCHAR(10),ISNULL(DATEDIFF(SECOND, StartDataTime, EndDataTime) % 60,0)) AS HrsMin
                //                            FROM (SELECT ID,
                //                            (SELECT StartDataTime FROM HeatStartStopReport WHERE ID=a.ID AND DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date) as EndDataTime,
                //                            ISNULL((SELECT EndDataTime FROM HeatStartStopReport WHERE ID=a.ID-1 AND DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date),(SELECT StartDataTime FROM HeatStartStopReport WHERE ID=a.ID AND DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date)) AS StartDataTime,
                //                            (SELECT DataValue2 FROM HeatStartStopReport WHERE ID=a.ID AND DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date) AS DataValue,
                //                            ISNULL((SELECT DataValue FROM HeatStartStopReport WHERE ID=a.ID-1 AND DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date),(SELECT DataValue2 FROM HeatStartStopReport WHERE ID=a.ID AND DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date)) AS DataValue2
                //                            FROM HeatStartStopReport AS a WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date AND ID = @ID 
                //                            ) AS temp ORDER BY ID DESC";

                string strQuery = @"SELECT TOP 1 *, Convert(varchar(10),ISNULL(fur_second / 3600,0))  +':'+ Convert(varchar(10),ISNULL(fur_second % 3600/60,0)) +':'+ Convert(varchar(10),ISNULL(fur_second % 60,0)) As HrsMin FROM (
                                            SELECT ID,StartDataTime,EndDataTime,DATEDIFF(SECOND, StartDataTime, EndDataTime) AS fur_second,DataValueLast, DataValueFirst,(DataValueLast-DataValueFirst) AS UnitDifference 
                                            FROM DryrunHeatReport WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date and ID=@ID and isupdated = 1
                                            ) AS HeatReport ORDER BY ID DESC";

                sqlCMD.CommandText = strQuery;
                sqlCMD.Parameters.AddWithValue("@ID", ID);
                sqlCMD.Parameters.AddWithValue("@date", date);
                lstENT = DBHelper.GetEntityListByQuery<ENT.DryrunHeatReportApi>(sqlCMD);
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

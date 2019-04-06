using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ENT = GI.COMPORT.ENTITY;

namespace GI.COMPORT.DAL
{
    public class HeatStartStopReport
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertHeatStartStopReport(ENT.HeatStartStopReport objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteHeatStartStopReport";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@LineCountStart", objENT.LineCountStart);
                sqlCMD.Parameters.AddWithValue("@LineCountEnd", objENT.LineCountEnd);
                sqlCMD.Parameters.AddWithValue("@StartDataTime", objENT.StartDataTime);
                sqlCMD.Parameters.AddWithValue("@EndDataTime", objENT.EndDataTime);
                sqlCMD.Parameters.AddWithValue("@DataValue", objENT.DataValue);
                sqlCMD.Parameters.AddWithValue("@DataValue2", objENT.DataValue2);
                sqlCMD.Parameters.AddWithValue("@FileName", objENT.FileName);
                sqlCMD.Parameters.AddWithValue("@EntryDate", objENT.EntryDate);
                sqlCMD.Parameters.AddWithValue("@isupdated", objENT.isupdated);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public bool UpdateHeatStartStopReport(ENT.HeatStartStopReport objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteHeatStartStopReport";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@fur_name", objENT.fur_name);
                sqlCMD.Parameters.AddWithValue("@fur_no", objENT.fur_no);
                sqlCMD.Parameters.AddWithValue("@fur_status_stop", objENT.fur_status_stop);
                sqlCMD.Parameters.AddWithValue("@fur_status_start", objENT.fur_status_start);
                sqlCMD.Parameters.AddWithValue("@fur_open_time", objENT.fur_open_time);
                sqlCMD.Parameters.AddWithValue("@fur_close_time", objENT.fur_close_time);
                sqlCMD.Parameters.AddWithValue("@LineCountEnd", objENT.LineCountEnd);
                sqlCMD.Parameters.AddWithValue("@EndDataTime", objENT.EndDataTime);
                sqlCMD.Parameters.AddWithValue("@DataValue", objENT.DataValue);
                sqlCMD.Parameters.AddWithValue("@isupdated", objENT.isupdated);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.HeatStartStopReport> GetHeatStartStopReport(ENT.HeatStartStopReport objENT)
        {
            List<ENT.HeatStartStopReport> lstENT = new List<ENT.HeatStartStopReport>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetHeatStartStopReport";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.HeatStartStopReport>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public List<ENT.HeatStartStopReportApi> GetHeatStartStopReportByDate(string date, Int64 ID)
        {
            List<ENT.HeatStartStopReportApi> lstENT = new List<ENT.HeatStartStopReportApi>();
            try
            {
                sqlCMD = new SqlCommand();
                string strQuery = @"SELECT TOP 1 *, Convert(varchar(10),ISNULL(fur_second / 3600,0))  +':'+ Convert(varchar(10),ISNULL(fur_second % 3600/60,0)) +':'+ Convert(varchar(10),ISNULL(fur_second % 60,0)) As HrsMin FROM (
                                            SELECT ID,StartDataTime,EndDataTime,DATEDIFF(SECOND, StartDataTime, EndDataTime) AS fur_second,DataValue, DataValue2,(DataValue-DataValue2) AS UnitDifference 
                                            FROM HeatStartStopReport WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date and ID=@ID and isupdated = 1 
                                            ) AS HeatReport ORDER BY ID DESC";
                sqlCMD.CommandText = strQuery;
                sqlCMD.Parameters.AddWithValue("@ID", ID);
                sqlCMD.Parameters.AddWithValue("@date", date);
                lstENT = DBHelper.GetEntityList<ENT.HeatStartStopReportApi>(sqlCMD);

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

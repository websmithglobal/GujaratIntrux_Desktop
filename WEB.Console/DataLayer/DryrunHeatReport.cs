using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = WEB.ConsoleApp.Entity;

namespace WEB.ConsoleApp.DataLayer
{
    public class DryrunHeatReport
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        /// <summary>
        /// Insert Update Dryrun report data
        /// </summary>
        /// <param name="objENT"></param>
        /// <returns></returns>
        public bool InsertUpdateDeleteDryrunHeatReport(ENT.DryrunHeatReport objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteDryrunHeatReport";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@LineCountEnd", objENT.LineCountEnd);
                sqlCMD.Parameters.AddWithValue("@EndDataTime", objENT.EndDataTime);
                sqlCMD.Parameters.AddWithValue("@DataValueLast", objENT.DataValueLast);
                sqlCMD.Parameters.AddWithValue("@EntryDate", objENT.EntryDate);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        /// <summary>
        /// get MeterSlaveMaster object by date
        /// </summary>
        /// <param name="date">using this format 31-MAR-2019</param>
        /// <returns></returns>
        public List<ENT.MeterSlaveMaster> GetMeterSlaveMasterTop1ByDate(string date)
        {
            List<ENT.MeterSlaveMaster> lstENT = new List<ENT.MeterSlaveMaster>();
            try
            {
                sqlCMD = new SqlCommand();
                string strQuery = @"SELECT TOP 1 * 
                                            FROM MeterSlaveMaster WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date 
                                            ORDER BY EntryDate DESC";
                sqlCMD.CommandText = strQuery;
                sqlCMD.Parameters.AddWithValue("@date", date);
                lstENT = DBHelper.GetEntityListByQuery<ENT.MeterSlaveMaster>(sqlCMD);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
                //throw ex;
            }
            return lstENT;
        }

        /// <summary>
        /// get dryrun heat report by date
        /// </summary>
        /// <param name="date">using this format 31-MAR-2019</param>
        /// <returns></returns>
        public List<ENT.DryrunHeatReport> GetDryrunHeatReportTop1ByDate(string date)
        {
            List<ENT.DryrunHeatReport> lstENT = new List<ENT.DryrunHeatReport>();
            try
            {
                sqlCMD = new SqlCommand();
                string strQuery = @"SELECT TOP 1 * FROM DryrunHeatReport 
                                            WHERE DATEADD(d, DATEDIFF(d, 0, EntryDate), 0) = @date AND IsUpdated = 0
                                            ORDER BY ID DESC";
                sqlCMD.CommandText = strQuery;
                sqlCMD.Parameters.AddWithValue("@date", date);
                lstENT = DBHelper.GetEntityListByQuery<ENT.DryrunHeatReport>(sqlCMD);
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

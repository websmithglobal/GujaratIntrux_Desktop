using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Websmith.Entity;

namespace Websmith.DataLayer
{
    public class JobEntry
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertUpdateDeleteJobEntry(ENT.JobEntry objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteJobEntry";
                sqlCMD.Parameters.AddWithValue("@JobID", objENT.JobID);
                sqlCMD.Parameters.AddWithValue("@JobNo", objENT.JobNo);
                sqlCMD.Parameters.AddWithValue("@JobFurnaceNo", objENT.JobFurnaceNo);
                sqlCMD.Parameters.AddWithValue("@StartTime", objENT.StartTime);
                sqlCMD.Parameters.AddWithValue("@EndTime", objENT.EndTime);
                sqlCMD.Parameters.AddWithValue("@Pour1Time", objENT.Pour1Time);
                sqlCMD.Parameters.AddWithValue("@Pour2Time", objENT.Pour2Time);
                sqlCMD.Parameters.AddWithValue("@Pour3Time", objENT.Pour3Time);
                sqlCMD.Parameters.AddWithValue("@Pour4Time", objENT.Pour4Time);
                sqlCMD.Parameters.AddWithValue("@Pour5Time", objENT.Pour5Time);
                sqlCMD.Parameters.AddWithValue("@Pour6Time", objENT.Pour6Time);
                sqlCMD.Parameters.AddWithValue("@IsFinish", objENT.IsFinish);
                sqlCMD.Parameters.AddWithValue("@HeatFurnaceID", objENT.HeatFurnaceID);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.JobEntry> GetJobEntry(ENT.JobEntry objENT)
        {
            List<ENT.JobEntry> lstENT = new List<ENT.JobEntry>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetJobEntry";
                sqlCMD.Parameters.AddWithValue("@JobID", objENT.JobID);
                sqlCMD.Parameters.AddWithValue("@StartTime", objENT.StartTime);
                sqlCMD.Parameters.AddWithValue("@EndTime", objENT.EndTime);
                sqlCMD.Parameters.AddWithValue("@HeatFurnaceID", objENT.HeatFurnaceID);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.JobEntry>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }
        
        public DataTable GetJobEntry()
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "SELECT TOP 1 JobNo FROM JobEntry ORDER BY StartTime DESC;";
                sqlCMD = new SqlCommand(strqry);
                dt = DBHelper.GetDataTableByQuery(sqlCMD);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            return dt;
        }
    }
}

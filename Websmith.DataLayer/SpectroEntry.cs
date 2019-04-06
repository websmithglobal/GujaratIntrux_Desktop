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
    public class SpectroEntry
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();
        
        public bool InsertUpdateDeleteSpectroEntry(ENT.SpectroEntry objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteSpectroEntry";
                sqlCMD.Parameters.AddWithValue("@SpectroID", objENT.SpectroID);
                sqlCMD.Parameters.AddWithValue("@SpectroNo", objENT.SpectroNo);
                sqlCMD.Parameters.AddWithValue("@KeyName", objENT.KeyName);
                sqlCMD.Parameters.AddWithValue("@KeyValue", objENT.KeyValue);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public bool InsertUpdateDeleteSpectroMaster(ENT.SpectroMaster objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteSpectroMaster";
                sqlCMD.Parameters.AddWithValue("@SpectroID", objENT.SpectroID);
                sqlCMD.Parameters.AddWithValue("@SpectroNo", objENT.SpectroNo);
                sqlCMD.Parameters.AddWithValue("@SpectroDate", objENT.SpectroDate);
                sqlCMD.Parameters.AddWithValue("@Quality", objENT.Quality);
                sqlCMD.Parameters.AddWithValue("@Grade", objENT.Grade);
                sqlCMD.Parameters.AddWithValue("@SampleNo", objENT.SampleNo);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.SpectroEntry> GetSpectroEntry(ENT.SpectroEntry objENT)
        {
            List<ENT.SpectroEntry> lstENT = new List<ENT.SpectroEntry>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetSpectroEntry";
                sqlCMD.Parameters.AddWithValue("@SpectroID", objENT.SpectroID);
                sqlCMD.Parameters.AddWithValue("@SpectroNo", objENT.SpectroNo);
                sqlCMD.Parameters.AddWithValue("@EntryDate", objENT.EntryDate);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.SpectroEntry>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public List<ENT.SpectroMaster> GetSpectroMasterForAPI(ENT.SpectroMasterParam objENT)
        {
            List<ENT.SpectroMaster> lstENT = new List<ENT.SpectroMaster>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetSpectroDataForAPI";
                sqlCMD.Parameters.AddWithValue("@FromDate", objENT.FromDate);
                sqlCMD.Parameters.AddWithValue("@ToDate", objENT.ToDate);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.SpectroMaster>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public List<ENT.SpectroEntry> GetSpectroDetailForAPI(ENT.SpectroDetailParam objENT)
        {
            List<ENT.SpectroEntry> lstENT = new List<ENT.SpectroEntry>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetSpectroDataForAPI";

                sqlCMD.Parameters.AddWithValue("@SpectroNo", objENT.SpectroNo);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.SpectroEntry>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public Int64 GetTopOneSpectroNo()
        {
            Int64 Spectro_No = 0;
            try
            {
                string strqry = "Select TOP 1 SpectroNo From [SpectroMaster] Order By SpectroNo Desc;";
                sqlCMD = new SqlCommand(strqry);
                DataTable dt = DBHelper.GetDataTableByQuery(sqlCMD);
                if (dt.Rows.Count > 0)
                { Spectro_No = Convert.ToInt64(dt.Rows[0][0]); }
                else
                { Spectro_No = 0; }
            }
            catch (Exception ex)
            {
                Spectro_No = 0;
                throw ex;
            }
            return Spectro_No;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ENT = Websmith.Entity;

namespace Websmith.DataLayer
{
    public class MeterSlaveMaster
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertUpdateDeleteMeterSlaveMaster(ENT.MeterSlaveMaster objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteMeterSlaveMaster";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@Address", objENT.Address);
                sqlCMD.Parameters.AddWithValue("@DataDate", objENT.DataDate);
                sqlCMD.Parameters.AddWithValue("@DataValue", objENT.DataValue);
                sqlCMD.Parameters.AddWithValue("@MeterID", objENT.MeterID);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public bool InsertUpdateDeleteMeterSlaveMasterAPI(ENT.MeterSlaveMasterParam objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteMeterSlaveMaster";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@DataDate", objENT.DataDate);
                sqlCMD.Parameters.AddWithValue("@DataTime", objENT.DataTime);
                sqlCMD.Parameters.AddWithValue("@MeterID", objENT.MeterID);
                sqlCMD.Parameters.AddWithValue("@SlaveID", objENT.SlaveID);
                sqlCMD.Parameters.AddWithValue("@Address", objENT.Address);
                sqlCMD.Parameters.AddWithValue("@Quantity", objENT.Quantity);
                sqlCMD.Parameters.AddWithValue("@DataValue", objENT.DataValue);
                sqlCMD.Parameters.AddWithValue("@DataValue2", objENT.DataValue2);
                sqlCMD.Parameters.AddWithValue("@Difference", objENT.Difference);
                sqlCMD.Parameters.AddWithValue("@Value1", objENT.Value1);
                sqlCMD.Parameters.AddWithValue("@Value2", objENT.Value2);
                sqlCMD.Parameters.AddWithValue("@LineCount", objENT.LineCount);
                sqlCMD.Parameters.AddWithValue("@FinalUnit", objENT.FinalUnit);
                sqlCMD.Parameters.AddWithValue("@FileName", objENT.FileName);
                sqlCMD.Parameters.AddWithValue("@EntryDate", objENT.EntryDate);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.MeterSlaveMaster> GetMeterSlaveMaster(ENT.MeterSlaveMaster objENT)
        {
            List<ENT.MeterSlaveMaster> lstENT = new List<ENT.MeterSlaveMaster>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetMeterSlaveMaster";
                sqlCMD.Parameters.AddWithValue("@ID", objENT.ID);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.MeterSlaveMaster>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public List<ENT.MeterSlaveMaster> GetMeterSlaveMasterBySearch(ENT.SearchByDate objENT)
        {
            List<ENT.MeterSlaveMaster> lstENT = new List<ENT.MeterSlaveMaster>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetMeterSlaveMaster";
                sqlCMD.Parameters.AddWithValue("@FromDate", objENT.FromDate);
                sqlCMD.Parameters.AddWithValue("@ToDate", objENT.ToDate);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.MeterSlaveMaster>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public DataTable GetMeterSlaveMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "SELECT TOP 1 DataValue FROM MeterSlaveMaster ORDER BY EntryDate DESC;";
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

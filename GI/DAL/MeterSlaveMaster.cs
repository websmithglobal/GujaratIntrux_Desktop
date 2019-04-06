using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ENT = GI.ENTITY;

namespace GI.DAL
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

        public List<ENT.MeterSlaveMaster> GetTopOneRecord(ENT.MeterSlaveMaster objENT)
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

        public DataTable GetMeterSlaveMasterFirstRecord()
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "SELECT TOP 1 LineCount FROM MeterSlaveMaster WHERE DataDate='"+DateTime.Now.ToString("dd/MMM/yyyy")+"' ORDER BY LineCount;";
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

        public static DataTable GetDataTableFromCSVFile(string csvfilePath)
        {
            DataTable csvData = new DataTable();
            using (TextFieldParser csvReader = new TextFieldParser(csvfilePath))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;

                //Read columns from CSV file, remove this line if columns not exits  
                string[] colFields = csvReader.ReadFields();

                foreach (string column in colFields)
                {
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    csvData.Columns.Add(datecolumn);
                }

                DataColumn datecolumn1 = new DataColumn("StatusValues");
                datecolumn1.AllowDBNull = true;
                csvData.Columns.Add(datecolumn1);

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    //Making empty value as null
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }
            return csvData;
        }
    }
}

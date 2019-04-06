using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = GI.COMPORT.ENTITY;

namespace GI.COMPORT.DAL
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
                sqlCMD.Parameters.AddWithValue("@LineCountStart", objENT.LineCountStart);
                sqlCMD.Parameters.AddWithValue("@LineCountEnd", objENT.LineCountEnd);
                sqlCMD.Parameters.AddWithValue("@StartDataTime", objENT.StartDataTime);
                sqlCMD.Parameters.AddWithValue("@EndDataTime", objENT.EndDataTime);
                sqlCMD.Parameters.AddWithValue("@DataValueLast", objENT.DataValueLast);
                sqlCMD.Parameters.AddWithValue("@DataValueFirst", objENT.DataValueFirst);
                sqlCMD.Parameters.AddWithValue("@FileName", objENT.FileName);
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

    }
}

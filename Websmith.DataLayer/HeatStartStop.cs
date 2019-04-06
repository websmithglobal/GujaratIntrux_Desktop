using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Websmith.Entity;

namespace Websmith.DataLayer
{
    public class HeatStartStop
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertUpdateDeleteHeatStartStop(ENT.HeatStartStop objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteHeatStartStop";
                sqlCMD.Parameters.AddWithValue("@HeatID", objENT.HeatID);
                sqlCMD.Parameters.AddWithValue("@HeatStart", objENT.HeatStart);
                sqlCMD.Parameters.AddWithValue("@HeatStop", objENT.HeatStop);
                sqlCMD.Parameters.AddWithValue("@IsStop", objENT.IsStop);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.HeatStartStop> GetHeatStartStop(ENT.HeatStartStop objENT)
        {
            List<ENT.HeatStartStop> lstENT = new List<ENT.HeatStartStop>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetHeatStartStop";
                sqlCMD.Parameters.AddWithValue("@HeatID", objENT.HeatID);
                sqlCMD.Parameters.AddWithValue("@HeatStart", objENT.HeatStart);
                sqlCMD.Parameters.AddWithValue("@HeatStop", objENT.HeatStop);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.HeatStartStop>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }
    }
}

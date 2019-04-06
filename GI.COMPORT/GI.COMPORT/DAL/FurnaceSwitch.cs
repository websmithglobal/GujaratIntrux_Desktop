using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ENT = GI.COMPORT.ENTITY;

namespace GI.COMPORT.DAL
{
    public class FurnaceSwitch
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();
        
        public bool InsertUpdateDeleteFurnaceSwitch(ENT.FurnaceSwitch objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteFurnaceSwitch";
                sqlCMD.Parameters.AddWithValue("@fur_id", objENT.fur_id);
                sqlCMD.Parameters.AddWithValue("@fur_name", objENT.fur_name);
                sqlCMD.Parameters.AddWithValue("@fur_no", objENT.fur_no);
                sqlCMD.Parameters.AddWithValue("@fur_status", objENT.fur_status);
                sqlCMD.Parameters.AddWithValue("@fur_file_time", objENT.fur_file_time);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.FurnaceSwitch> GetFurnaceSwitch(ENT.FurnaceSwitch objENT)
        {
            List<ENT.FurnaceSwitch> lstENT = new List<ENT.FurnaceSwitch>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetFurnaceSwitch";
                sqlCMD.Parameters.AddWithValue("@fur_id", objENT.fur_id);
                sqlCMD.Parameters.AddWithValue("@fur_name", objENT.fur_name);
                sqlCMD.Parameters.AddWithValue("@fur_no", objENT.fur_no);
                sqlCMD.Parameters.AddWithValue("@fur_status", objENT.fur_status);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.FurnaceSwitch>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

    }
}

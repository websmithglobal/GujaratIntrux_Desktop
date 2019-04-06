using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GI.DAL
{
    public class SpectroMaster
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

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

        public bool InsertUpdateDeleteDeviceID(ENTITY.GI obj)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteDeviceID";
                sqlCMD.Parameters.AddWithValue("@DeviceId", obj.response);
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

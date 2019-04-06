using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Websmith.Entity;

namespace Websmith.DataLayer
{
    public class FurnaceSwitch
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertFurnaceSwitchAPI(ENT.FurnaceSwitchParam objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteFurnaceSwitchAPI";
                sqlCMD.Parameters.AddWithValue("@fur_id", objENT.fur_id);
                sqlCMD.Parameters.AddWithValue("@fur_name", objENT.fur_name);
                sqlCMD.Parameters.AddWithValue("@fur_no", objENT.fur_no);
                sqlCMD.Parameters.AddWithValue("@fur_status", objENT.fur_status);
                sqlCMD.Parameters.AddWithValue("@fur_open_time", objENT.fur_open_time);
                sqlCMD.Parameters.AddWithValue("@fur_close_time", objENT.fur_close_time);
                sqlCMD.Parameters.AddWithValue("@fur_entry_time", objENT.fur_entry_time);
                sqlCMD.Parameters.AddWithValue("@fur_file_time", objENT.fur_file_time);
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

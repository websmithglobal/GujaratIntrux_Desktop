using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Websmith.Entity;

namespace Websmith.DataLayer
{
    public class Device
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertUpdateDeleteDeviceID(ENT.Device objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteDeviceID";
                sqlCMD.Parameters.AddWithValue("@DeviceId", objENT.DeviceId);
                sqlCMD.Parameters.AddWithValue("@DeviceCode", objENT.DeviceCode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.Device> GetLastDeviceID()
        {
            List<ENT.Device> lstENT = new List<ENT.Device>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetDeviceID";
                lstENT = DBHelper.GetEntityList<ENT.Device>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public List<ENT.Device> GetAllDeviceID()
        {
            List<ENT.Device> lstENT = new List<ENT.Device>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetDeviceID";
                lstENT = DBHelper.GetEntityList<ENT.Device>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }
    }
}

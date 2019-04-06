using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENT = Websmith.Entity;

namespace Websmith.DataLayer
{
    public class UnitChartClass
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public List<ENT.UnitChartClass> GetUnitChartData(ENT.UnitChartParam objENT)
        {
            List<ENT.UnitChartClass> lstENT = new List<ENT.UnitChartClass>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetFurnaceSwitch";
                sqlCMD.Parameters.AddWithValue("@datefrom", objENT.datefrom);
                sqlCMD.Parameters.AddWithValue("@dateto", objENT.dateto);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.mode);
                lstENT = DBHelper.GetEntityList<ENT.UnitChartClass>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }
    }
}

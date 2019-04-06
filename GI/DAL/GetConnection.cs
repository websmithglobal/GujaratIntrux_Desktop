using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GI.DAL
{
    public class GetConnection
    {
        [SettingsDescription("This properties will return connection is open or not.")]
        [DefaultSettingValue("false")]
        public static Boolean isConnectionOpen { get; set; }

        private static string ReadConnectionString()
        {
            try
            {
                return GI.Properties.Settings.Default.ConnectionString.ToString();
            }
            catch
            {
                return "Data Source=MAITRI-ANDROIDD;Initial Catalog=GujaratIntrux;uid=sa; pwd=abcd;";
            }
        }

        public static SqlConnection GetDBConnection()
        {
            SqlConnection sqlcon = null;
            try
            {
                sqlcon = new SqlConnection(ReadConnectionString());
                if (sqlcon.State == ConnectionState.Closed)
                    sqlcon.Open();

                if (sqlcon.State == ConnectionState.Open)
                {
                    isConnectionOpen = true;
                }
                else
                {
                    isConnectionOpen = false;
                }
            }
            catch (Exception)
            {
                isConnectionOpen = false;
            }
            return sqlcon;
        }

        public static Boolean OpenConnection(SqlConnection sqlCon)
        {
            try
            {
                sqlCon = new SqlConnection(ReadConnectionString());
                if (sqlCon.State == ConnectionState.Closed)
                    sqlCon.Open();

                if (sqlCon.State == ConnectionState.Open)
                {
                    isConnectionOpen = true;
                }
                else
                {
                    isConnectionOpen = false;
                }
            }
            catch (Exception)
            {
                isConnectionOpen = false;
            }
            return isConnectionOpen;
        }

        public static Boolean CloseConnection(SqlConnection sqlCon)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();

                if (sqlCon.State == ConnectionState.Closed)
                {
                    isConnectionOpen = false;
                }
                else
                {
                    isConnectionOpen = true;
                }
            }
            catch (Exception)
            {
                isConnectionOpen = true;
            }
            return isConnectionOpen;
        }

    }
}

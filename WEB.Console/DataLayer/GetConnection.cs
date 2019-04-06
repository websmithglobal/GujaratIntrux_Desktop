using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WEB.ConsoleApp.DataLayer
{
    public class GetConnection
    {
        [SettingsDescription("This properties will return connection is open or not.")]
        [DefaultSettingValue("false")]
        public static Boolean isConnectionOpen { get; set; }

        //private static SqlConnection sqlcon = new SqlConnection(ReadConnectionString());

        private static string ReadConnectionString()
        {
            try
            {
                //return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                return Properties.Settings.Default.ConnectionString;
                //return @"Data Source=DESKTOP-02A3QO4\SQLEXPRESS;Initial Catalog=GIL;uid=sa; pwd=abcd;";
            }
            catch
            {
                return "Data Source=51.255.68.69;Initial Catalog=dbGujaratIntrux;uid=gujarat@@intrux##2018; pwd=GI$$2018##kg@@;";
                //return "Data Source=COM-PC;Initial Catalog=KAESER;uid=sa; pwd=abcd;";
            }
        }

        public static SqlConnection GetDBConnection()
        {
            SqlConnection sqlcon=null;
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
            catch (Exception ex)
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

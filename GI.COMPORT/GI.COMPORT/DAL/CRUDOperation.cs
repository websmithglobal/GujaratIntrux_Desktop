using System;
using System.Data;
using System.Data.SqlClient;

namespace GI.COMPORT.DAL
{
    public class CRUDOperation
    {
        private SqlConnection sqlCON;

        public CRUDOperation()
        {
            //sqlCON = GetConnection.GetDBConnection();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~CRUDOperation()
        {
            if (sqlCON != null)
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
            }
        }

        #region Insert,Update,Delete, Select Methods

        public bool InsertUpdateDelete(SqlCommand sqlCMD)
        {
            bool blnResult = false;
            //GetConnection.OpenConnection(sqlCON);
            sqlCON = GetConnection.GetDBConnection();
            SqlTransaction transaction = sqlCON.BeginTransaction(IsolationLevel.ReadCommitted);
            sqlCMD.Transaction = transaction;
            try
            {
                if (sqlCON.State == ConnectionState.Open)
                {
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlCMD.Connection = sqlCON;
                    int row = sqlCMD.ExecuteNonQuery();
                    if (row > 0)
                    {
                        transaction.Commit();
                        blnResult = true;
                    }
                    sqlCON.Close();
                    GetConnection.CloseConnection(sqlCON);
                    sqlCON.Dispose();
                }
                else
                {
                    blnResult = false;
                    throw new Exception("Server not found...database connection error");
                }
            }
            catch (Exception)
            {
                blnResult = false;
                transaction.Rollback();
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
                throw;
            }
            finally
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
            }
            return blnResult;
        }

        public DataTable getDataTable(SqlCommand sqlCMD)
        {
            DataTable dt = new DataTable();
            try
            {
                //GetConnection.OpenConnection(sqlCON);
                sqlCON = GetConnection.GetDBConnection();
                if (sqlCON.State == ConnectionState.Open)
                {
                    sqlCMD.CommandType = CommandType.StoredProcedure;
                    sqlCMD.Connection = sqlCON;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCMD);
                    da.Fill(dt);
                    sqlCON.Close();
                }
                else
                {
                    throw new Exception("Server not found...database connection error");
                }
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
            }
            catch (Exception ex)
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
            }
            return dt;
        }

        public DataTable getDataTableByQuery(SqlCommand sqlCMD)
        {
            DataTable dt = new DataTable();
            try
            {
                //GetConnection.OpenConnection(sqlCON);
                sqlCON = GetConnection.GetDBConnection();
                if (sqlCON.State == ConnectionState.Open)
                {
                    sqlCMD.CommandType = CommandType.Text;
                    sqlCMD.Connection = sqlCON;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCMD);
                    da.Fill(dt);
                    sqlCON.Close();
                    GetConnection.CloseConnection(sqlCON);
                    sqlCON.Dispose();
                }
                else
                {
                    throw new Exception("Server not found... database connection error");
                }
            }
            catch (Exception)
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
                throw;
            }
            finally
            {
                if (sqlCON.State == ConnectionState.Open)
                    sqlCON.Close();
                GetConnection.CloseConnection(sqlCON);
                sqlCON.Dispose();
            }
            return dt;
        }

        #endregion
    }
}

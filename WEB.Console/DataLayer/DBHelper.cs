using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace WEB.ConsoleApp.DataLayer
{
    public class DBHelper
    {
        public static object FillEntityFromReader(SqlDataReader dr, object objEntity)
        {
            object objResult = null;
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        foreach (var prop in objEntity.GetType().GetProperties())
                        {
                            try
                            {
                                int ordinal = dr.GetOrdinal(prop.Name);
                                object objValue = dr.GetValue(ordinal);
                                if (objValue != System.DBNull.Value)
                                {
                                    prop.SetValue(objEntity, objValue, null);
                                }
                            }
                            catch { }
                        }
                    }
                    objResult = objEntity;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return objResult;
        }
        
        public static TEntity CopyDataReaderToSingleEntity<TEntity>(IDataReader dataReader) where TEntity : class
        {
            TEntity entities = null;
            PropertyInfo[] properties = typeof(TEntity).GetProperties();
            while (dataReader.Read())
            {
                TEntity tempEntity = Activator.CreateInstance<TEntity>();
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        SetValue<TEntity>(property, tempEntity, dataReader[property.Name]);
                    }
                    catch { }
                }
                entities = tempEntity;
            }
            return entities;
        }

        public static List<TEntity> CopyDataReaderToEntity<TEntity>(IDataReader dataReader) where TEntity : class
        {
            List<TEntity> entities = new List<TEntity>();
            try
            {
                PropertyInfo[] properties = typeof(TEntity).GetProperties();
                while (dataReader.Read())
                {
                    TEntity tempEntity = Activator.CreateInstance<TEntity>();
                    foreach (PropertyInfo property in properties)
                    {
                        try
                        {
                            SetValue<TEntity>(property, tempEntity, dataReader[property.Name]);
                        }
                        catch { }
                    }
                    entities.Add(tempEntity);
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return entities;
        }

        private static TEntity SetValue<TEntity>(PropertyInfo property, TEntity entity, object propertyValue) where TEntity : class
        {
            if (property.CanRead)
            {
                if (propertyValue == null)
                {
                    if (property.PropertyType.Name == "String")
                        propertyValue = "";
                    else
                        propertyValue = 0;
                }
                if (property.CanWrite)
                {
                    if (propertyValue != DBNull.Value)
                    {
                        if (property.PropertyType.Name == "Single")
                            property.SetValue(entity, Convert.ToSingle(propertyValue), null);
                        else if (property.PropertyType.Name == "Int32")
                            property.SetValue(entity, Convert.ToInt32(propertyValue), null);
                        else if (property.PropertyType.Name == "Int64")
                            property.SetValue(entity, Convert.ToInt64(propertyValue), null);
                        else { property.SetValue(entity, propertyValue, null); }
                    }
                }
            }
            return entity;
        }

        public static List<T> GetEntityList<T>(SqlCommand sqlCMD)
        {
            CRUDOperation objCRUD = new CRUDOperation();
            List<T> data = new List<T>();
            try
            {
                using (DataTable dt = objCRUD.getDataTable(sqlCMD))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        T item = GetItem<T>(row);
                        data.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                sqlCMD.Connection.Close();
                sqlCMD.Dispose();
                throw;
            }
            finally
            {
                objCRUD = null;
                sqlCMD.Connection.Close();
                sqlCMD.Dispose();
            }
            return data;
        }

        public static List<T> GetEntityListByQuery<T>(SqlCommand sqlCMD)
        {
            CRUDOperation objCRUD = new CRUDOperation();
            List<T> data = new List<T>();
            try
            {
                using (DataTable dt = objCRUD.getDataTableByQuery(sqlCMD))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        T item = GetItem<T>(row);
                        data.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
                sqlCMD.Connection.Close();
                sqlCMD.Dispose();
                //throw;
            }
            finally
            {
                objCRUD = null;
                sqlCMD.Connection.Close();
                sqlCMD.Dispose();
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            try
            {
                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            if (dr[column.ColumnName] != DBNull.Value)
                            {
                                switch (pro.PropertyType.Name)
                                {
                                    case "String":
                                        pro.SetValue(obj, Convert.ToString(dr[column.ColumnName]), null);
                                        break;
                                    case "Decimal":
                                        pro.SetValue(obj, Convert.ToDecimal(dr[column.ColumnName]), null);
                                        break;
                                    case "Int32":
                                        pro.SetValue(obj, Convert.ToInt32(dr[column.ColumnName]), null);
                                        break;
                                    case "Int64":
                                        pro.SetValue(obj, Convert.ToInt64(dr[column.ColumnName]), null);
                                        break;
                                    case "Boolean":
                                        pro.SetValue(obj, Convert.ToBoolean(dr[column.ColumnName]), null);
                                        break;
                                    case "Double":
                                        pro.SetValue(obj, Convert.ToDouble(dr[column.ColumnName]), null);
                                        break;
                                    case "Guid":
                                        pro.SetValue(obj, dr[column.ColumnName], null);
                                        break;
                                    default:
                                        pro.SetValue(obj, dr[column.ColumnName], null);
                                        break;
                                }
                            }
                        }
                        else
                            continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                temp = null;
            }
            return obj;
        }

        public static List<T> GetEntityListByDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return data;
        }

        public static DataTable GetDataTableByQuery(SqlCommand sqlCMD)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = new CRUDOperation().getDataTableByQuery(sqlCMD);
            }
            catch (Exception)
            {
                dt = new DataTable();
                sqlCMD.Connection.Close();
                sqlCMD.Dispose();
                throw;
            }
            finally
            {
                sqlCMD.Connection.Close();
                sqlCMD.Dispose();
            }
            return dt;
        }

        public static string ChangeDate(string date)
        {
            string tmpDate = date.Substring(0, 10);
            string temp = "";
            temp = tmpDate.Substring(0, 2);
            switch (Convert.ToInt32(tmpDate.Substring(3, 2)))
            {
                case 1:
                    temp = temp + "-JAN-";
                    break;
                case 2:
                    temp = temp + "-FEB-";
                    break;
                case 3:
                    temp = temp + "-MAR-";
                    break;
                case 4:
                    temp = temp + "-APR-";
                    break;
                case 5:
                    temp = temp + "-MAY-";
                    break;
                case 6:
                    temp = temp + "-JUN-";
                    break;
                case 7:
                    temp = temp + "-JUL-";
                    break;
                case 8:
                    temp = temp + "-AUG-";
                    break;
                case 9:
                    temp = temp + "-SEP-";
                    break;
                case 10:
                    temp = temp + "-OCT-";
                    break;
                case 11:
                    temp = temp + "-NOV-";
                    break;
                case 12:
                    temp = temp + "-DEC-";
                    break;
            }
            temp = temp + tmpDate.Substring(6, 4);  // 01/01/2015
            return temp.ToString();
        }

        public static string ChangeDateTime(string date)
        {
            string temp = "";
            try
            {
                string tmpDate = date.Substring(0, 10);   // 04/10/2017 12:43 PM
                string tmpTime = date.Substring(11, 8);
                temp = tmpDate.Substring(0, 2);
                switch (Convert.ToInt32(tmpDate.Substring(3, 2)))
                {
                    case 1:
                        temp = temp + "-JAN-";
                        break;
                    case 2:
                        temp = temp + "-FEB-";
                        break;
                    case 3:
                        temp = temp + "-MAR-";
                        break;
                    case 4:
                        temp = temp + "-APR-";
                        break;
                    case 5:
                        temp = temp + "-MAY-";
                        break;
                    case 6:
                        temp = temp + "-JUN-";
                        break;
                    case 7:
                        temp = temp + "-JUL-";
                        break;
                    case 8:
                        temp = temp + "-AUG-";
                        break;
                    case 9:
                        temp = temp + "-SEP-";
                        break;
                    case 10:
                        temp = temp + "-OCT-";
                        break;
                    case 11:
                        temp = temp + "-NOV-";
                        break;
                    case 12:
                        temp = temp + "-DEC-";
                        break;
                }
                temp = temp + tmpDate.Substring(6, 4);
                temp = temp.Trim() + " " + tmpTime.Trim();
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show(ex.Message.ToString(), "Change Datetime", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return temp.ToString();
        }

        public static bool IsDate(string date)
        {
            bool result = false;
            try
            {
                DateTime temp;
                if (DateTime.TryParse(date, out temp))
                    result = true;
                else
                    result = false;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

    }
}

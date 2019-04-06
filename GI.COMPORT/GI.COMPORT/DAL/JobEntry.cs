using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ENT = GI.COMPORT.ENTITY;

namespace GI.COMPORT.DAL
{
    public class JobEntry
    {
        SqlCommand sqlCMD;
        CRUDOperation objCRUD = new CRUDOperation();

        public bool InsertUpdateDeleteJobEntry(ENT.JobEntry objENT)
        {
            bool row = false;
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "InsertUpdateDeleteJobEntry";
                sqlCMD.Parameters.AddWithValue("@JobID", objENT.JobID);
                sqlCMD.Parameters.AddWithValue("@JobNo", objENT.JobNo);
                sqlCMD.Parameters.AddWithValue("@JobFurnaceNo", objENT.JobFurnaceNo);
                sqlCMD.Parameters.AddWithValue("@StartTime", objENT.StartTime);
                sqlCMD.Parameters.AddWithValue("@EndTime", objENT.EndTime);
                sqlCMD.Parameters.AddWithValue("@Pour1Time", objENT.Pour1Time);
                sqlCMD.Parameters.AddWithValue("@Pour2Time", objENT.Pour2Time);
                sqlCMD.Parameters.AddWithValue("@Pour3Time", objENT.Pour3Time);
                sqlCMD.Parameters.AddWithValue("@Pour4Time", objENT.Pour4Time);
                sqlCMD.Parameters.AddWithValue("@Pour5Time", objENT.Pour5Time);
                sqlCMD.Parameters.AddWithValue("@Pour6Time", objENT.Pour6Time);
                sqlCMD.Parameters.AddWithValue("@IsFinish", objENT.IsFinish);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                row = objCRUD.InsertUpdateDelete(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return row;
        }

        public List<ENT.JobEntry> GetJobEntry(ENT.JobEntry objENT)
        {
            List<ENT.JobEntry> lstENT = new List<ENT.JobEntry>();
            try
            {
                sqlCMD = new SqlCommand();
                sqlCMD.CommandText = "GetJobEntry";
                sqlCMD.Parameters.AddWithValue("@JobID", objENT.JobID);
                sqlCMD.Parameters.AddWithValue("@JobNo", objENT.JobNo);
                sqlCMD.Parameters.AddWithValue("@StartTime", objENT.strJobStartTime);
                sqlCMD.Parameters.AddWithValue("@EndTime", objENT.strJobEndTime);
                sqlCMD.Parameters.AddWithValue("@Mode", objENT.Mode);
                lstENT = DBHelper.GetEntityList<ENT.JobEntry>(sqlCMD);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstENT;
        }

        public DataTable GetJobEntry()
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "SELECT TOP 1 JobNo FROM JobEntry ORDER BY StartTime DESC;";
                sqlCMD = new SqlCommand(strqry);
                dt = DBHelper.GetDataTableByQuery(sqlCMD);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            return dt;
        }

        public DataTable GetOverAllHeatFurnaceTime(string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "";
                if (string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                {
                    strqry = @"SELECT JobNo, JobFurnaceNo, JobStartDateTime, JobEndDateTime,
                    CONVERT(varchar(50), ISNULL(TotalSecond / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(TotalSecond % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalSecond % 60, 0)) As TotalHrsMinSec
                    FROM(
                    SELECT JobNo, JobFurnaceNo, DATEDIFF(SECOND, JobStartDateTime, JobEndDateTime) AS TotalSecond, JobStartDateTime, JobEndDateTime
                    FROM JobEntry
                    WHERE IsFinish = 1) AS Temp ORDER BY convert(datetime,JobStartDateTime) DESC";
                }
                else
                {
                    strqry = @"SELECT JobNo, JobFurnaceNo, JobStartDateTime, JobEndDateTime,
                    CONVERT(varchar(50), ISNULL(TotalSecond / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(TotalSecond % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalSecond % 60, 0)) As TotalHrsMinSec
                    FROM(
                    SELECT JobNo, JobFurnaceNo, DATEDIFF(SECOND, JobStartDateTime, JobEndDateTime) AS TotalSecond, JobStartDateTime, JobEndDateTime
                    FROM JobEntry
                    WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, JobStartDateTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "') AS Temp ORDER BY convert(datetime,JobStartDateTime) DESC";
                }
                //if (string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                //{
                //    strqry = @"SELECT JobNo, JobFurnaceNo, JobStartDateTime, JobEndDateTime,
                //    CONVERT(time, CONVERT(varchar(50), ISNULL(TotalSecond / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(TotalSecond % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalSecond % 60, 0))) As TotalHrsMinSec
                //    FROM(
                //    SELECT JobNo, JobFurnaceNo, DATEDIFF(SECOND, JobStartDateTime, JobEndDateTime) AS TotalSecond, JobStartDateTime, JobEndDateTime
                //    FROM JobEntry
                //    WHERE IsFinish = 1) AS Temp ORDER BY convert(datetime,JobStartDateTime) DESC";
                //}
                //else
                //{
                //    strqry = @"SELECT JobNo, JobFurnaceNo, JobStartDateTime, JobEndDateTime,
                //    CONVERT(time, CONVERT(varchar(50), ISNULL(TotalSecond / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(TotalSecond % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalSecond % 60, 0))) As TotalHrsMinSec
                //    FROM(
                //    SELECT JobNo, JobFurnaceNo, DATEDIFF(SECOND, JobStartDateTime, JobEndDateTime) AS TotalSecond, JobStartDateTime, JobEndDateTime
                //    FROM JobEntry
                //    WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, JobStartDateTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "') AS Temp ORDER BY convert(datetime,JobStartDateTime) DESC";
                //}

                sqlCMD = new SqlCommand(strqry);
                dt = DBHelper.GetDataTableByQuery(sqlCMD);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            return dt;
        }

        public DataTable GetEnergyConsumptionHeatWise(string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "";
                if (string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                {
                    strqry = @"SELECT JobNo,JobFurnaceNo,
	                    (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN StartTime and EndTime) AS TotalUnit, 
                        StartTime,EndTime,HeatStartStop.HeatStart,HeatStartStop.HeatStop
	                    FROM JobEntry 
	                    LEFT JOIN HeatStartStop ON HeatFurnaceID = HeatID
	                    WHERE IsFinish=1
	                    ORDER BY JobEntry.StartTime DESC";
                }
                else
                {
                    strqry = @"SELECT JobNo,JobFurnaceNo,
	                    (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN StartTime and EndTime) AS TotalUnit, 
                        StartTime,EndTime,HeatStartStop.HeatStart,HeatStartStop.HeatStop
                        FROM JobEntry 
	                    LEFT JOIN HeatStartStop ON HeatFurnaceID = HeatID
	                    WHERE IsFinish = 1
                        AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' ORDER BY JobEntry.StartTime DESC";
                }

                sqlCMD = new SqlCommand(strqry);
                dt = DBHelper.GetDataTableByQuery(sqlCMD);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            return dt;
        }

        public DataTable GetCurrentJob(string FromDate, string ToDate, bool IsLive, string JobNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string strqry = "";
                strqry = @"SELECT TOP 1 JobNo, JobFurnaceNo, StartTime, EndTime, Pour1Time, Pour2Time, Pour3Time, Pour4Time, Pour5Time, Pour6Time,
                        CONVERT(varchar(50), ISNULL(Pour1Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour1Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour1Sec % 60, 0)) As TotalPour1HrsMinSec
                        ,CONVERT(varchar(50), ISNULL(Pour2Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour2Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour2Sec % 60, 0)) As TotalPour2HrsMinSec
                        ,CONVERT(varchar(50), ISNULL(Pour3Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour3Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour3Sec % 60, 0)) As TotalPour3HrsMinSec
                        ,CONVERT(varchar(50), ISNULL(Pour4Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour4Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour4Sec % 60, 0)) As TotalPour4HrsMinSec
                        ,CONVERT(varchar(50), ISNULL(Pour5Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour5Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour5Sec % 60, 0)) As TotalPour5HrsMinSec
                        ,CONVERT(varchar(50), ISNULL(Pour6Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour6Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour6Sec % 60, 0)) As TotalPour6HrsMinSec
                        ,P1UnitTotal,P2UnitTotal,P3UnitTotal,P4UnitTotal,P5UnitTotal,P6UnitTotal                        
                        FROM (
                        SELECT jb.JobID, JobNo, JobFurnaceNo, StartTime, EndTime, Pour1Time, Pour2Time, Pour3Time, Pour4Time, Pour5Time, Pour6Time, IsFinish, 
                        CASE WHEN ISNULL(p1.Pour1Sec,0) < 0 THEN 0 ELSE ISNULL(p1.Pour1Sec,0) END AS Pour1Sec, 
                        CASE WHEN ISNULL(p2.Pour2Sec,0) < 0 THEN 0 ELSE ISNULL(p2.Pour2Sec,0) END AS Pour2Sec, 
                        CASE WHEN ISNULL(p3.Pour3Sec,0) < 0 THEN 0 ELSE ISNULL(p3.Pour3Sec,0) END AS Pour3Sec, 
                        CASE WHEN ISNULL(p4.Pour4Sec,0) < 0 THEN 0 ELSE ISNULL(p4.Pour4Sec,0) END AS Pour4Sec, 
                        CASE WHEN ISNULL(p5.Pour5Sec,0) < 0 THEN 0 ELSE ISNULL(p5.Pour5Sec,0) END AS Pour5Sec, 
                        CASE WHEN ISNULL(p6.Pour6Sec,0) < 0 THEN 0 ELSE ISNULL(p6.Pour6Sec,0) END AS Pour6Sec,
                        P1UnitTotal,P2UnitTotal,P3UnitTotal,P4UnitTotal,P5UnitTotal,P6UnitTotal
                        FROM JobEntry AS jb INNER JOIN (
                        SELECT JobID, DATEDIFF(SECOND, StartTime, Pour1Time) AS Pour1Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN StartTime and Pour1Time) AS P1UnitTotal FROM JobEntry) AS p1 ON jb.JobID=p1.JobID INNER JOIN (
                        SELECT JobID, DATEDIFF(SECOND, Pour1Time, Pour2Time) AS Pour2Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour1Time and Pour2Time) AS P2UnitTotal FROM JobEntry) AS p2 ON jb.JobID=p2.JobID INNER JOIN (
                        SELECT JobID, DATEDIFF(SECOND, Pour2Time, Pour3Time) AS Pour3Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour2Time and Pour3Time) AS P3UnitTotal FROM JobEntry) AS p3 ON jb.JobID=p3.JobID INNER JOIN (
                        SELECT JobID, DATEDIFF(SECOND, Pour3Time, Pour4Time) AS Pour4Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour3Time and Pour4Time) AS P4UnitTotal FROM JobEntry) AS p4 ON jb.JobID=p4.JobID INNER JOIN (
                        SELECT JobID, DATEDIFF(SECOND, Pour4Time, Pour5Time) AS Pour5Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour4Time and Pour5Time) AS P5UnitTotal FROM JobEntry) AS p5 ON jb.JobID=p5.JobID INNER JOIN (
                        SELECT JobID, DATEDIFF(SECOND, Pour5Time, Pour6Time) AS Pour6Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour6Time and Pour6Time) AS P6UnitTotal FROM JobEntry) AS p6 ON jb.JobID=p6.JobID 
                        ) AS Temp";
                //strqry = @"SELECT TOP 1 JobNo, JobFurnaceNo, StartTime, EndTime, Pour1Time, Pour2Time, Pour3Time, Pour4Time, Pour5Time, Pour6Time,
                //        CONVERT(time, CONVERT(varchar(50), ISNULL(Pour1Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour1Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour1Sec % 60, 0))) As TotalPour1HrsMinSec
                //        ,CONVERT(time, CONVERT(varchar(50), ISNULL(Pour2Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour2Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour2Sec % 60, 0))) As TotalPour2HrsMinSec
                //        ,CONVERT(time, CONVERT(varchar(50), ISNULL(Pour3Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour3Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour3Sec % 60, 0))) As TotalPour3HrsMinSec
                //        ,CONVERT(time, CONVERT(varchar(50), ISNULL(Pour4Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour4Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour4Sec % 60, 0))) As TotalPour4HrsMinSec
                //        ,CONVERT(time, CONVERT(varchar(50), ISNULL(Pour5Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour5Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour5Sec % 60, 0))) As TotalPour5HrsMinSec
                //        ,CONVERT(time, CONVERT(varchar(50), ISNULL(Pour6Sec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(Pour6Sec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(Pour6Sec % 60, 0))) As TotalPour6HrsMinSec
                //        ,P1UnitTotal,P2UnitTotal,P3UnitTotal,P4UnitTotal,P5UnitTotal,P6UnitTotal                        
                //        FROM (
                //        SELECT jb.JobID, JobNo, JobFurnaceNo, StartTime, EndTime, Pour1Time, Pour2Time, Pour3Time, Pour4Time, Pour5Time, Pour6Time, IsFinish, 
                //        CASE WHEN ISNULL(p1.Pour1Sec,0) < 0 THEN 0 ELSE ISNULL(p1.Pour1Sec,0) END AS Pour1Sec, 
                //        CASE WHEN ISNULL(p2.Pour2Sec,0) < 0 THEN 0 ELSE ISNULL(p2.Pour2Sec,0) END AS Pour2Sec, 
                //        CASE WHEN ISNULL(p3.Pour3Sec,0) < 0 THEN 0 ELSE ISNULL(p3.Pour3Sec,0) END AS Pour3Sec, 
                //        CASE WHEN ISNULL(p4.Pour4Sec,0) < 0 THEN 0 ELSE ISNULL(p4.Pour4Sec,0) END AS Pour4Sec, 
                //        CASE WHEN ISNULL(p5.Pour5Sec,0) < 0 THEN 0 ELSE ISNULL(p5.Pour5Sec,0) END AS Pour5Sec, 
                //        CASE WHEN ISNULL(p6.Pour6Sec,0) < 0 THEN 0 ELSE ISNULL(p6.Pour6Sec,0) END AS Pour6Sec,
                //        P1UnitTotal,P2UnitTotal,P3UnitTotal,P4UnitTotal,P5UnitTotal,P6UnitTotal
                //        FROM JobEntry AS jb INNER JOIN (
                //        SELECT JobID, DATEDIFF(SECOND, StartTime, Pour1Time) AS Pour1Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN StartTime and Pour1Time) AS P1UnitTotal FROM JobEntry) AS p1 ON jb.JobID=p1.JobID INNER JOIN (
                //        SELECT JobID, DATEDIFF(SECOND, Pour1Time, Pour2Time) AS Pour2Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour1Time and Pour2Time) AS P2UnitTotal FROM JobEntry) AS p2 ON jb.JobID=p2.JobID INNER JOIN (
                //        SELECT JobID, DATEDIFF(SECOND, Pour2Time, Pour3Time) AS Pour3Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour2Time and Pour3Time) AS P3UnitTotal FROM JobEntry) AS p3 ON jb.JobID=p3.JobID INNER JOIN (
                //        SELECT JobID, DATEDIFF(SECOND, Pour3Time, Pour4Time) AS Pour4Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour3Time and Pour4Time) AS P4UnitTotal FROM JobEntry) AS p4 ON jb.JobID=p4.JobID INNER JOIN (
                //        SELECT JobID, DATEDIFF(SECOND, Pour4Time, Pour5Time) AS Pour5Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour4Time and Pour5Time) AS P5UnitTotal FROM JobEntry) AS p5 ON jb.JobID=p5.JobID INNER JOIN (
                //        SELECT JobID, DATEDIFF(SECOND, Pour5Time, Pour6Time) AS Pour6Sec, (SELECT ISNULL(SUM(DataValue-DataValue2),0) FROM MeterSlaveMaster WHERE DataTime BETWEEN Pour6Time and Pour6Time) AS P6UnitTotal FROM JobEntry) AS p6 ON jb.JobID=p6.JobID 
                //        ) AS Temp";

                if (string.IsNullOrEmpty(FromDate) && string.IsNullOrEmpty(ToDate))
                {
                    if (IsLive)
                    {
                        strqry = strqry + " WHERE IsFinish = 0 ORDER BY convert(datetime,StartTime) DESC";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(JobNo.Trim()))
                        { strqry = strqry + " WHERE IsFinish = 1 AND JobNo='" + JobNo.Trim() + "' ORDER BY convert(datetime,StartTime) DESC"; }
                        else
                        { strqry = strqry + " WHERE IsFinish = 1 ORDER BY convert(datetime,StartTime) DESC"; }
                    }
                }
                else
                {
                    if (IsLive)
                        strqry = strqry + " WHERE IsFinish = 0 AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' ORDER BY convert(datetime, StartTime) DESC";
                    else
                    {
                        if (!string.IsNullOrEmpty(JobNo.Trim()))
                        { strqry = strqry + " WHERE IsFinish = 1 AND JobNo='" + JobNo.Trim() + "' AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' ORDER BY convert(datetime, StartTime) DESC"; }
                        else
                        { strqry = strqry + " WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' ORDER BY convert(datetime, StartTime) DESC"; }

                    }

                }

                sqlCMD = new SqlCommand(strqry);
                dt = DBHelper.GetDataTableByQuery(sqlCMD);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            return dt;
        }

        public DataTable GetDryRunReport(string FromDate, string ToDate)
        {
            DataTable dt = new DataTable();
            try
            {
                string strQuery = "";
               
                strQuery = @"SELECT HeatID, HeatStart, HeatStop, ";
                strQuery += " CONVERT(varchar(50), ISNULL(TotalFurSec / 3600, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalFurSec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalFurSec % 60, 0)) As TotFurTime, ";
                strQuery += " CONVERT(varchar(50), ISNULL(TotalJobSec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(TotalJobSec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalJobSec % 60, 0)) As TotJobTime, ";
                strQuery += " CONVERT(varchar(50), ISNULL((TotalFurSec-TotalJobSec) / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL((TotalFurSec-TotalJobSec) % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL((TotalFurSec-TotalJobSec) % 60, 0)) As TotDryRunTime, ";
                strQuery += " FurUnitTotal, JobUnitTotal, (FurUnitTotal-JobUnitTotal) AS DryRunUnitTot, ((FurUnitTotal-JobUnitTotal)*(SELECT UnitRate FROM UnitRateMaster WHERE IsActive=1)) AS DryRunAmtTot ";
                strQuery += " FROM ( ";
                strQuery += " SELECT HeatID, HeatStart, HeatStop,DATEDIFF(SECOND, HeatStart, HeatStop) AS TotalFurSec,JOB.TotalJobSec,JOB.JobUnitTotal, ";
                strQuery += " (SELECT ISNULL(SUM(DataValue - DataValue2), 0) FROM MeterSlaveMaster WHERE DataTime BETWEEN HeatStart AND HeatStop) AS FurUnitTotal";
                strQuery += " FROM JobEntry INNER JOIN HeatStartStop ON HeatStartStop.HeatID = JobEntry.HeatFurnaceID INNER JOIN ";
                strQuery += " (SELECT JOBSEC.HeatFurnaceID, JOBSEC.TotalJobSec, SUM(JOBUNI.JobUnitTotal) AS JobUnitTotal ";
                strQuery += " FROM ( ";
                strQuery += " SELECT HeatFurnaceID, SUM(DATEDIFF(SECOND, StartTime, EndTime)) AS TotalJobSec ";
                strQuery += " FROM JobEntry WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' GROUP BY HeatFurnaceID ";
                strQuery += " ) As JOBSEC INNER JOIN ( ";
                strQuery += " SELECT HeatFurnaceID, (SELECT ISNULL(SUM(DataValue - DataValue2), 0) FROM MeterSlaveMaster WHERE DataTime BETWEEN StartTime AND EndTime) AS JobUnitTotal ";
                strQuery += " FROM JobEntry WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' GROUP BY HeatFurnaceID,StartTime,EndTime ";
                strQuery += " ) AS JOBUNI ON JOBSEC.HeatFurnaceID = JOBUNI.HeatFurnaceID ";
                strQuery += " GROUP BY JOBSEC.HeatFurnaceID, JOBSEC.TotalJobSec) ";
                strQuery += " AS JOB ON JOB.HeatFurnaceID = JobEntry.HeatFurnaceID AND IsFinish = 1 AND IsStop = 1 ";
                strQuery += " AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' GROUP BY HeatID, HeatStart, HeatStop, JOB.TotalJobSec, JOB.JobUnitTotal ";
                strQuery += " ) AS Temp ORDER BY HeatStart DESC";

                //strQuery = @"SELECT HeatID, HeatStart, HeatStop, ";
                //strQuery += " CONVERT(time, CONVERT(varchar(50), ISNULL(TotalFurSec / 3600, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalFurSec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalFurSec % 60, 0))) As TotFurTime, ";
                //strQuery += " CONVERT(time, CONVERT(varchar(50), ISNULL(TotalJobSec / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL(TotalJobSec % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL(TotalJobSec % 60, 0))) As TotJobTime, ";
                //strQuery += " CONVERT(time, CONVERT(varchar(50), ISNULL((TotalFurSec-TotalJobSec) / 3600, 0)) +':' + CONVERT(varchar(50), ISNULL((TotalFurSec-TotalJobSec) % 3600 / 60, 0)) + ':' + CONVERT(varchar(50), ISNULL((TotalFurSec-TotalJobSec) % 60, 0))) As TotDryRunTime, ";
                //strQuery += " FurUnitTotal, JobUnitTotal, (FurUnitTotal-JobUnitTotal) AS DryRunUnitTot, ((FurUnitTotal-JobUnitTotal)*(SELECT UnitRate FROM UnitRateMaster WHERE IsActive=1)) AS DryRunAmtTot ";
                //strQuery += " FROM ( ";
                //strQuery += " SELECT HeatID, HeatStart, HeatStop,DATEDIFF(SECOND, HeatStart, HeatStop) AS TotalFurSec,JOB.TotalJobSec,JOB.JobUnitTotal, ";
                //strQuery += " (SELECT ISNULL(SUM(DataValue - DataValue2), 0) FROM MeterSlaveMaster WHERE DataTime BETWEEN HeatStart AND HeatStop) AS FurUnitTotal";
                //strQuery += " FROM JobEntry INNER JOIN HeatStartStop ON HeatStartStop.HeatID = JobEntry.HeatFurnaceID INNER JOIN ";
                //strQuery += " (SELECT JOBSEC.HeatFurnaceID, JOBSEC.TotalJobSec, SUM(JOBUNI.JobUnitTotal) AS JobUnitTotal ";
                //strQuery += " FROM ( ";
                //strQuery += " SELECT HeatFurnaceID, SUM(DATEDIFF(SECOND, StartTime, EndTime)) AS TotalJobSec ";
                //strQuery += " FROM JobEntry WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' GROUP BY HeatFurnaceID ";
                //strQuery += " ) As JOBSEC INNER JOIN ( ";
                //strQuery += " SELECT HeatFurnaceID, (SELECT ISNULL(SUM(DataValue - DataValue2), 0) FROM MeterSlaveMaster WHERE DataTime BETWEEN StartTime AND EndTime) AS JobUnitTotal ";
                //strQuery += " FROM JobEntry WHERE IsFinish = 1 AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' GROUP BY HeatFurnaceID,StartTime,EndTime ";
                //strQuery += " ) AS JOBUNI ON JOBSEC.HeatFurnaceID = JOBUNI.HeatFurnaceID ";
                //strQuery += " GROUP BY JOBSEC.HeatFurnaceID, JOBSEC.TotalJobSec) ";
                //strQuery += " AS JOB ON JOB.HeatFurnaceID = JobEntry.HeatFurnaceID AND IsFinish = 1 AND IsStop = 1 ";
                //strQuery += " AND DATEADD(d, DATEDIFF(d, 0, StartTime), 0) BETWEEN '" + FromDate + "' AND '" + ToDate + "' GROUP BY HeatID, HeatStart, HeatStop, JOB.TotalJobSec, JOB.JobUnitTotal ";
                //strQuery += " ) AS Temp ORDER BY HeatStart DESC";

                sqlCMD = new SqlCommand(strQuery);
                dt = DBHelper.GetDataTableByQuery(sqlCMD);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                throw ex;
            }
            return dt;
        }
    }
}

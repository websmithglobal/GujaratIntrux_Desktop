using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
using ENT = WEB.ConsoleApp.Entity;
using DAL = WEB.ConsoleApp.DataLayer;
using System.Reflection;

namespace WEB.ConsoleApp
{
    class Program
    {
        /// <summary>
        /// get connection string from app.config file. 
        /// </summary>
        private static readonly string _connectionString = Properties.Settings.Default.ConnectionString;

        /// <summary>
        /// console main function which start from here.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Console Setting
            Console.CursorVisible = false;
            Console.Title = "Send Heat Data Notification";
            Console.SetWindowSize(Console.WindowWidth / 2, Console.WindowHeight);

            // Create directory Log for log file if not exist on exe path
            string path = Properties.Settings.Default.logPath; //@"E:\WEBSMITH\Notification\Log";
           
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // this function delete files which is created before 30 days.
            bool result = DeleteOldLogFile(path);

            // Write log in console window
            Console.WriteLine("*************************************************");
            Console.WriteLine("Waiting for receiving notifications...");
            Console.WriteLine("*************************************************");

            // Write log in text file
            WriteLog("********************************************************");
            WriteLog("Application Started.");
            WriteLog("********************************************************");

            // connection of local database using connection string for table FurnaceSwitch
            using (var tableDependency = new SqlTableDependency<ENT.FurnaceSwitch>(_connectionString))
            {
                // creating onchange event for FurnaceSwitch
                tableDependency.OnChanged += _dependency_Changed;
                tableDependency.Start();

                // connection of local database using connection string for table MeterSlaveMaster
                using (var MeterSlaveMasterDependency = new SqlTableDependency<ENT.MeterSlaveMaster>(_connectionString))
                {
                    // creating onchange event for MeterSlaveMaster
                    MeterSlaveMasterDependency.OnChanged += _MeterSlaveMasterDependencyDependency_Changed;
                    MeterSlaveMasterDependency.Start();

                    using (var HeatStartStopReportDependency = new SqlTableDependency<ENT.HeatStartStopReport>(_connectionString))
                    {
                        HeatStartStopReportDependency.OnChanged += _HeatStartStopReportDependencyDependency_Changed;
                        HeatStartStopReportDependency.Start();
                        Console.ReadKey();
                    }
                    Console.ReadKey();
                }
                Console.ReadKey();
            }
        }

        /// <summary>
        /// _dependency OnError Event for FurnaceSwitch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _dependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }

        /// <summary>
        /// this event is called when any of the change found in database table of FurnaceSwitch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _dependency_Changed(object sender, RecordChangedEventArgs<ENT.FurnaceSwitch> e)
        {
            // write log in to file and console window
            Console.WriteLine("Date Time : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
            Console.WriteLine("FurnaceSwitch : " + e.ChangeType);
            WriteLog("FurnaceSwitch : " + e.ChangeType);
            Console.WriteLine(e.ChangeType + " = > ID : " + e.Entity.fur_id);
            WriteLog(e.ChangeType + " = > ID : " + e.Entity.fur_id);

            // if getting INSERT flag from dependency then we process for insert in live database
            if (e.ChangeType.ToString().ToUpper() == "INSERT")
            {
                string jsonNotification = string.Empty;
                ENT.ResponseHeatReport objResponse = new ENT.ResponseHeatReport();
                List<ENT.HeatFurnaceReport> lstResult = new List<ENT.HeatFurnaceReport>();

                if (e.Entity.fur_status == 0)
                {
                    Console.WriteLine("Furnace Status : " + (e.Entity.fur_status == 0 ? "POURSTOP" : "POURSTART"));
                    WriteLog("Furnace Status : " + (e.Entity.fur_status == 0 ? "POURSTOP" : "POURSTART"));

                    // get data from local database
                    lstResult = new DAL.HeatReport().GetHeatReportByDate(DateTime.Now.ToString("dd-MMM-yyyy"));
                    if (lstResult.Count > 0)
                    {
                        Console.WriteLine("Record Found : " + lstResult.Count);
                        WriteLog("Record Found : " + lstResult.Count);

                        // heat timming data
                        objResponse.ReportName = "PourReport";
                        objResponse.PowerOn = Convert.ToString(lstResult[0].fur_entry_time);
                        objResponse.SuperHeat = Convert.ToString(lstResult[0].fur_open_time);
                        objResponse.HeatTapped = Convert.ToString(lstResult[0].fur_close_time);
                        objResponse.TapToTapHrsMin = Convert.ToString(lstResult[0].HrsMin);

                        // heat unit consumption data
                        objResponse.KwhrAtStart = Convert.ToString(lstResult[0].DataValue);
                        objResponse.KwhrAtEnd = Convert.ToString(lstResult[0].DataValue2);
                        objResponse.TotalKwhr = Convert.ToString(lstResult[0].UnitDifference);
                        objResponse.KwhrHeat = Convert.ToString(lstResult[0].UnitDifference * 10);

                        // create json for send data to mobile app
                        jsonNotification = Newtonsoft.Json.JsonConvert.SerializeObject(objResponse);
                        WriteLog(jsonNotification);
                    }
                }

                // here, call the function for send data from local to live database and send FCM notificatio.
                string response = SendFurnaceSwitchData(e.Entity, jsonNotification);

                // write log of return response from FCM
                Console.WriteLine("API Result : " + response);
                WriteLog("API Result : " + response);
            }

            WriteLog("*************************************************");
            Console.WriteLine("*************************************************");
            Console.WriteLine("Waiting for receiving new notifications...");
            Console.WriteLine("*************************************************");
        }

        /// <summary>
        /// this function is used to send data from local to live database and also send notification to mobile app
        /// </summary>
        /// <param name="obj">object of FurnaceSwitch data</param>
        /// <param name="json">json of FurnaceSwitch for send to FCM notification</param>
        /// <returns></returns>
        private static string SendFurnaceSwitchData(ENT.FurnaceSwitch obj, string json)
        {
            string message = string.Empty;
            try
            {
                // this is our live api url for send data to live database and send notification also
                WebRequest tRequest = WebRequest.Create("http://gisrv.appsmith.co.in/api/HeatReport/InsertFurnaceSwitchAPI");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                obj.fur_json = json;
                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);

                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();
                                message = "FurnaceSwitch : " + responseFromFirebaseServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
            return message;
        }

        /// <summary>
        /// this event is called when any of the change found in database table of MeterSlaveMaster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _MeterSlaveMasterDependencyDependency_Changed(object sender, RecordChangedEventArgs<ENT.MeterSlaveMaster> e)
        {
            // write log in to file and console window
            Console.WriteLine("Date Time : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
            Console.WriteLine("MeterSlaveMaster : " + e.ChangeType);
            WriteLog("MeterSlaveMaster : " + e.ChangeType);

            // if getting INSERT flag from dependency then we process for insert in live database
            if (e.ChangeType.ToString().ToUpper() == "INSERT")
            {
                // For Dryrun Report get top one record from meter slave master table
                List<ENT.MeterSlaveMaster> lstMSM = new List<ENT.MeterSlaveMaster>();
                lstMSM = new DAL.DryrunHeatReport().GetMeterSlaveMasterTop1ByDate(DateTime.Now.ToString("dd-MMM-yyyy"));
                if (lstMSM.Count > 0)
                {
                    List<ENT.DryrunHeatReport> lstDryrun = new List<ENT.DryrunHeatReport>();
                    lstDryrun = new DAL.DryrunHeatReport().GetDryrunHeatReportTop1ByDate(DateTime.Now.ToString("dd-MMM-yyyy"));

                    decimal dcmUnit = lstMSM[0].DataValue - lstMSM[0].DataValue2;
                    Console.WriteLine("Unit Is : " + dcmUnit + " => Last Record Found : " + lstDryrun.Count);

                    if (dcmUnit > 5 && lstDryrun.Count > 0)
                    {
                        WriteLog("Dryrun unit greater then five found at line of " + lstMSM[0].LineCount + " And Unit Is : " + dcmUnit);
                        ENT.DryrunHeatReport objHDRH = new ENT.DryrunHeatReport();
                        // Insert First record for dryrun heat report
                        try
                        {
                            objHDRH.ID = lstDryrun[0].ID;
                            objHDRH.LineCountEnd = lstMSM[0].LineCount;
                            objHDRH.EndDataTime = lstMSM[0].DataTime;
                            objHDRH.DataValueLast = lstMSM[0].DataValue;
                            objHDRH.EntryDate = DateTime.Now;
                            objHDRH.Mode = "UPDATE";
                            if (new DAL.DryrunHeatReport().InsertUpdateDeleteDryrunHeatReport(objHDRH))
                            {
                                WriteLog("Dryrun Unit Updated Suucessfully.");

                                // send dryrun report notification
                                string response = SendDryrunHeatReport(lstDryrun[0].ID);

                                // write log of return response from API
                                Console.WriteLine(response);
                                WriteLog(response);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                            WriteLog(ex.Message);
                        }
                    }
                }

                // here, call the function for send data from local to live database
                string result = SendMeterSlaveMasterData(e.Entity);


                // write log of return response from API
                Console.WriteLine(result);
                WriteLog(result);
            }

            // write log in to file and console window
            Console.WriteLine(e.ChangeType + " = > ID : " + e.Entity.ID);
            WriteLog(e.ChangeType + " = > ID : " + e.Entity.ID);

            Console.WriteLine("*************************************************");
            Console.WriteLine("Waiting for receiving new notifications...");
            Console.WriteLine("*************************************************");
            WriteLog("*************************************************");
        }

        /// <summary>
        /// MeterSlaveMasterDependencyDependency OnError Event for MeterSlaveMaster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _MeterSlaveMasterDependencyDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }

        /// <summary>
        /// this function is used to send data from local to live database.
        /// </summary>
        /// <param name="obj">object of MeterSlaveMaster data</param>
        /// <returns></returns>
        private static string SendMeterSlaveMasterData(ENT.MeterSlaveMaster obj)
        {
            string message = string.Empty;
            try
            {
                // this is our live api url for send data to live database
                WebRequest tRequest = WebRequest.Create("http://gisrv.appsmith.co.in/api/HeatReport/InsertMeterSlaveMasterAPI");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);

                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();
                                message = "MeterSlaveMaste : " + responseFromFirebaseServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
            return message;
        }

        /// <summary>
        /// _HeatStartStopReportDependency OnError Event for HeatStartStopReport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _HeatStartStopReportDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            throw e.Error;
        }

        /// <summary>
        /// this event is called when any of the change found in database table of MeterSlaveMaster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _HeatStartStopReportDependencyDependency_Changed(object sender, RecordChangedEventArgs<ENT.HeatStartStopReport> e)
        {
            // write log in to file and console window
            Console.WriteLine("Date Time : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt"));
            Console.WriteLine("HeatStartStopReport : " + e.ChangeType);
            WriteLog("HeatStartStopReport : " + e.ChangeType);
            
            // write log in to file and console window
            Console.WriteLine(e.ChangeType + " = > ID : " + e.Entity.ID);
            WriteLog(e.ChangeType + " = > ID : " + e.Entity.ID);
            
            string jsonNotification = string.Empty;
            ENT.ResponseHeatReport objResponse = new ENT.ResponseHeatReport();
            ENT.ResponseHeatReport objDryrun = new ENT.ResponseHeatReport();
            List<ENT.HeatStartStopReportApi> lstResult = new List<ENT.HeatStartStopReportApi>();
            List<ENT.DryrunHeatReportApi> lstDryRun = new List<ENT.DryrunHeatReportApi>();

            // if getting INSERT flag from dependency then we process for insert in live database
            if (e.ChangeType.ToString().ToUpper() == "UPDATE")
            {
                // get data from local database
                lstResult = new DAL.HeatStartStopReport().GetHeatStartStopReportByDate(DateTime.Now.ToString("dd-MMM-yyyy"), e.Entity.ID); 
                if (lstResult.Count > 0)
                {
                    Console.WriteLine("Record Found : " + lstResult.Count);
                    WriteLog("Record Found : " + lstResult.Count);

                    // heat timming data
                    objResponse.ReportName = "HeatReport";
                    objResponse.PowerOn = Convert.ToString(lstResult[0].StartDataTime);
                    objResponse.SuperHeat = Convert.ToString(lstResult[0].StartDataTime);
                    objResponse.HeatTapped = Convert.ToString(lstResult[0].EndDataTime);
                    objResponse.TapToTapHrsMin = Convert.ToString(lstResult[0].HrsMin);

                    // heat unit consumption data
                    objResponse.KwhrAtStart = Convert.ToString(lstResult[0].DataValue2); // Datavalue Start Unit
                    objResponse.KwhrAtEnd = Convert.ToString(lstResult[0].DataValue); // Datavalue End Unit
                    objResponse.TotalKwhr = Convert.ToString(lstResult[0].UnitDifference);
                    objResponse.KwhrHeat = Convert.ToString(lstResult[0].UnitDifference * 10);

                    lstDryRun = new DAL.HeatStartStopReport().GetHeatDryrunReportByDate(DateTime.Now.ToString("dd-MMM-yyyy"), e.Entity.ID);
                    if (lstDryRun.Count > 0)
                    {
                        Console.WriteLine("Dryrun Record Found : " + lstDryRun.Count);
                        WriteLog("Dryrun Record Found : " + lstDryRun.Count);

                        // dryrun heat timming data
                        objDryrun.ReportName = "HeatDryrunReport";
                        objDryrun.PowerOn = Convert.ToString(lstDryRun[0].StartDataTime);
                        objDryrun.SuperHeat = Convert.ToString(lstDryRun[0].StartDataTime);
                        objDryrun.HeatTapped = Convert.ToString(lstDryRun[0].EndDataTime);
                        objDryrun.TapToTapHrsMin = Convert.ToString(lstDryRun[0].HrsMin);

                        // dryrun heat unit consumption data
                        objDryrun.KwhrAtStart = Convert.ToString(lstDryRun[0].DataValueFirst); // Datavalue Start Unit
                        objDryrun.KwhrAtEnd = Convert.ToString(lstDryRun[0].DataValueLast); // Datavalue End Unit
                        objDryrun.TotalKwhr = Convert.ToString(lstDryRun[0].UnitDifference);
                        objDryrun.KwhrHeat = Convert.ToString(lstDryRun[0].UnitDifference * 10);
                    }

                    ENT.HeatReport objHR = new ENT.HeatReport();
                    objHR.ReportType = "1";
                    objHR.HeatUnitReport = objResponse;
                    objHR.HeatDryrunReport = objDryrun;

                    // create json for send data to mobile app
                    jsonNotification = Newtonsoft.Json.JsonConvert.SerializeObject(objHR);
                    WriteLog(jsonNotification);

                    // here, call the function for send data from local to live database
                    string result = SendHeatStartStopReportData(e.Entity, jsonNotification);

                    // write log of return response from API
                    Console.WriteLine(result);
                    WriteLog(result);
                }
                else
                {
                    Console.WriteLine("Record Not Found For Notification : " + lstResult.Count + ". Heat Started Now.");
                    WriteLog("Record Not Found For Notification : " + lstResult.Count + ". Heat Started Now.");
                }
            }

            Console.WriteLine("*************************************************");
            Console.WriteLine("Waiting for receiving new notifications...");
            Console.WriteLine("*************************************************");
            WriteLog("*************************************************");
        }

        /// <summary>
        /// this function is used to send data from local to live database.
        /// </summary>
        /// <param name="obj">object of HeatStartStopReport data</param>
        /// <returns></returns>
        private static string SendHeatStartStopReportData(ENT.HeatStartStopReport obj, string json)
        {
            string message = string.Empty;
            try
            {
                // this is our live api url for send data to live database
                WebRequest tRequest = WebRequest.Create("http://gisrv.appsmith.co.in/api/HeatReport/InsertHeatStartStopReportAPI");
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                obj.heat_json = json;

                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);

                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();
                                message = "HeatStartStopReport : " + responseFromFirebaseServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
            return message;
        }

        /// <summary>
        /// this function is not in use
        /// </summary>
        /// <returns></returns>
        private static List<ENT.Device> GetDeviceTokenList()
        {
            //ENT.TokenJsonResponse  TokenList = new ENT.TokenJsonResponse();
            List<ENT.Device> TokenList = new List<ENT.Device>();
            try
            {
                // Create a request for the URL.   
                WebRequest request = WebRequest.Create("http://gisrv.appsmith.co.in/api/Spectro/getDeviceID");

                // Get the response.  
                WebResponse response = request.GetResponse();

                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);

                // Read the content.  
                string responseFromServer = reader.ReadToEnd();

                var lstToken = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ENT.Device>>(responseFromServer);

                // asign ICollection list to cast device token list 
                TokenList = (List<ENT.Device>)lstToken;

                // Display the content.  
                //Console.WriteLine(responseFromServer);

                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return TokenList;
            }
            return TokenList;
        }

        /// <summary>
        /// this function is not in use
        /// </summary>
        /// <returns></returns>
        private static string SendNotification(string jsonNotification)
        {
            string ResMessage = string.Empty;
            try
            {
                #region Send Spectro Data To FCM

                ENT.FCMRootObject FCMData = new ENT.FCMRootObject();
                ENT.Notification NotificationBody = new ENT.Notification();
                ENT.Data NotificationData = new ENT.Data();

                int count = 0;
                List<ENT.Device> lstENT = GetDeviceTokenList();
                Console.WriteLine(lstENT.Count + " Device Found.");
                WriteLog(lstENT.Count + " Device Found.");

                for (int i = 0; i < lstENT.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lstENT[i].DeviceId))
                    {
                        FCMData.to = lstENT[i].DeviceId;
                        NotificationData.Description = jsonNotification;
                        NotificationBody.title = "GI ADMIN";
                        NotificationBody.body = jsonNotification;
                        FCMData.data = NotificationData;
                        // FCMData.notification = NotificationBody;

                        ENT.FCMResponse s = new ENT.FCMSender().SendClientNotification(FCMData);
                        if (s.success > 0)
                        {
                            count++;
                        }
                    }
                }
                ResMessage = count.ToString() + " notification send successfull out of " + lstENT.Count + ".";

                #endregion
            }
            catch (Exception ex)
            {
                ResMessage = ex.Message;
                return ResMessage;
            }
            return ResMessage;
        }

        /// <summary>
        /// this function is used to write log in text file.
        /// </summary>
        /// <param name="content">any type of string message</param>
        static void WriteLog(string content)
        {
            String fname = Path.Combine(Properties.Settings.Default.logPath, DateTime.Now.ToString("yyyyMMdd") + ".txt");
            StreamWriter writer = new StreamWriter(fname, true);
            writer.WriteLine(DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt") + " => " + content);
            writer.Close();
        }

        /// <summary>
        /// this function is used to delete old log file from Log Folder which is 30 days old file.
        /// </summary>
        private static bool DeleteOldLogFile(string path)
        {
            bool result = false;
            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        if (File.GetCreationTime(file) < DateTime.Now.AddDays(-15))
                        {
                            File.Delete(file);
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
                WriteLog(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// this function is used to send dryrun heat unit report
        /// </summary>
        /// <param name="DryrunID"></param>
        /// <returns></returns>
        private static string SendDryrunHeatReport(Int64 DryrunID)
        {
            try
            {
                string jsonNotification = string.Empty;
                ENT.ResponseHeatReport objHeatReport = new ENT.ResponseHeatReport();
                ENT.ResponseHeatReport objDryrun = new ENT.ResponseHeatReport();
                ENT.HeatStartStopReport objHSSR = new ENT.HeatStartStopReport();
                List<ENT.DryrunHeatReportApi> lstDryRun = new List<ENT.DryrunHeatReportApi>();

                lstDryRun = new DAL.HeatStartStopReport().GetHeatDryrunReportByDate(DateTime.Now.ToString("dd-MMM-yyyy"), DryrunID);
                if (lstDryRun.Count > 0)
                {
                    Console.WriteLine("Dryrun Record Found : " + lstDryRun.Count);
                    WriteLog("Dryrun Record Found : " + lstDryRun.Count);

                    // heat timming data for temp not in use
                    objHeatReport.ReportName = "HeatReport";
                    objHeatReport.PowerOn = Convert.ToString(lstDryRun[0].StartDataTime);
                    objHeatReport.SuperHeat = Convert.ToString(lstDryRun[0].StartDataTime);
                    objHeatReport.HeatTapped = Convert.ToString(lstDryRun[0].StartDataTime);
                    objHeatReport.TapToTapHrsMin = "0:0:0";

                    // heat unit consumption data for temp not in use
                    objHeatReport.KwhrAtStart = "0.00"; 
                    objHeatReport.KwhrAtEnd = "0.00"; 
                    objHeatReport.TotalKwhr = "0.00";
                    objHeatReport.KwhrHeat = "0.00";

                    // dryrun heat timming data 
                    objDryrun.ReportName = "HeatDryrunReport";
                    objDryrun.PowerOn = Convert.ToString(lstDryRun[0].StartDataTime);
                    objDryrun.SuperHeat = Convert.ToString(lstDryRun[0].StartDataTime);
                    objDryrun.HeatTapped = Convert.ToString(lstDryRun[0].EndDataTime);
                    objDryrun.TapToTapHrsMin = Convert.ToString(lstDryRun[0].HrsMin);

                    // dryrun heat unit consumption data 
                    objDryrun.KwhrAtStart = Convert.ToString(lstDryRun[0].DataValueFirst); // Datavalue Start Unit
                    objDryrun.KwhrAtEnd = Convert.ToString(lstDryRun[0].DataValueLast); // Datavalue End Unit
                    objDryrun.TotalKwhr = Convert.ToString(lstDryRun[0].UnitDifference);
                    objDryrun.KwhrHeat = Convert.ToString(lstDryRun[0].UnitDifference * 10);

                    // This object value is non use but i have pass for api so created this temp
                    objHSSR.ID = lstDryRun[0].ID;
                    objHSSR.StartDataTime = lstDryRun[0].StartDataTime;
                    objHSSR.EndDataTime = lstDryRun[0].EndDataTime;
                    objHSSR.DataValue = lstDryRun[0].DataValueFirst;
                    objHSSR.DataValue2 = lstDryRun[0].DataValueLast;
                }

                ENT.HeatReport objHR = new ENT.HeatReport();
                objHR.ReportType = "2";
                objHR.HeatUnitReport = objHeatReport;
                objHR.HeatDryrunReport = objDryrun;

                // create json for send data to mobile app
                jsonNotification = Newtonsoft.Json.JsonConvert.SerializeObject(objHR);
                WriteLog(jsonNotification);
                

                // here, call the function for send data from local to live database
                string result = SendHeatStartStopReportData(objHSSR, jsonNotification);

                return result;
            }
            catch (Exception ex)
            {
                return "SendDryrunHeatReport(): " + ex.Message.ToString();
            }
        }

    }
}

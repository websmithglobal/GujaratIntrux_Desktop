using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DAL = GI.COMPORT.DAL;
using ENT = GI.COMPORT.ENTITY;

namespace GI.COMPORT
{
    public partial class frmComPort : Form
    {
        SerialPort mySerialPort = new SerialPort();
        ENT.FurnaceSwitch objENTFur = new ENT.FurnaceSwitch();
        List<ENT.FurnaceSwitch> lstENTFur = new List<ENT.FurnaceSwitch>();
        DAL.FurnaceSwitch objDALFur = new DAL.FurnaceSwitch();

        int CURR_POUR_STATUS = 0;
        int CURR_FUR_NO = 0;
        string CURR_FUR_NAME = "";

        public frmComPort()
        {
            InitializeComponent();
        }

        private void ComPort_Load(object sender, EventArgs e)
        {
            try
            {
                string logFilePath = Path.Combine(Application.StartupPath, "Log");

                // Craete directoty if not exist log folder
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }

                // this function delete files which is created before 15 days.
                bool result = DeleteOldLogFile(logFilePath);

                lblPortName.Text = ""; lblCon.Text = "";
                button1.Enabled = true;
                button2.Enabled = false;
                tbLog.Clear();
                button1_Click(button1, new EventArgs());
            }
            catch (Exception ex)
            {
                this.SetText("Error => " + ex.Message.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = "COMPortLog" + DateTime.Now.ToString("dd-MM-yyyy");
            try
            {
                // configuratoin of COM port
                mySerialPort.PortName = Properties.Settings.Default.PortName;
                mySerialPort.BaudRate = Convert.ToInt32(Properties.Settings.Default.BaudRate);
                mySerialPort.DataBits = Convert.ToInt32(Properties.Settings.Default.DataBits);
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.Handshake = Handshake.None;
                mySerialPort.Open();
                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                button1.Enabled = false;
                button2.Enabled = true;
                lblPortName.Text = "PORT NAME : " + Properties.Settings.Default.PortName;
                lblCon.Text = mySerialPort.IsOpen ? "Connected" : "Not Connect";
                lblCon.ForeColor = mySerialPort.IsOpen ? Color.Green : Color.Red;
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                {
                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> ERROR >> " + ex.Message + "");
                }
                this.SetText("Error => " + ex.Message.ToString() + Environment.NewLine);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string fileName = "COMPortLog" + DateTime.Now.ToString("dd-MM-yyyy");
            try
            {
                this.Invoke((Action)delegate
                {
                    SerialPort sp = (SerialPort)sender;
                    string indata = sp.ReadLine();
                    string[] words = indata.Split('|');

                    if (words.Length >= 3)
                    {
                        CURR_FUR_NAME = Convert.ToString(words[0]).Trim();
                        CURR_FUR_NO = Convert.ToInt32(words[1]);
                        CURR_POUR_STATUS = Convert.ToString(words[2]).Trim() == "POURSTOP" ? Convert.ToInt32(DAL.MyEnum.PourStatus.POUR_STOP) : Convert.ToInt32(DAL.MyEnum.PourStatus.POUR_START);

                        if (CURR_FUR_NAME.Equals("FURNACE2") && CURR_FUR_NO.Equals(1))
                        {
                            SetText(CURR_FUR_NAME + "|" + CURR_FUR_NO + "|" + Convert.ToString(words[2]) + "|True");
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                            {
                                file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> STRING >> " + indata + "");
                            }
                        }

                        objENTFur.Mode = "GetTopOne";
                        objENTFur.fur_name = CURR_FUR_NAME.Trim();
                        objENTFur.fur_no = CURR_FUR_NO;
                        objENTFur.fur_status = CURR_POUR_STATUS;
                        lstENTFur = objDALFur.GetFurnaceSwitch(objENTFur);

                        if (lstENTFur.Count > 0)
                        {
                            if (lstENTFur[0].fur_status != CURR_POUR_STATUS)
                            {
                                // get last time from unit file.
                                SetText(getDateFromCSVFile().ToString("dd/MMM/yyyy hh:mm:ss tt") + " Date From File.");
                                objENTFur.fur_file_time = getDateFromCSVFile().ToString("dd/MMM/yyyy hh:mm:ss tt");
                                objENTFur.fur_id = Guid.NewGuid();
                                objENTFur.Mode = "ADD";

                                if (objDALFur.InsertUpdateDeleteFurnaceSwitch(objENTFur))
                                {
                                    SetText(indata + "|True|InsertSuccess");
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                                    {
                                        file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> STRING >> " + indata + "|True|InsertSuccess");
                                    }
                                }
                                else
                                {
                                    SetText(indata + "|False|InsertFail");
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                                    {
                                        file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> STRING >> " + indata + "|False|InsertFail");
                                    }
                                }

                                if (CURR_POUR_STATUS == 1)
                                {
                                    // get top one record by id for updated
                                    List<ENT.HeatStartStopReport> lstHSSR = new List<ENT.HeatStartStopReport>();
                                    lstHSSR = new DAL.HeatStartStopReport().GetHeatStartStopReport(new ENT.HeatStartStopReport { Mode = "GetByTopOneUpdate" });
                                    if (lstHSSR.Count > 0)
                                    {
                                        // when status is 0 then update record one time
                                        if (lstHSSR[0].isupdated == 0)
                                        {
                                            try
                                            {
                                                // get last record from MeterSlaveMaster for update into HeatStartStopReport table
                                                List<ENT.MeterSlaveMaster> lstMeter = new List<ENT.MeterSlaveMaster>();
                                                lstMeter = new DAL.MeterSlaveMaster().GetTopOneRecord(new ENT.MeterSlaveMaster { Mode = "GetByTopOne" });
                                                if (lstMeter.Count > 0)
                                                {
                                                    ENT.HeatStartStopReport objHSSR = new ENT.HeatStartStopReport();
                                                    objHSSR = new ENT.HeatStartStopReport();
                                                    objHSSR.ID = lstHSSR[0].ID;
                                                    objHSSR.fur_name = lstENTFur[0].fur_name;
                                                    objHSSR.fur_no = lstENTFur[0].fur_no;
                                                    objHSSR.fur_status_stop = lstENTFur[0].fur_status;
                                                    objHSSR.fur_open_time = Convert.ToDateTime(lstENTFur[0].fur_open_time);
                                                    objHSSR.fur_close_time = Convert.ToDateTime(lstENTFur[0].fur_close_time);
                                                    objHSSR.LineCountEnd = lstMeter[0].LineCount;
                                                    objHSSR.EndDataTime = lstMeter[0].DataTime;
                                                    objHSSR.DataValue = lstMeter[0].DataValue;
                                                    objHSSR.isupdated = 1;
                                                    objHSSR.Mode = "UPDATE";
                                                    if (new DAL.HeatStartStopReport().UpdateHeatStartStopReport(objHSSR))
                                                    {
                                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                                                        {
                                                            file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> FILE_NAME >> " + lstMeter[0].FileName + " >> " + " >> LINE_NO >> " + objHSSR.LineCountEnd + " >>  Updated For Report.");
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                                                {
                                                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> ERROR >> " + ex.Message + "");
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (CURR_POUR_STATUS == 0)
                                {
                                    // get record from database where isupdated=0 if not found any record then insert one record with isupdated=0
                                    List<ENT.HeatStartStopReport> lstHSSR = new List<ENT.HeatStartStopReport>();
                                    lstHSSR = new DAL.HeatStartStopReport().GetHeatStartStopReport(new ENT.HeatStartStopReport { Mode = "GetByTopOneInsert" });
                                    if (lstHSSR.Count == 0)
                                    {
                                        // get last record from MeterSlaveMaster for insert into HeatStartStopReport table
                                        List<ENT.MeterSlaveMaster> lstMeter = new List<ENT.MeterSlaveMaster>();
                                        lstMeter = new DAL.MeterSlaveMaster().GetTopOneRecord(new ENT.MeterSlaveMaster { Mode = "GetByTopOne" });
                                        if (lstMeter.Count > 0)
                                        {
                                            ENT.HeatStartStopReport objHSSR = new ENT.HeatStartStopReport();
                                            objHSSR.LineCountStart = lstMeter[0].LineCount;
                                            objHSSR.LineCountEnd = lstMeter[0].LineCount;
                                            objHSSR.StartDataTime = lstMeter[0].DataTime;
                                            objHSSR.EndDataTime = lstMeter[0].DataTime;
                                            objHSSR.DataValue = lstMeter[0].DataValue;
                                            objHSSR.DataValue2 = lstMeter[0].DataValue;
                                            objHSSR.FileName = lstMeter[0].FileName;
                                            objHSSR.EntryDate = DateTime.Now;
                                            objHSSR.isupdated = 0;
                                            objHSSR.Mode = "ADD";
                                            if (new DAL.HeatStartStopReport().InsertHeatStartStopReport(objHSSR))
                                            {
                                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                                                {
                                                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> FILE_NAME >> " + objHSSR.FileName + " >> " + " LINE_NO >> " + objHSSR.LineCountStart + " >>  Inserted For Report.");
                                                }
                                            }

                                            // Insert First record for dryrun heat report
                                            ENT.DryrunHeatReport objHDRH = new ENT.DryrunHeatReport();
                                            try
                                            {
                                                objHDRH.LineCountStart = lstMeter[0].LineCount;
                                                objHDRH.LineCountEnd = lstMeter[0].LineCount;
                                                objHDRH.StartDataTime = lstMeter[0].DataTime;
                                                objHDRH.EndDataTime = lstMeter[0].DataTime;
                                                objHDRH.DataValueLast = lstMeter[0].DataValue;
                                                objHDRH.DataValueFirst = lstMeter[0].DataValue;
                                                objHDRH.FileName = lstMeter[0].FileName;
                                                objHDRH.EntryDate = DateTime.Now;
                                                objHDRH.IsUpdated = 0;
                                                objHDRH.Mode = "ADD";
                                                if (new DAL.DryrunHeatReport().InsertUpdateDeleteDryrunHeatReport(objHDRH))
                                                {
                                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                                                    {
                                                        file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> FILE_NAME >> " + objHDRH.FileName + " >> " + " LINE_NO >> " + objHDRH.LineCountStart + " >>  Inserted For Dryrun Report.");
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\ErrorLog.txt", true))
                                                {
                                                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> FILE_NAME >> " + objHDRH.FileName + " >> " + " >> LINE_NO >> " + objHDRH.LineCountStart + " >> " + ex.Message.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // get last time from unit file.
                            SetText(getDateFromCSVFile().ToString("dd/MMM/yyyy hh:mm:ss tt") + " Date From File.");
                            objENTFur.fur_file_time = getDateFromCSVFile().ToString("dd/MMM/yyyy hh:mm:ss tt");
                            objENTFur.fur_id = Guid.NewGuid();
                            objENTFur.Mode = "ADD";
                            if (objDALFur.InsertUpdateDeleteFurnaceSwitch(objENTFur))
                            {
                                SetText(indata + "|True|FirstInsertSuccess");
                            }
                            else
                            {
                                SetText(indata + "|False|FirstInsertFail");
                            }
                        }
                    }
                    else
                    {
                        SetText(indata + "|NA");
                    }
                });
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                {
                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> ERROR >> " + ex.Message + "");
                }
                this.SetText("Error => " + ex.Message.ToString());
            }
        }

        void SetText(string PortText)
        {
            try
            {
                tbLog.Invoke((Action)delegate
                {
                    if (tbLog.Lines.Count() > 100)
                    {
                        tbLog.Clear();
                    }
                    tbLog.AppendText(PortText + "|" + DateTime.Now.ToString("hh:mm:ss tt") + Environment.NewLine); // + Environment.NewLine + Environment.NewLine
                    tbLog.Refresh(); tbLog.ScrollToCaret();
                });
            }
            catch (Exception ex)
            {
                tbLog.AppendText("Error => " + ex.Message.ToString() + Environment.NewLine);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (mySerialPort.IsOpen)
                {
                    Thread CloseDown = new Thread(new ThreadStart(CloseSerialOnExit)); //close port in new thread to avoid hang
                    CloseDown.Start(); //close port in new thread to avoid hang
                }
                button1.Enabled = true;
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                this.SetText("Error => " + ex.Message.ToString() + Environment.NewLine);
            }
        }

        private void CloseSerialOnExit()
        {
            try
            {
                mySerialPort.Close(); //close the serial port
            }
            catch (Exception ex)
            {
                this.SetText("Error => " + ex.Message.ToString() + Environment.NewLine);
            }
        }

        private DateTime getDateFromCSVFile()
        {
            DateTime dtTime = DateTime.Now;
            string fileName = "COMPortLog" + DateTime.Now.ToString("dd-MM-yyyy");
            try
            {
                string strFilename = "01SELEC" + DateTime.Now.ToString("yyMMdd") + ".csv";
                string copyToFilePath = Path.Combine(Application.StartupPath, strFilename);
                string[] copyFromFilePath = File.ReadAllLines(Path.Combine(Application.StartupPath, "DownloadFilePath.txt"));

                #region Copy File From FTP Folder
                if (File.Exists(copyToFilePath))
                {
                    if (File.Exists(Path.Combine(copyFromFilePath[0], strFilename)))
                    {
                        File.Delete(copyToFilePath);
                        File.Copy(Path.Combine(copyFromFilePath[0], strFilename), copyToFilePath);
                    }
                    else
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                        {
                            file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> File Read >> File not generated by machine, Please check machine status.");
                        }
                    }
                }
                else
                {
                    if (File.Exists(Path.Combine(copyFromFilePath[0], strFilename)))
                    {
                        File.Copy(Path.Combine(copyFromFilePath[0], strFilename), copyToFilePath);
                    }
                    else
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                        {
                            file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> File Read >> File not generated by machine, Please check machine status.");
                        }
                    }
                }
                #endregion

                DataTable dtCSV = new DataTable();
                if (File.Exists(copyToFilePath))
                {
                    dtCSV = DAL.MeterSlaveMaster.GetDataTableFromCSVFile((Path.Combine(Application.StartupPath, strFilename)));
                }
                else
                {
                    return dtTime;
                }

                int RC = dtCSV.Rows.Count;
                if (RC > 0)
                {
                    string date = dtCSV.Rows[RC - 1][0].ToString().Substring(0, 8);
                    string[] arrDate = date.Split('-');
                    DateTime dtDate = new DateTime(2000 + Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]), Convert.ToInt32(arrDate[2]), 0, 0, 0);

                    string time = dtCSV.Rows[RC - 1][0].ToString().Substring(9, 8);
                    string[] arrTime = time.Split(':');
                    dtTime = new DateTime(2000 + Convert.ToInt32(arrDate[0]), Convert.ToInt32(arrDate[1]), Convert.ToInt32(arrDate[2]), Convert.ToInt32(arrTime[0]), Convert.ToInt32(arrTime[1]), Convert.ToInt32(arrTime[2]));
                }
                return dtTime;
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\" + fileName + ".txt", true))
                {
                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> ERROR >> " + ex.Message + "");
                }
                return DateTime.Now;
            }
        }

        private void GetNotificationData(Int64 ID)
        {
            ENT.ResponseHeatReport objResponse = new ENT.ResponseHeatReport();
            List<ENT.HeatStartStopReportApi> lstResult = new List<ENT.HeatStartStopReportApi>();
            lstResult = new DAL.HeatStartStopReport().GetHeatStartStopReportByDate(DateTime.Now.ToString("dd-MMM-yyyy"), ID);
            if (lstResult.Count > 0)
            {
                // heat timming data
                objResponse.ReportName = "HeatReport";
                objResponse.PowerOn = Convert.ToString(lstResult[0].StartDataTime);
                objResponse.SuperHeat = Convert.ToString(lstResult[0].StartDataTime);
                objResponse.HeatTapped = Convert.ToString(lstResult[0].EndDataTime);
                objResponse.TapToTapHrsMin = Convert.ToString(lstResult[0].HrsMin);

                // heat unit consumption data
                objResponse.KwhrAtStart = Convert.ToString(lstResult[0].DataValue);
                objResponse.KwhrAtEnd = Convert.ToString(lstResult[0].DataValue2);
                objResponse.TotalKwhr = Convert.ToString(lstResult[0].UnitDifference);
                objResponse.KwhrHeat = Convert.ToString(lstResult[0].UnitDifference * 10);

                // create json for send data to mobile app
                string jsonNotification = Newtonsoft.Json.JsonConvert.SerializeObject(objResponse);

                ENT.HeatStartStopReport objENT = new ENT.HeatStartStopReport();

                // here, call the function for send data from local to live database
                string result = SendHeatStartStopReportData(objENT, jsonNotification);

            }
        }

        private string SendHeatStartStopReportData(ENT.HeatStartStopReport obj, string json)
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

        private bool DeleteOldLogFile(string logFilePath)
        {
            bool result = false;
            try
            {
                string[] files = Directory.GetFiles(logFilePath);
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
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\Log\\COMPortLog" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true))
                {
                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> ERROR >> " + ex.Message + "");
                }
            }
            return result;
        }
    }
}

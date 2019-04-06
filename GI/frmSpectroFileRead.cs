using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ENT = GI.ENTITY;

namespace GI
{
    public partial class frmSpectroFileRead : Form
    {
        System.Timers.Timer objTime = new System.Timers.Timer();
        private const decimal constHexToDec8 = 8388608;
        private const decimal constHexToDec4 = 4194304;
        string line = string.Empty;
        string strNetworkPath = string.Empty;
        string strRenameFile = string.Empty;
        int tmInerval = 1000;
        int cntInsert = 0;
        string conStr;
        SqlConnection conn;

        public frmSpectroFileRead()
        {
            InitializeComponent();
            conStr = GI.Properties.Settings.Default.ConnectionString;
            conn = new SqlConnection(conStr);
        }
        
        private void ReadSpectroFile()
        {
            try
            {
                string[] copyFromFilePath = File.ReadAllLines(Path.Combine(Application.StartupPath, "DownloadSpectroFilePath.txt"));
                strNetworkPath = copyFromFilePath[0];
                tmInerval = Convert.ToInt32(copyFromFilePath[1]);
                objTime.Interval = tmInerval;
                var lstCsvFile = new DirectoryInfo(strNetworkPath).GetFiles("*.csv");
                string latestFile = string.Empty;
                DateTime lastUpdated = DateTime.MinValue;
                foreach (FileInfo file in lstCsvFile)
                {
                    if (!file.Name.Contains("ReadedFile"))
                    {
                        latestFile = file.Name;
                    }
                }

                if (string.IsNullOrEmpty(latestFile))
                {
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\ErrorLog.txt", true))
                    //{
                    //    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> CSV FILE NOT FOUND.");
                    //}
                    return;
                }
                string strFullPath = Path.Combine(strNetworkPath, latestFile);

                // Read CSV File For Data Table
                DataTable dtCSV = DAL.MeterSlaveMaster.GetDataTableFromCSVFile(strFullPath);

                if (File.Exists(strFullPath))
                {
                    string readFileName = latestFile.Replace(latestFile, "ReadedFile-" + DateTime.Now.ToString("dd-MM-yy-hh-mm-ss") + ".csv");
                    strRenameFile = Path.Combine(strNetworkPath, readFileName);
                    File.Move(strFullPath, strRenameFile);
                }

                //MessageBox.Show(strFullPath + Environment.NewLine + strRenameFile);
                try
                {
                    ENT.SpectroMaster objEnt = new ENT.SpectroMaster();
                    for (int i = 0; i < dtCSV.Columns.Count-2; i++)
                    {
                        if (dtCSV.Columns[i].ColumnName.Trim() == "Date")
                        {
                            DateTime dt = DateTime.Now;
                            string[] dtDate = Convert.ToString(dtCSV.Rows[0]["Date"]).Split('/');
                            if (dtDate.Length == 3)
                            {
                                dt = new DateTime(Convert.ToInt32(dtDate[2]), Convert.ToInt32(dtDate[0]), Convert.ToInt32(dtDate[1]));
                            }
                            objEnt.SpectroDate = dt.ToString("dd/MMM/yyyy");
                        }
                        else if (dtCSV.Columns[i].ColumnName.Trim() == "Quality")
                        {
                            objEnt.Quality = Convert.ToString(dtCSV.Rows[0]["Quality"]);
                        }
                        else if (dtCSV.Columns[i].ColumnName.Trim() == "ID-3")
                        {
                            objEnt.Grade = Convert.ToString(dtCSV.Rows[0]["ID-3"]);
                        }
                        else if (dtCSV.Columns[i].ColumnName.Trim() == "SampleNo")
                        {
                            objEnt.SampleNo = Convert.ToString(dtCSV.Rows[0]["SampleNo"]);
                        }
                    }
                    if (objEnt != null)
                    {
                        Int64 SpectroNo = new DAL.SpectroMaster().GetTopOneSpectroNo()+1;
                        string QueryString = "INSERT INTO SpectroMaster (SpectroID,SpectroNo,SpectroDate,Quality,Grade,SampleNo,EntryDate) values ('" + Guid.NewGuid() + "',"+ SpectroNo + ",'" + objEnt.SpectroDate + "','" + objEnt.Quality + "','" + objEnt.Grade + "','" + objEnt.SampleNo + "',GETDATE())";
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();

                        SqlCommand cmdInsert = new SqlCommand();
                        cmdInsert.CommandText = QueryString;
                        cmdInsert.CommandType = CommandType.Text;
                        cmdInsert.Connection = conn;

                        if (cmdInsert.ExecuteNonQuery() > 0)
                        {
                            if (conn.State == ConnectionState.Open)
                                conn.Close();

                            for (int j = 3; j < dtCSV.Columns.Count - 3; j++)
                            {
                                string strQuery = "INSERT INTO SpectroEntry (SpectroID,SpectroNo,KeyName,KeyValue,EntryDate) values ('" + Guid.NewGuid() + "'," + SpectroNo + ",'" + Convert.ToString(dtCSV.Columns[j].ColumnName) + "','" + Convert.ToString(dtCSV.Rows[0][j]) + "',GETDATE())";
                                if (conn.State == ConnectionState.Closed)
                                    conn.Open();

                                SqlCommand cmdIns = new SqlCommand();
                                cmdIns.CommandText = strQuery;
                                cmdIns.CommandType = CommandType.Text;
                                cmdIns.Connection = conn;
                                cmdIns.ExecuteNonQuery();

                                if (conn.State == ConnectionState.Open)
                                    conn.Close();
                            }
                        }

                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\ErrorLog.txt", true))
                    {
                        file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> FILE_NAME >> " + strRenameFile + " >> " + ex.Message.ToString());
                    }

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    notifyIcon1.BalloonTipText = "Error : " + ex.Message.ToString();
                    notifyIcon1.ShowBalloonTip(2000);
                    objTime.Start();
                }
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\ErrorLog.txt", true))
                {
                    file.WriteLine("Date >> " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + " >> FILE_NAME >> " + strRenameFile + " >> " + ex.Message.ToString());
                }

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                notifyIcon1.BalloonTipText = "Error : " + ex.Message.ToString();
                notifyIcon1.ShowBalloonTip(2000);
                objTime.Start();
            }
        }

        private void frmSpectroFileRead_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "Spectro Data Reading Application Started.";
            notifyIcon1.ShowBalloonTip(2000);
        }

        private void ObjTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadSpectroFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                objTime = new System.Timers.Timer();
                objTime.Elapsed += new System.Timers.ElapsedEventHandler(ObjTime_Elapsed);
                objTime.Interval = tmInerval;
                objTime.Enabled = true;
                objTime.AutoReset = true;
                objTime.Start();
                notifyIcon1.BalloonTipText = "Spectro File Reading Started.";
                notifyIcon1.ShowBalloonTip(2000);
                //ReadSpectroFile();
            }
            catch (Exception ex)
            {
                notifyIcon1.BalloonTipText = "Error : " + ex.Message.ToString();
                notifyIcon1.ShowBalloonTip(2000);
                objTime.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            objTime.Stop();
            objTime.Enabled = false;
            notifyIcon1.BalloonTipText = "File Reading Is Stoped.";
            notifyIcon1.ShowBalloonTip(2000);
        }
        
    }
}

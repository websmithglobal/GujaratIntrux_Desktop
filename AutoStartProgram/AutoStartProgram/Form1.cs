using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoStartProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                WriteToFile("Auto Start Application Process Started.");

                #region Send Notification Application

                string ReadNotification = Properties.Settings.Default.ReadNotification.ToString();
                string ReadNotificationExeName = Properties.Settings.Default.ReadNotificationExeName.ToString();

                var appNotificationProcess = Process.GetProcessesByName(ReadNotificationExeName);
                if (appNotificationProcess.Length == 0)
                {
                    Process.Start(@"cmd.exe", @"/k "+ ReadNotification + "");
                    WriteToFile("Notification Application Started.");
                }
                else
                {
                    WriteToFile("Notification Application Running.");
                }

                #endregion

                #region Read Com Port Application

                string ReadComPort = Properties.Settings.Default.ReadComPort.ToString();
                string ReadComPortExeName = Properties.Settings.Default.ReadComPortExeName.ToString();

                var appComPortProcess = Process.GetProcessesByName(ReadComPortExeName);
                if (appComPortProcess.Length == 0)
                {
                    Process.Start(ReadComPort);
                    WriteToFile("Com Port Application Started.");
                }
                else
                {
                    WriteToFile("Com Port Application Running.");
                }

                #endregion

                #region Read Unit File Application

                string ReadFile = Properties.Settings.Default.ReadFile.ToString();
                string ReadFileExeName = Properties.Settings.Default.ReadFileExeName.ToString();

                var appReadFileProcess = Process.GetProcessesByName(ReadFileExeName);
                if (appReadFileProcess.Length == 0)
                {
                    Process.Start(ReadFile);
                    WriteToFile("Read Unit File Application Started.");
                }
                else
                {
                    WriteToFile("Read Unit File Application Running.");
                }

                #endregion

                WriteToFile("Auto Start Application Process Comleted.");
                WriteToFile("========XXX========XXX========");
                DeleteOldLogFile();
            }
            catch (Exception ex)
            {
                WriteToFile(ex.Message);
                Application.Exit();
            }
            Application.Exit();
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\MyLogs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\MyLogs\\ServiceLog_" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt")} => {Message} ");
                }
            }
            else
            {
                // write into file which is already created.
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine($"{DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt")} => {Message} ");
                }
            }
        }

        private bool DeleteOldLogFile()
        {
            bool result = false;
            try
            {
                string[] files = Directory.GetFiles(Path.Combine(Application.StartupPath, "MyLogs"));
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

                string[] files1 = Directory.GetFiles(Path.Combine(Application.StartupPath, "MyLogs"));
                foreach (string file in files1)
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
                WriteToFile(ex.Message);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace GI
{
    public partial class frmDashboard : Form
    {
        System.Timers.Timer objTime = new System.Timers.Timer();
        System.Timers.Timer objCurrJob = new System.Timers.Timer();

        public frmDashboard()
        {
            InitializeComponent();
            lblTime.Text = string.Empty;
        }

        private void GetCurrentUnit()
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    txtCurrUnit.Text = "";
                    DataTable dtNew = new DAL.MeterSlaveMaster().GetMeterSlaveMaster();
                    if (dtNew.Rows.Count > 0)
                    {
                        txtCurrUnit.Text = Convert.ToString(dtNew.Rows[0][0]) + " Kwh";
                    }
                    else
                    {
                        txtCurrUnit.Text = "0 Kwh";
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private DataTable GetCurrentJob(string fromDate, string toDate, bool IsLive, string JobNo)
        {
            DataTable dtNew = new DataTable();
            try
            {
                this.Invoke((Action)delegate
                {
                    dtNew = new DAL.JobEntry().GetCurrentJob(fromDate, toDate, IsLive, JobNo);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return dtNew;
        }

        private void SetCurrentJobData(DataTable dt)
        {
            try
            {
                clearLabel();
                if (dt.Rows.Count > 0)
                {
                    lblJobNo.Text = "Job No.: " + Convert.ToString(dt.Rows[0]["JobNo"]);
                    lblJobFur.Text = "Job Furnace No.: " + Convert.ToString(dt.Rows[0]["JobFurnaceNo"]);
                    lblStartTime.Text = "Start Time: " + Convert.ToString(dt.Rows[0]["StartTime"]);
                    lblEndTime.Text = "End Time: " + Convert.ToString(dt.Rows[0]["EndTime"]);

                    lblP1Date.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour1Time"])) ? "" : Convert.ToString(dt.Rows[0]["Pour1Time"]);
                    lblP2Date.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour2Time"])) ? "" : Convert.ToString(dt.Rows[0]["Pour2Time"]);
                    lblP3Date.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour3Time"])) ? "" : Convert.ToString(dt.Rows[0]["Pour3Time"]);
                    lblP4Date.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour4Time"])) ? "" : Convert.ToString(dt.Rows[0]["Pour4Time"]);
                    lblP5Date.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour5Time"])) ? "" : Convert.ToString(dt.Rows[0]["Pour5Time"]);
                    lblP6Date.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour6Time"])) ? "" : Convert.ToString(dt.Rows[0]["Pour6Time"]);

                    lblP1Time.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour1Time"])) ? "" : "Duration : " + Convert.ToString(dt.Rows[0]["TotalPour1HrsMinSec"]);
                    lblP2Time.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour2Time"])) ? "" : "Duration : " + Convert.ToString(dt.Rows[0]["TotalPour2HrsMinSec"]);
                    lblP3Time.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour3Time"])) ? "" : "Duration : " + Convert.ToString(dt.Rows[0]["TotalPour3HrsMinSec"]);
                    lblP4Time.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour4Time"])) ? "" : "Duration : " + Convert.ToString(dt.Rows[0]["TotalPour4HrsMinSec"]);
                    lblP5Time.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour5Time"])) ? "" : "Duration : " + Convert.ToString(dt.Rows[0]["TotalPour5HrsMinSec"]);
                    lblP6Time.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour6Time"])) ? "" : "Duration : " + Convert.ToString(dt.Rows[0]["TotalPour6HrsMinSec"]);

                    lblP1Unit.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour1Time"])) ? "" : "Unit : " + Convert.ToString(dt.Rows[0]["P1UnitTotal"]);
                    lblP2Unit.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour2Time"])) ? "" : "Unit : " + Convert.ToString(dt.Rows[0]["P2UnitTotal"]);
                    lblP3Unit.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour3Time"])) ? "" : "Unit : " + Convert.ToString(dt.Rows[0]["P3UnitTotal"]);
                    lblP4Unit.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour4Time"])) ? "" : "Unit : " + Convert.ToString(dt.Rows[0]["P4UnitTotal"]);
                    lblP5Unit.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour5Time"])) ? "" : "Unit : " + Convert.ToString(dt.Rows[0]["P5UnitTotal"]);
                    lblP6Unit.Text = string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour6Time"])) ? "" : "Unit : " + Convert.ToString(dt.Rows[0]["P6UnitTotal"]);

                    decimal TotalUnit = (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour1Time"])) ? 0 : Convert.ToDecimal(dt.Rows[0]["P1UnitTotal"].ToString())) +
                        (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour2Time"])) ? 0 : Convert.ToDecimal(dt.Rows[0]["P2UnitTotal"].ToString())) +
                        (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour3Time"])) ? 0 : Convert.ToDecimal(dt.Rows[0]["P3UnitTotal"].ToString())) +
                        (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour4Time"])) ? 0 : Convert.ToDecimal(dt.Rows[0]["P4UnitTotal"].ToString())) +
                        (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour5Time"])) ? 0 : Convert.ToDecimal(dt.Rows[0]["P5UnitTotal"].ToString())) +
                        (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Pour6Time"])) ? 0 : Convert.ToDecimal(dt.Rows[0]["P6UnitTotal"].ToString()));

                    lblTotalUnit.Text = "Total Unit: " + Convert.ToString(TotalUnit);

                    if (chkIsLive.Checked)
                        lblEndTime.Visible = false;
                    else
                        lblEndTime.Visible = true;
                }
                else
                { lblJobNo.Text = "Current Job Not Found."; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void clearLabel()
        {
            lblJobNo.Text = string.Empty;
            lblJobFur.Text = string.Empty;
            lblStartTime.Text = string.Empty;
            lblEndTime.Text = string.Empty;

            lblP1Date.Text = string.Empty;
            lblP2Date.Text = string.Empty;
            lblP3Date.Text = string.Empty;
            lblP4Date.Text = string.Empty;
            lblP5Date.Text = string.Empty;
            lblP6Date.Text = string.Empty;

            lblP1Time.Text = string.Empty;
            lblP2Time.Text = string.Empty;
            lblP3Time.Text = string.Empty;
            lblP4Time.Text = string.Empty;
            lblP5Time.Text = string.Empty;
            lblP6Time.Text = string.Empty;

            lblP1Unit.Text = string.Empty;
            lblP2Unit.Text = string.Empty;
            lblP3Unit.Text = string.Empty;
            lblP4Unit.Text = string.Empty;
            lblP5Unit.Text = string.Empty;
            lblP6Unit.Text = string.Empty;
            lblTotalUnit.Text = string.Empty;
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                clearLabel();
                txtSearchJobNo.Visible = false;
                lblSearchJobNo.Visible = false;

                double interval = Convert.ToDouble(GI.Properties.Settings.Default.IntervalUnit);
                GetCurrentUnit();
                objTime = new System.Timers.Timer();
                objTime.Elapsed += new System.Timers.ElapsedEventHandler(ObjTime_Elapsed);
                objTime.Interval = ((1000 * 60) * interval);  // Interval read from text file.
                objTime.Enabled = true;
                objTime.AutoReset = true;
                objTime.Start();

                double intervalJob = Convert.ToDouble(GI.Properties.Settings.Default.IntervalJob);
                objCurrJob = new System.Timers.Timer();
                objCurrJob.Elapsed += new System.Timers.ElapsedEventHandler(objCurrJob_Elapsed);
                objCurrJob.Interval = ((1000 * intervalJob) * 1);     // Five Second Interval
                objCurrJob.Enabled = true;
                objCurrJob.AutoReset = true;
                objCurrJob.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        
        private void tmrSysDate_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        private void ObjTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    GetCurrentUnit();
                });
            }
            catch (Exception)
            {
                objTime.Start();
            }
        }

        private void objCurrJob_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.Invoke((Action)delegate
                {
                    DataTable dtData = new DataTable();
                    if (string.IsNullOrEmpty(dtpFromOverall.Text) && string.IsNullOrEmpty(dtpToOverall.Text))
                    {
                        dtData = GetCurrentJob(string.Empty, string.Empty, chkIsLive.Checked, txtSearchJobNo.Text);
                    }
                    else
                    {
                        dtData = GetCurrentJob(dtpFromMonitoring.Value.ToString("dd/MMM/yyyy"), dtpToMonitoring.Value.ToString("dd/MMM/yyyy"), chkIsLive.Checked, txtSearchJobNo.Text);
                    }
                    SetCurrentJobData(dtData);
                });
            }
            catch (Exception)
            {
                objTime.Start();
            }
        }

        private void btnCurrentUnit_Click(object sender, EventArgs e)
        {
            frmViewData frmView = new frmViewData();
            frmView.ShowDialog();
        }
        
        private void btnViewSpectro_Click(object sender, EventArgs e)
        {
            frmViewSpectroData frmViewSpec = new frmViewSpectroData();
            frmViewSpec.ShowDialog();
        }

        private void btnShowMonitoring_Click(object sender, EventArgs e)
        {
            try
            {
                DAL.JobEntry objDAL = new DAL.JobEntry();
                DataTable dtData = new DataTable();
                if (string.IsNullOrEmpty(dtpFromMonitoring.Text) && string.IsNullOrEmpty(dtpToMonitoring.Text))
                {
                    dtData = objDAL.GetCurrentJob(string.Empty, string.Empty, chkIsLive.Checked, txtSearchJobNo.Text.Trim());
                }
                else
                {
                    dtData = objDAL.GetCurrentJob(dtpFromMonitoring.Value.ToString("dd/MMM/yyyy"), dtpToMonitoring.Value.ToString("dd/MMM/yyyy"), chkIsLive.Checked, txtSearchJobNo.Text.Trim());
                }
                if (dtData.Rows.Count > 0)
                { SetCurrentJobData(dtData); }
                else
                {
                    MessageBox.Show("Record Not Found For Selected Date.", "Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewJob_Click(object sender, EventArgs e)
        {
            frmViewJobEntry frmViewJob = new frmViewJobEntry();
            frmViewJob.ShowDialog();
        }

        private void btnShowOverall_Click(object sender, EventArgs e)
        {
            try
            {
                DAL.JobEntry objDAL = new DAL.JobEntry();
                DataTable dtData = new DataTable();
                if (string.IsNullOrEmpty(dtpFromOverall.Text) && string.IsNullOrEmpty(dtpToOverall.Text))
                {
                    dtData = objDAL.GetOverAllHeatFurnaceTime(string.Empty, string.Empty);
                }
                else
                {
                    dtData = objDAL.GetOverAllHeatFurnaceTime(dtpFromOverall.Value.ToString("dd/MMM/yyyy"), dtpToOverall.Value.ToString("dd/MMM/yyyy"));
                }

                dgvOverAll.DataSource = dtData;
                dgvOverAll.Columns[0].HeaderText = "Job No.";
                dgvOverAll.Columns[1].HeaderText = "Job Furnace No.";
                dgvOverAll.Columns[2].HeaderText = "Job Start";
                dgvOverAll.Columns[3].HeaderText = "Job End";
                dgvOverAll.Columns[4].HeaderText = "Job Duration";

                dgvOverAll.Columns[0].Width = 100;
                dgvOverAll.Columns[1].Width = 100;
                dgvOverAll.Columns[2].Width = 150;
                dgvOverAll.Columns[3].Width = 150;
                dgvOverAll.Columns[4].Width = 100;

                dgvOverAll.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
                dgvOverAll.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowConsumption_Click(object sender, EventArgs e)
        {
            try
            {
                DAL.JobEntry objDAL = new DAL.JobEntry();
                DataTable dtData = new DataTable();
                if (string.IsNullOrEmpty(dtpFromConsumption.Text) && string.IsNullOrEmpty(dtpToConsumption.Text))
                {
                    dtData = objDAL.GetEnergyConsumptionHeatWise(string.Empty, string.Empty);
                }
                else
                {
                    dtData = objDAL.GetEnergyConsumptionHeatWise(dtpFromConsumption.Value.ToString("dd/MMM/yyyy"), dtpToConsumption.Value.ToString("dd/MMM/yyyy"));
                }
                dgvEngCons.DataSource = dtData;
                dgvEngCons.Columns[0].HeaderText = "Job No.";
                dgvEngCons.Columns[1].HeaderText = "Job Furnace No.";
                dgvEngCons.Columns[2].HeaderText = "Total Unit";
                dgvEngCons.Columns[3].HeaderText = "Job Start";
                dgvEngCons.Columns[4].HeaderText = "Job End";
                dgvEngCons.Columns[5].HeaderText = "Heat Start";
                dgvEngCons.Columns[6].HeaderText = "Heat End";


                dgvEngCons.Columns[0].Width = 100;
                dgvEngCons.Columns[1].Width = 100;
                dgvEngCons.Columns[2].Width = 100;
                dgvEngCons.Columns[3].Width = 150;
                dgvEngCons.Columns[4].Width = 150;
                dgvEngCons.Columns[5].Width = 150;
                dgvEngCons.Columns[6].Width = 150;

                dgvEngCons.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
                dgvEngCons.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
                dgvEngCons.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
                dgvEngCons.Columns[6].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkIsLive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsLive.Checked)
            {
                lblSearchJobNo.Visible = false;
                txtSearchJobNo.Visible = false;
            }
            else
            {
                txtSearchJobNo.Text = string.Empty;
                lblSearchJobNo.Visible = true;
                txtSearchJobNo.Visible = true;
            }
        }

        private void btnShowCalculation_Click(object sender, EventArgs e)
        {
            try
            {
                DAL.JobEntry objDAL = new DAL.JobEntry();
                DataTable dtData = new DataTable();
                if (string.IsNullOrEmpty(dtpFromCalculation.Text) && string.IsNullOrEmpty(dtpToCalculation.Text))
                {
                    dtData = objDAL.GetDryRunReport(string.Empty, string.Empty);
                }
                else
                {
                    dtData = objDAL.GetDryRunReport(dtpFromCalculation.Value.ToString("dd/MMM/yyyy"), dtpToCalculation.Value.ToString("dd/MMM/yyyy"));
                }
                dgvDryRun.DataSource = dtData;
                dgvDryRun.Columns[0].Visible = false;
                dgvDryRun.Columns[1].HeaderText = "Heat Start Time";
                dgvDryRun.Columns[2].HeaderText = "Heat End Time";
                dgvDryRun.Columns[3].HeaderText = "Tot. Fur. Dur.";
                dgvDryRun.Columns[4].HeaderText = "Tot. All Job Dur.";
                dgvDryRun.Columns[5].HeaderText = "Tot. Dry Run Dur.";
                dgvDryRun.Columns[6].HeaderText = "Tot. Fur. Unit";
                dgvDryRun.Columns[7].HeaderText = "Tot. Job Unit";
                dgvDryRun.Columns[8].HeaderText = "Tot. Dry Run Unit";
                dgvDryRun.Columns[9].HeaderText = "Tot. Dry Run Amt.";

                dgvDryRun.Columns[0].Width = 0;
                dgvDryRun.Columns[1].Width = 150;
                dgvDryRun.Columns[2].Width = 150;
                dgvDryRun.Columns[3].Width = 65;
                dgvDryRun.Columns[4].Width = 65;
                dgvDryRun.Columns[5].Width = 65;
                dgvDryRun.Columns[6].Width = 65;
                dgvDryRun.Columns[7].Width = 65;
                dgvDryRun.Columns[8].Width = 65;
                dgvDryRun.Columns[9].Width = 65;

                dgvDryRun.Columns[1].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
                dgvDryRun.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy hh:mm:ss tt";
                dgvDryRun.Columns[9].DefaultCellStyle.Format = "#0.000";

                dgvDryRun.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDryRun.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDryRun.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDryRun.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            frmComPort frm = new frmComPort();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ENTITY.GI response = new ENTITY.GI();
                response.response = "Data Not Found.";
                WebRequest tRequest = WebRequest.Create("http://kaeser.appsmith.co.in/api/GujaratIntrux/GetLastDeviceID");
                tRequest.Method = "GET";
                tRequest.ContentType = "application/json";

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();
                            response = Newtonsoft.Json.JsonConvert.DeserializeObject<ENTITY.GI>(responseFromFirebaseServer);
                            System.IO.StreamWriter file = new System.IO.StreamWriter(Application.StartupPath + "\\JSONString.txt");
                            file.WriteLine(responseFromFirebaseServer);
                            file.Close();
                        }
                    }
                }

                if (new DAL.SpectroMaster().InsertUpdateDeleteDeviceID(response))
                {
                    MessageBox.Show("Sync successfully.");
                }
                else
                {
                    MessageBox.Show("Sync not success.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Branch Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
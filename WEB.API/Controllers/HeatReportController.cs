using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENT = Websmith.Entity;
using DAL = Websmith.DataLayer;
using WEB.API.Models;

namespace WEB.API.Controllers
{
    public class HeatReportController : ApiController
    {
        /// <summary>
        /// get heat report which is displayed in mobile app
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("GetHeatReport")]
        public HttpResponseMessage GetHeatReport(ENT.HeatReportParam obj)
        {
            string Message = string.Empty;
            ENT.ResponseHeatReport objResponse = new ENT.ResponseHeatReport();
            List<ENT.HeatFurnaceReport> lstResult = new List<ENT.HeatFurnaceReport>();
            try
            {
                if (string.IsNullOrWhiteSpace(obj.CurrentDate))
                {
                    Message = "Please Enter Valid Date.";
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message, objResponse });
                }

                lstResult = new DAL.HeatReport().GetHeatReportByDate(DAL.DBHelper.ChangeDate(obj.CurrentDate));
                if (lstResult.Count > 0)
                {
                    Message = "Record Get Successfully.";
                    objResponse.PowerOn = Convert.ToString(lstResult[0].fur_entry_time);
                    objResponse.SuperHeat = Convert.ToString(lstResult[0].fur_open_time);
                    objResponse.HeatTapped = Convert.ToString(lstResult[0].fur_close_time);
                    objResponse.TapToTapHrsMin = Convert.ToString(lstResult[0].HrsMin);

                    objResponse.KwhrAtStart = Convert.ToString(lstResult[0].DataValue);
                    objResponse.KwhrAtEnd = Convert.ToString(lstResult[0].DataValue2);
                    objResponse.TotalKwhr = Convert.ToString(lstResult[0].UnitDifference);
                    objResponse.KwhrHeat = Convert.ToString(lstResult[0].UnitDifference * 10);
                }
                else
                {
                    Message = "Record Not Found.";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { Message, objResponse });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { Message, objResponse });
        }

        /// <summary>
        /// This api is used for insert data to live database table of MeterSlaveMaster
        /// </summary>
        /// <param name="obj">object of MeterSlaveMasterParam</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("InsertMeterSlaveMasterAPI")]
        public HttpResponseMessage InsertMeterSlaveMasterAPI(ENT.MeterSlaveMasterParam obj)
        {
            try
            {
                if (obj != null)
                {
                    if (new DAL.MeterSlaveMaster().InsertUpdateDeleteMeterSlaveMasterAPI(obj))
                    {
                        return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = "Data save successfully." });
                    }
                    else
                    {
                        return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { response = "Internal Server Error." });
                    }
                }
                else
                {
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = "Data should not be empty or null." });
                }
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SaveDeviceID()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { response = ex.Message });
            }
        }

        /// <summary>
        /// This api is used for insert data to live database table of FurnaceSwitchParam
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("InsertFurnaceSwitchAPI")]
        public HttpResponseMessage InsertFurnaceSwitchAPI(ENT.FurnaceSwitchParam obj)
        {
            bool IsSuccess = false, IsSendToFcm = false; string ResMessage = string.Empty;
            try
            {
                if (obj != null)
                {
                    // insert data into live database
                    if (new DAL.FurnaceSwitch().InsertFurnaceSwitchAPI(obj))
                    {
                        ResMessage = "Data save successfully.";
                       
                        #region Send Heat Data To FCM
                        if (!string.IsNullOrWhiteSpace(obj.fur_json))
                        {
                            FCMRootObject FCMData = new FCMRootObject();
                            Notification NotificationBody = new Notification();
                            Data NotificationData = new Data();

                            // Send notification if device token found.
                            int count = 0;
                            List<ENT.Device> lstENT = getDeviceID();
                            for (int i = 0; i < lstENT.Count; i++)
                            {
                                if (!string.IsNullOrWhiteSpace(lstENT[i].DeviceId))
                                {
                                    FCMData.to = lstENT[i].DeviceId;
                                    NotificationData.Description = obj.fur_json;
                                    NotificationBody.title = "GI ADMIN";
                                    NotificationBody.body = obj.fur_json;
                                    FCMData.data = NotificationData;
                                    // FCMData.notification = NotificationBody;

                                    // send notification using FCM sender
                                    FCMResponse s = new FCMSender().SendClientNotification(FCMData);
                                    if (s.success > 0)
                                    {
                                        // successfull send notification count
                                        count++;
                                    }
                                }
                            }
                            if (count == lstENT.Count)
                            {
                                IsSendToFcm = true;
                            }
                            else
                            {
                                IsSendToFcm = false;
                            }
                            ResMessage += " And [" + count.ToString() + "] Notification Send Successfull Out of [" + lstENT.Count + "].";
                        }
                        #endregion

                        return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = ResMessage });
                    }
                    else
                    {
                        return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { response = "Internal Server Error." });
                    }
                }
                else
                {
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = "Data should not be empty or null." });
                }
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SaveDeviceID()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
        }

        /// <summary>
        /// This api is used for insert data to live database table of FurnaceSwitchParam
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("InsertHeatStartStopReportAPI")]
        public HttpResponseMessage InsertHeatStartStopReportAPI(ENT.HeatStartStopReport obj)
        {
            bool IsSuccess = false, IsSendToFcm = false; string ResMessage = string.Empty;
            try
            {
                if (obj != null)
                {
                    // insert data into live database
                    //if (new DAL.FurnaceSwitch().InsertFurnaceSwitchAPI(obj))
                    //{
                    //    ResMessage = "Data save successfully.";

                    #region Send Heat Data To FCM
                    if (!string.IsNullOrWhiteSpace(obj.heat_json))
                    {
                        FCMRootObject FCMData = new FCMRootObject();
                        Notification NotificationBody = new Notification();
                        Data NotificationData = new Data();

                        // Send notification if device token found.
                        int count = 0;
                        List<ENT.Device> lstENT = getDeviceID();
                        for (int i = 0; i < lstENT.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(lstENT[i].DeviceId))
                            {
                                FCMData.to = lstENT[i].DeviceId;
                                NotificationData.Description = obj.heat_json;
                                NotificationBody.title = "GI ADMIN";
                                NotificationBody.body = obj.heat_json;
                                FCMData.data = NotificationData;
                                // FCMData.notification = NotificationBody;

                                // send notification using FCM sender
                                FCMResponse s = new FCMSender().SendClientNotification(FCMData);
                                if (s.success > 0)
                                {
                                    // successfull send notification count
                                    count++;
                                }
                            }
                        }
                        if (count == lstENT.Count)
                        {
                            IsSendToFcm = true;
                        }
                        else
                        {
                            IsSendToFcm = false;
                        }
                        ResMessage += "[" + count.ToString() + "] Notification Send Successfull Out of [" + lstENT.Count + "].";
                    }
                    #endregion

                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = ResMessage });
                   
                    //}
                    //else
                    //{
                    //    return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { response = "Internal Server Error." });
                    //}
                }
                else
                {
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = "Data should not be empty or null." });
                }
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SaveDeviceID()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
        }

        /// <summary>
        /// This function is return all device token list from database
        /// </summary>
        /// <returns>String(DeviceID)</returns>
        public List<ENT.Device> getDeviceID()
        {
            List<ENT.Device> lstENT = new List<ENT.Device>();
            try
            {
                lstENT = new DAL.Device().GetAllDeviceID();
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : getDeviceID()");
            }
            return lstENT;
        }
    }
}

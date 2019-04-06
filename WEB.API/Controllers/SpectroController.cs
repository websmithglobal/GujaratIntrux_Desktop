using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENT = Websmith.Entity;
using DAL = Websmith.DataLayer;
using WEB.API.Models;
using System.IO;
using System.Text;

namespace WEB.API.Controllers
{
    public class SpectroController : ApiController
    {
        /// <summary>
        /// This API used for saving element value in detail table in database
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaveSpectroEntry")]
        public HttpResponseMessage SaveSpectroEntry(List<ENT.SpectroEntry> Model)
        {
            try
            {
                int cnt = 0;
                Int64 SpectroNo = new DAL.SpectroEntry().GetTopOneSpectroNo();
                foreach (ENT.SpectroEntry el in Model)
                {
                    el.Mode = "ADD";
                    el.SpectroID = Guid.NewGuid();
                    el.SpectroNo = SpectroNo;
                    if (new DAL.SpectroEntry().InsertUpdateDeleteSpectroEntry(el))
                    {
                        Model[cnt].Message = "Entry Save Successfully.";
                    }
                    else
                    {
                        Model[cnt].Message = "Internal Server Error.";
                    }
                    cnt++;
                }
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SaveSpectroEntry()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, Model);
            //return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { Model });
        }

        /// <summary>
        /// This API used for send FCM notification with saving master data into database
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaveSpectroMaster")]
        public HttpResponseMessage SaveSpectroMaster(ENT.SpectroMaster Model)
        {
            bool IsSuccess = false, IsSendToFcm = false;
            string ResMessage = string.Empty;
            try
            {
                #region Send Spectro Data To FCM

                FCMRootObject FCMData = new FCMRootObject();
                Notification NotificationBody = new Notification();
                Data NotificationData = new Data();

                int count = 0;
                List<ENT.Device> lstENT = getDeviceID();
                for (int i = 0; i < lstENT.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lstENT[i].DeviceId))
                    {
                        FCMData.to = lstENT[i].DeviceId;
                        NotificationData.Description = Model.SpectroJson;
                        NotificationBody.title = "GI ADMIN";
                        NotificationBody.body = Model.SpectroJson;
                        FCMData.data = NotificationData;
                        // FCMData.notification = NotificationBody;

                        FCMResponse s = new FCMSender().SendClientNotification(FCMData);
                        if (s.success > 0)
                        {
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
                ResMessage = count.ToString() + " notification send successfull out of " + lstENT.Count +".";

                #endregion

                #region Save Master To Local Database

                Int64 SpectroNo = new DAL.SpectroEntry().GetTopOneSpectroNo();
                Model.Mode = "ADD";
                Model.SpectroID = Guid.NewGuid();
                Model.SpectroNo = SpectroNo + 1;

                DateTime dt1 = DateTime.Now;
                string[] dtDate = Model.SpectroDate.Split('/');
                if (dtDate.Length == 3)
                {
                    dt1 = new DateTime(Convert.ToInt32(dtDate[2]), Convert.ToInt32(dtDate[0]), Convert.ToInt32(dtDate[1]));
                }
                Model.SpectroDate = dt1.ToString("dd/MMM/yyyy");
                if (new DAL.SpectroEntry().InsertUpdateDeleteSpectroMaster(Model))
                {
                    ResMessage += " Entry Save Successfully.";
                    IsSuccess = true;
                }
                else
                {
                    ResMessage += " Entry Not Save Successfully.";
                    IsSuccess = false;
                }

                #endregion
            }
            catch (Exception ex)
            {
                ResMessage += ResMessage + " Error: " + ex.Message;
                IsSuccess = false;
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SaveSpectroMaster()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ResMessage, IsSuccess, IsSendToFcm });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { ResMessage, IsSuccess, IsSendToFcm });
        }

        /// <summary>
        /// This API used for send FCM notification without saving data into database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SendSpectroDataToFCM")]
        public HttpResponseMessage SendSpectroDataToFCM(FcmRequest model)
        {
            bool IsSuccess = false;
            try
            {
                FCMRootObject FCMData = new FCMRootObject();
                Notification NotificationBody = new Notification();
                Data NotificationData = new Data();

                string dt = ""; // getDeviceID();
                if (!string.IsNullOrWhiteSpace(dt))
                {
                    FCMData.to = dt; 
                    NotificationData.Description = model.JsonModel;
                    NotificationBody.title = "GI ADMIN";
                    NotificationBody.body = model.JsonModel;
                    FCMData.data = NotificationData;

                    FCMResponse FCMResp = new FCMSender().SendClientNotification(FCMData);
                    IsSuccess = true;
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { FCMResp, IsSuccess });
                }
                else
                {
                    IsSuccess = false;
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { Response = "Token Invalid.", IsSuccess });
                }
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SendSpectroDataToFCM()");
                IsSuccess = false;
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message, IsSuccess });
            }
        }

        /// <summary>
        /// This API is used to save Device ID for send FCM notification.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaveDeviceID")]
        public HttpResponseMessage SaveDeviceID(ENT.Device obj)
        {
            DAL.Device objDAL = new DAL.Device();
            try
            {
                if (!string.IsNullOrWhiteSpace(obj.DeviceId) && !string.IsNullOrWhiteSpace(obj.DeviceCode))
                {
                    if (objDAL.InsertUpdateDeleteDeviceID(obj))
                    {
                        return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = "Data save successfully." });
                    }
                    else
                    {
                        throw new Exception("Internal Server Error.");
                    }
                }
                else
                {
                    return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response = "Device ID and Device Token should not be empty or null or white space." });
                }
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : SaveDeviceID()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
        }

        /// <summary>
        /// This function returns last inserted device id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ActionName("GetLastDeviceID")]
        public HttpResponseMessage GetLastDeviceID()
        {
            DAL.Device objDAL = new DAL.Device();
            string response = "";
            try
            {
                List<ENT.Device> lstENT = objDAL.GetLastDeviceID();
                if (lstENT.Count > 0)
                    response = lstENT[0].DeviceId;
                else
                    response = "Record not found.";
            }
            catch (Exception ex)
            {
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : GetLastDeviceID()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { response });
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("GetSpectroEntry")]
        public HttpResponseMessage GetSpectroEntry()
        {
            ENT.SpectroEntry objENT = new ENT.SpectroEntry();
            List<ENT.SpectroEntry> lstResult = new List<ENT.SpectroEntry>();
            try
            {
                objENT.Mode = "GetAllSpectroEntry";
                lstResult = new DAL.SpectroEntry().GetSpectroEntry(objENT);
                objENT.Message = lstResult.Count + " Record Found.";
            }
            catch (Exception ex)
            {
                objENT.Message = ex.Message.ToString();
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : GetSpectroEntry()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { objENT });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult, objENT });
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("GetSpectroMaster")]
        public HttpResponseMessage GetSpectroMaster(ENT.SpectroMasterParam objENT)
        {
            List<ENT.SpectroMaster> lstResult = new List<ENT.SpectroMaster>();
            try
            {
                objENT.Mode = "GetSpectroMaster";
                lstResult = new DAL.SpectroEntry().GetSpectroMasterForAPI(objENT);
                objENT.Message = lstResult.Count + " record found.";
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { objENT, lstResult });
            }
            catch (Exception ex)
            {
                objENT.Message = ex.Message.ToString();
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : GetSpectroMaster()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { objENT, lstResult });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("GetSpectroDetail")]
        public HttpResponseMessage GetSpectroDetail(ENT.SpectroDetailParam objENT)
        {
            List<ENT.SpectroEntry> lstResult = new List<ENT.SpectroEntry>();
            try
            {
                objENT.Mode = "GetSpectroDetail";
                lstResult = new DAL.SpectroEntry().GetSpectroDetailForAPI(objENT);
                objENT.Message = lstResult.Count + " record found.";
                return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { objENT, lstResult });
            }
            catch (Exception ex)
            {
                objENT.Message = ex.Message.ToString();
                ERRORREPORTING.Report(ex, Request.RequestUri.AbsoluteUri, new Guid("00000000-0000-0000-0000-000000000000"), "Gujarat_Intrux", "Function Name : GetSpectroDetail()");
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { objENT, lstResult });
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

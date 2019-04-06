using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENT = Websmith.Entity;
using DAL = Websmith.DataLayer;

namespace WEB.API.Controllers
{
    public class JobController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaveJobEntry")]
        public HttpResponseMessage SaveJobEntry(ENT.JobEntry Model)
        {
            try
            {
                if (new DAL.JobEntry().InsertUpdateDeleteJobEntry(Model))
                {
                    Model.Message = "Job Save Successfully.";
                }
                else
                {
                    Model.Message = "Internal Server Error";
                }
            }
            catch (Exception ex)
            {
                Model.Message = ex.Message.ToString();
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { Model });
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("GetAllJobData")]
        public HttpResponseMessage GetAllJobData()
        {
            ENT.JobEntry objENT = new ENT.JobEntry();
            List<ENT.JobEntry> lstResult = new List<ENT.JobEntry>();
            try
            {
                objENT.Mode = "GetAllJobEntry";
                lstResult = new DAL.JobEntry().GetJobEntry(objENT);
                objENT.Message = lstResult.Count + " Record Found.";
            }
            catch (Exception ex)
            {
                objENT.Message = ex.Message.ToString();
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { objENT });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult, objENT });
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("SaveHeatStartStop")]
        public HttpResponseMessage SaveHeatStartStop(ENT.HeatStartStop Model)
        {
            try
            {
                if (new DAL.HeatStartStop().InsertUpdateDeleteHeatStartStop(Model))
                {
                    Model.Message = "Heat Started Successfully.";
                }
                else
                {
                    Model.Message = "Internal Server Error";
                }
            }
            catch (Exception ex)
            {
                Model.Message = ex.Message.ToString();
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { ex.Message });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { Model });
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("GetHeatStartStop")]
        public HttpResponseMessage GetHeatStartStop()
        {
            ENT.HeatStartStop objENT = new ENT.HeatStartStop();
            List<ENT.HeatStartStop> lstResult = new List<ENT.HeatStartStop>();
            try
            {
                objENT.Mode = "GetAll";
                lstResult = new DAL.HeatStartStop().GetHeatStartStop(objENT);
                objENT.Message = lstResult.Count + " Record Found.";
            }
            catch (Exception ex)
            {
                objENT.Message = ex.Message.ToString();
                return ControllerContext.Request.CreateResponse(HttpStatusCode.InternalServerError, new { objENT });
            }
            return ControllerContext.Request.CreateResponse(HttpStatusCode.OK, new { lstResult, objENT });
        }
    }
}


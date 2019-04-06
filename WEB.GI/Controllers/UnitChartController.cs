using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENT = Websmith.Entity;
using DA = Websmith.DataLayer;

namespace WEB.GI.Controllers
{
    public class UnitChartController : Controller
    {
        List<ENT.UnitChartClass> lstChart = new List<ENT.UnitChartClass>();
        DA.UnitChartClass objDAL = new DA.UnitChartClass();

        // GET: UnitChart
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getChartData(ENT.UnitChartParam prm)
        {
            ENT.UnitChartParam ent = new ENT.UnitChartParam();
            try
            {
                ent.mode = "UnitChart";
                ent.datefrom = DA.DBHelper.ChangeDate(prm.datefrom) + " 12:00 AM";
                ent.dateto = DA.DBHelper.ChangeDate(prm.dateto) + " 11:59 PM";
                lstChart = objDAL.GetUnitChartData(ent);
            }
            catch (Exception ex)
            {
                lstChart = new List<ENT.UnitChartClass>();
                ent.mode = ex.Message.ToString();
                return Json(new { lstChart, ent }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { lstChart, ent }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class ResponseHeatReport
    {
        public string PowerOn { get; set; }
        public string SuperHeat { get; set; }
        public string HeatTapped { get; set; }
        public string TapToTapHrsMin { get; set; }
        public string KwhrAtStart { get; set; }
        public string KwhrAtEnd { get; set; }
        public string TotalKwhr { get; set; }
        public string KwhrHeat { get; set; }
    }

    public class HeatFurnaceReport
    {
        public string fur_name { get; set; }
        public int fur_no { get; set; }
        public int fur_status { get; set; }
        public DateTime fur_open_time { get; set; }
        public DateTime fur_close_time { get; set; }
        public DateTime DataTime { get; set; }
        public decimal DataValue { get; set; }
        public decimal DataValue2 { get; set; }
        public DateTime fur_entry_time { get; set; }
        public decimal UnitDifference { get; set; }
        public string HrsMin { get; set; }
    }

    public class HeatReportParam
    {
        public string CurrentDate { get; set; }
    }
}

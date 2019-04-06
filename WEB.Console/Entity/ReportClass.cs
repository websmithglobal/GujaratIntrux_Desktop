using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB.ConsoleApp.Entity
{
    class ReportClass
    {
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

    public class ResponseHeatStartStopReport
    {
        public string PowerOn { get; set; } = string.Empty;
        public string SuperHeat { get; set; } = string.Empty;
        public string HeatTapped { get; set; } = string.Empty;
        public string TapToTapHrsMin { get; set; } = string.Empty;
        public string KwhrAtStart { get; set; } = string.Empty;
        public string KwhrAtEnd { get; set; } = string.Empty;
        public string TotalKwhr { get; set; } = string.Empty;
        public string KwhrHeat { get; set; } = string.Empty;
    }
}

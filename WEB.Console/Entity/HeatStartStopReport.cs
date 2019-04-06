using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB.ConsoleApp.Entity
{
    public class HeatStartStopReport
    {
        public Int64 ID { get; set; }
        public int LineCountStart { get; set; }
        public int LineCountEnd { get; set; }
        public DateTime StartDataTime { get; set; }
        public DateTime EndDataTime { get; set; }
        public decimal DataValue { get; set; }
        public decimal DataValue2 { get; set; }
        public string FileName { get; set; }
        public DateTime EntryDate { get; set; }
        public string fur_name { get; set; }
        public int fur_no { get; set; }
        public int fur_status_stop { get; set; }
        public int fur_status_start { get; set; }
        public DateTime fur_open_time { get; set; }
        public DateTime fur_close_time { get; set; }
        public int isupdated { get; set; }
        public string heat_json { get; set; }
    }

    public class HeatStartStopReportApi
    {
        public Int64 ID { get; set; }
        public DateTime StartDataTime { get; set; }
        public DateTime EndDataTime { get; set; }
        public decimal DataValue { get; set; }
        public decimal DataValue2 { get; set; }
        public string fur_second { get; set; }
        public decimal UnitDifference { get; set; }
        public string HrsMin { get; set; }
    }

    public class ResponseHeatReport
    {
        public string ReportName { get; set; } = string.Empty;
        public string PowerOn { get; set; } = string.Empty;
        public string SuperHeat { get; set; } = string.Empty;
        public string HeatTapped { get; set; } = string.Empty;
        public string TapToTapHrsMin { get; set; } = string.Empty;
        public string KwhrAtStart { get; set; } = string.Empty;
        public string KwhrAtEnd { get; set; } = string.Empty;
        public string TotalKwhr { get; set; } = string.Empty;
        public string KwhrHeat { get; set; } = string.Empty;
    }

    public class HeatReport
    {
        public string ReportType { get; set; }
        public ResponseHeatReport HeatUnitReport { get; set; }
        public ResponseHeatReport HeatDryrunReport { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GI.ENTITY
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
        public int issend { get; set; }
        public string heat_json { get; set; }
        public string Mode { get; set; }
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
}

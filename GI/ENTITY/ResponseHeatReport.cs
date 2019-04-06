using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GI.ENTITY
{
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
}

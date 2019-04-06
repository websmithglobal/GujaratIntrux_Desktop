using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI.ENTITY
{
    public class DryrunHeatReport
    {
        public Int64 ID { get; set; }
        public Int64 LineCountStart { get; set; }
        public Int64 LineCountEnd { get; set; }
        public DateTime StartDataTime { get; set; }
        public DateTime EndDataTime { get; set; }
        public decimal DataValueLast { get; set; }
        public decimal DataValueFirst { get; set; }
        public string FileName { get; set; }
        public DateTime EntryDate { get; set; }
        public int IsUpdated { get; set; }
        public string Mode { get; set; }
    }
}

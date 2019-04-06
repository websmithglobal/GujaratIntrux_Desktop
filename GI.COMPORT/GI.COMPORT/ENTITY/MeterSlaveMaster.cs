using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GI.COMPORT.ENTITY
{
    public class MeterSlaveMaster
    {
        public Guid ID { get; set; }
        public DateTime DataDate { get; set; }
        public DateTime DataTime { get; set; }
        public Int64 MeterID { get; set; }
        public Int64 SlaveID { get; set; }
        public Int64 Address { get; set; }
        public Int64 Quantity { get; set; }
        public decimal DataValue { get; set; }
        public decimal DataValue2 { get; set; }
        public decimal Difference { get; set; }
        public decimal Value1 { get; set; }
        public decimal Value2 { get; set; }
        public int LineCount { get; set; }
        public decimal FinalUnit { get; set; }
        public string  FileName { get; set; }
        public DateTime EntryDate { get; set; }
        public string Mode { get; set; }
    }
}

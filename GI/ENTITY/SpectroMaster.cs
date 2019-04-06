using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GI.ENTITY
{
    class SpectroMaster
    {
        public Guid SpectroID { get; set; }
        public Int64 SpectroNo { get; set; }
        public string SpectroDate { get; set; }
        public string Quality { get; set; }
        public string Grade { get; set; }
        public string SampleNo { get; set; }
        public DateTime EntryDate { get; set; }
        public string Mode { get; set; }
        public string Message { get; set; }
    }
}

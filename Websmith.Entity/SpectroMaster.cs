using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class SpectroMaster
    {
        public Guid SpectroID { get; set; } = new Guid("00000000-0000-0000-0000-000000000000");
        public Int64 SpectroNo { get; set; } = 0;
        public string SpectroDate { get; set; } = string.Empty;
        public string Quality { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string SampleNo { get; set; } = string.Empty;
        public DateTime EntryDate { get; set; } = DateTime.Now;
        public string Mode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string SpectroJson { get; set; } = string.Empty;
    }

    public class SpectroMasterParam
    {
        public string FromDate { get; set; } = string.Empty;
        public string ToDate { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

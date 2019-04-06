using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class UnitChartClass
    {
        public DateTime x { get; set; }
        public decimal y { get; set; }
        public string indexLabel { get; set; }
        public string markerType { get; set; }
        public string markerColor { get; set; }
    }

    public class UnitChartParam
    {
        public string datefrom { get; set; }
        public string dateto { get; set; }
        public string mode { get; set; }
    }
}

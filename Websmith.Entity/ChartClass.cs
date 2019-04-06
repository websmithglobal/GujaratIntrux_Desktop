using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class ChartDataset
    {
        public List<decimal> data { get; set; }
        public List<string> backgroundColor { get; set; }
    }

    public class ChartJsonObject
    {
        public List<string> labels { get; set; }
        public List<ChartDataset> datasets { get; set; }
    }
}

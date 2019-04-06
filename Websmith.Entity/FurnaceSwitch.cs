using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Websmith.Entity
{
    public class FurnaceSwitchParam
    {
        public Guid fur_id { get; set; }
        public string fur_name { get; set; }
        public int fur_no { get; set; }
        public int fur_status { get; set; }
        public DateTime fur_open_time { get; set; }
        public DateTime fur_close_time { get; set; }
        public DateTime fur_entry_time { get; set; }
        public DateTime fur_file_time { get; set; }
        public string fur_json { get; set; }
    }

    public class FurnaceSwitch
    {
        public Guid fur_id { get; set; }
        public string fur_name { get; set; }
        public int fur_no { get; set; }
        public int fur_status { get; set; }
        public DateTime fur_open_time { get; set; }
        public DateTime fur_close_time { get; set; }
        public DateTime fur_entry_time { get; set; }
        public DateTime fur_file_time { get; set; }
    }
}

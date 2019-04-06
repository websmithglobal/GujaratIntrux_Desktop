using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GI.COMPORT.ENTITY
{
    public class FurnaceSwitch
    {
        public Guid fur_id { get; set; }
        public string fur_name { get; set; }
        public int fur_no { get; set; }
        public int fur_status { get; set; }
        public string fur_open_time { get; set; }
        public string fur_close_time { get; set; }
        public string fur_entry_time { get; set; }
        public string fur_file_time { get; set; }
        public string Mode { get; set; }
    }                   
}

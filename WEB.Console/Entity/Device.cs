using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB.ConsoleApp.Entity
{
    public class Device
    {
        public string DeviceId { get; set; }
        public string DeviceCode { get; set; }
    }

    public class TokenJsonResponse
    {
        public List<Device> DeviceTokenList { get; set; }
    }
}

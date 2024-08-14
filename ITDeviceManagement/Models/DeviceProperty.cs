using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITDeviceManagement.Models
{
    public class DeviceProperty
    {
        public int DeviceID { get; set; }
        public int PropertyID { get; set; }
        public string PropertyValue { get; set; }

        // Navigation properties
        public Device Device { get; set; }
        public Property Property { get; set; }
    }

}
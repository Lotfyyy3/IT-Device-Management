using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITDeviceManagement.Models
{
    public class DeviceCategoryProperty
    {
        public int DeviceCategoryID { get; set; }
        public int PropertyID { get; set; }

        // Navigation properties
        public DeviceCategory DeviceCategory { get; set; }
        public Property Property { get; set; }
    }

}
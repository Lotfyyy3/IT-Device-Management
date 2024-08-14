using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace ITDeviceManagement.Models
{
    public class DeviceCategory
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        // Navigation property
        public ICollection<DeviceCategoryProperty> DeviceCategoryProperties { get; set; }
        public ICollection<Device> Devices { get; set; }
    }

}
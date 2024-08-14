using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITDeviceManagement.Models
{
    public class Property
    {
        public int ID { get; set; }
        public string PropertyDescription { get; set; }

        // Navigation property
        public ICollection<DeviceCategoryProperty> DeviceCategoryProperties { get; set; }
        public ICollection<DeviceProperty> DeviceProperties { get; set; }
    }

}
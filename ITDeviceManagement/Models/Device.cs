using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITDeviceManagement.Models
{
    public class Device
    {
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public int DeviceCategoryID { get; set; }
        public DateTime? AcquisitionDate { get; set; }
        public string Memo { get; set; }

        // Navigation properties
        public DeviceCategory DeviceCategory { get; set; }
        public ICollection<DeviceProperty> DeviceProperties { get; set; }
    }

}
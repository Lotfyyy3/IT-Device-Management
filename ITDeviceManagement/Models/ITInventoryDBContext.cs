using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace ITDeviceManagement.Models
{
    public class ITInventoryDBContext : DbContext
    {
        public DbSet<DeviceCategory> DeviceCategories { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<DeviceCategoryProperty> DeviceCategoryProperties { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceProperty> DeviceProperties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceCategoryProperty>()
                .HasKey(dcp => new { dcp.DeviceCategoryID, dcp.PropertyID });

            modelBuilder.Entity<DeviceProperty>()
                .HasKey(dp => new { dp.DeviceID, dp.PropertyID });

            // Define relationships
            modelBuilder.Entity<Device>()
                .HasMany(d => d.DeviceProperties)
                .WithRequired(dp => dp.Device)
                .HasForeignKey(dp => dp.DeviceID);

            modelBuilder.Entity<Property>()
                .HasMany(p => p.DeviceProperties)
                .WithRequired(dp => dp.Property)
                .HasForeignKey(dp => dp.PropertyID);

            base.OnModelCreating(modelBuilder);
        }

    }

}
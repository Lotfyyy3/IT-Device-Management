using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ITDeviceManagement.Models;

namespace ITDeviceManagement.Controllers
{
    public class DevicesController : Controller
    {
        private ITInventoryDBContext db = new ITInventoryDBContext();

        /// GET: Devices
        public async Task<ActionResult> Index()
        {
            var devices = db.Devices
                            .Include(d => d.DeviceCategory)
                            .Include(d => d.DeviceProperties.Select(dp => dp.Property)); // Include related properties
            return View(await devices.ToListAsync());
        }


        // GET: Devices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = await db.Devices.FindAsync(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        public ActionResult Create()
        {
            // Populate the dropdown list with device categories
            ViewBag.DeviceCategoryID = new SelectList(db.DeviceCategories, "ID", "CategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeviceName,DeviceCategoryID,AcquisitionDate,Memo")] Device device, List<DeviceProperty> deviceProperties)
        {
            if (ModelState.IsValid)
            {
                // Add the new device to the database
                db.Devices.Add(device);
                db.SaveChanges();

                // Save the properties associated with this device
                foreach (var prop in deviceProperties)
                {
                    prop.DeviceID = device.ID;
                    db.DeviceProperties.Add(prop);
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // If we got this far, something failed; re-populate the dropdown list
            ViewBag.DeviceCategoryID = new SelectList(db.DeviceCategories, "ID", "CategoryName", device.DeviceCategoryID);
            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int id)
        {
            // Include the related DeviceProperties and their associated Property details
            var device = db.Devices
                           .Include(d => d.DeviceProperties.Select(dp => dp.Property))
                           .FirstOrDefault(d => d.ID == id);

            if (device == null)
            {
                return HttpNotFound();
            }

            ViewBag.DeviceCategoryID = new SelectList(db.DeviceCategories, "ID", "CategoryName", device.DeviceCategoryID);
            return View(device);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Device model, List<DeviceProperty> deviceProperties)
        {
            if (ModelState.IsValid)
            {
                // Load the existing device from the database
                var device = db.Devices.Include(d => d.DeviceProperties)
                                       .SingleOrDefault(d => d.ID == model.ID);

                if (device == null)
                {
                    return HttpNotFound();
                }

                // Update device details
                device.DeviceName = model.DeviceName;
                device.DeviceCategoryID = model.DeviceCategoryID;
                device.AcquisitionDate = model.AcquisitionDate;
                device.Memo = model.Memo;

                // Remove old properties
                db.DeviceProperties.RemoveRange(device.DeviceProperties);

                // Add new properties
                foreach (var prop in deviceProperties)
                {
                    if (!string.IsNullOrEmpty(prop.PropertyValue))
                    {
                        db.DeviceProperties.Add(new DeviceProperty
                        {
                            DeviceID = device.ID,
                            PropertyID = prop.PropertyID,
                            PropertyValue = prop.PropertyValue
                        });
                    }
                }

                // Save changes
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Reload categories for the view
            ViewBag.DeviceCategoryID = new SelectList(db.DeviceCategories, "ID", "CategoryName", model.DeviceCategoryID);
            return View(model);
        }

        // GET: Devices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = await db.Devices.FindAsync(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Device device = await db.Devices.FindAsync(id);
            db.Devices.Remove(device);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public JsonResult GetPropertiesByCategory(int categoryId)
        {
            // Retrieve the properties associated with the selected device category
            var properties = (from p in db.Properties
                              join cp in db.DeviceCategoryProperties on p.ID equals cp.PropertyID
                              where cp.DeviceCategoryID == categoryId
                              select new
                              {
                                  p.ID,
                                  p.PropertyDescription
                              }).ToList();

            return Json(properties, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

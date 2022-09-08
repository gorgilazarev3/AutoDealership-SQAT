using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoDealership.Models;

namespace AutoDealership.Controllers
{
    public class VehicleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vehicle
        public ActionResult Index()
        {
            return View(db.Vehicles.ToList());
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }
        [Authorize]
        // GET: Vehicle/Create
        public ActionResult Create()
        {
            CreateVehicleViewModel model = new CreateVehicleViewModel();
            model.Brands = db.Brands.ToList();
            return View(model);
        }

        // POST: Vehicle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Brand,Model,FuelType,BodyStyle,Transmission,Year,Mileage,DrivetrainType,Color,InteriorColor,FuelEfficiency,Horsepower,Torque,Engine,Description,Price,IsForLease,IsForRent,MonthlyPayment,DailyPayment,VehicleStatus,InStock,CoverImageURL")] Vehicle vehicle)
        {
            vehicle.ImagesURL = new List<string>();
            vehicle.Features = new List<string>();
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var brand = db.Brands.Include(b => b.Vehicles).Where(b => b.Name.Equals(vehicle.Brand.Name)).FirstOrDefault();
                if(brand != null)
                {
                    vehicle.Brand = brand;
                    if(brand.Vehicles == null)
                    {
                        brand.Vehicles = new List<Vehicle>();
                    }
                    brand.Vehicles.Add(vehicle);
                    //db.Entry<Vehicle>(vehicle).State = EntityState.Modified;
                    //db.Entry<Brand>(brand).State = EntityState.Modified;
                    db.Vehicles.Add(vehicle);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            CreateVehicleViewModel model = new CreateVehicleViewModel();
            model.Vehicle = vehicle;
            model.Brands = db.Brands.ToList();
            return View(model);
        }

        public ActionResult CreateBrand()
        {
            CreateVehicleViewModel model = new CreateVehicleViewModel();
            model.Brands = db.Brands.ToList();
            return View("CreateBrand", model);
        }
        [HttpPost]
        public ActionResult CreateBrand(CreateVehicleViewModel model)
        {
            db.Brands.Add(model.NewBrand);
            db.SaveChanges();
            return RedirectToAction("Create");
        }

        public ActionResult GetBrand(int id)
        {

            var brand = db.Brands.Find(id);
            if (brand != null)
                return Json(brand, JsonRequestBehavior.AllowGet);
            return HttpNotFound();
            
        }

        // GET: Vehicle/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Brand,Model,FuelType,BodyStyle,Transmission,Year,Mileage,DrivetrainType,Color,InteriorColor,FuelEfficiency,Horsepower,Torque,Engine,Description,Price,IsForLease,IsForRent,MonthlyPayment,DailyPayment,VehicleStatus,InStock,CoverImageURL")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
            db.SaveChanges();
            return RedirectToAction("Index");
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

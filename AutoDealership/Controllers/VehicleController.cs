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
        [Authorize(Roles = "Editor, Administrator")]
        public ActionResult Index()
        {
            ViewBag.Brands = db.Brands.Include(b => b.Vehicles).ToList();
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
            //Vehicle vehicle = db.Vehicles.Include(v => v.ImagesURL).Where(v => v.Id == id).FirstOrDefault();
            Brand brand = db.Brands.Find(vehicle.BrandId);
            VehicleDetailsViewModel model = new VehicleDetailsViewModel();
            model.Vehicle = vehicle;
            model.Brand = brand;
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [Authorize(Roles = "Editor, Administrator")]
        // GET: Vehicle/Create
        public ActionResult Create()
        {
            CreateVehicleViewModel model = new CreateVehicleViewModel();
            model.Brands = db.Brands.ToList();
            return View(model);
        }

        [Authorize(Roles = "Editor, Administrator")]
        // POST: Vehicle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BrandId,Model,FuelType,BodyStyle,Features,ImagesURL,Transmission,Year,Mileage,DrivetrainType,Color,InteriorColor,FuelEfficiency,Horsepower,Torque,Engine,Description,Price,IsForLease,IsForRent,MonthlyPayment,DailyPayment,VehicleStatus,InStock,CoverImageURL")] Vehicle vehicle)
        {
            //string[] featureArray = viewModel.ListOfFeatures.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                //var brand = db.Brands.Include(b => b.Vehicles).Where(b => b.Name.Equals(vehicle.Brand.Name)).FirstOrDefault();
                var brand = db.Brands.Include(b => b.Vehicles).Where(b => b.Id.Equals(vehicle.BrandId)).FirstOrDefault();
                if(brand != null)
                {
                    //vehicle.Brand = brand;
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
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateBrand()
        {
            CreateVehicleViewModel model = new CreateVehicleViewModel();
            model.Brands = db.Brands.ToList();
            return View("CreateBrand", model);
        }
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public ActionResult Brands() 
        {
            return View(db.Brands.Include(b => b.Vehicles).ToList());
        }

        // GET: Vehicle/EditBrand/5
        [Authorize(Roles = "Administrator")]
        public ActionResult EditBrand(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Include(b => b.Vehicles).Where(b => b.Id == id).FirstOrDefault();
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        //POST: Vehicle/EditBrand/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBrand([Bind(Include = "Id,Name,LogoURL,Vehicles")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Brands");
        }

        // GET: Vehicle/Edit/5
        [Authorize(Roles = "Administrator, Editor")]
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
        public ActionResult Edit([Bind(Include = "Id,BrandId,Model,FuelType,Features,ImagesURL,BodyStyle,Transmission,Year,Mileage,DrivetrainType,Color,InteriorColor,FuelEfficiency,Horsepower,Torque,Engine,Description,Price,IsForLease,IsForRent,MonthlyPayment,DailyPayment,VehicleStatus,InStock,CoverImageURL")] Vehicle vehicle)
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

        // DELETE: Vehicle/DeleteAllFromBrand
        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteAllFromBrand(int id)
        {
            var brand = db.Brands.Include(b => b.Vehicles).Where(b => b.Id == id).FirstOrDefault();
            var vehiclesInBrand = brand.Vehicles;
            foreach(var vehicle in vehiclesInBrand.ToList())
            {
                db.Vehicles.Remove(vehicle);
            }
            brand.Vehicles.Clear();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VehicleReservations()
        {
            ViewBag.Users = db.Users.ToList();
            ViewBag.Vehicles = db.Vehicles.ToList();
            return View(db.VehicleReservations.ToList());
        }
        [Authorize]
        [HttpPost]
        public ActionResult ReserveVehicle(VehicleReservation model)
        {
            var user = db.Users.Where(u => u.UserName.Equals(model.UserId)).FirstOrDefault();
            model.UserId = user.Id;
            var vehicle = db.Vehicles.Find(model.VehicleId);
            if (model.IsTestDrive) 
            { 
                vehicle.IsTestDriven = true;
            }
            else
            {
                user.ReservedVehicleId = vehicle.Id;
                vehicle.InStock = false;
            }
            db.VehicleReservations.Add(model);
            db.SaveChanges();
            return Redirect("/Vehicle/Details/" + model.VehicleId);
        }
        [Authorize(Roles = "Customer, Administrator")]
        [HttpDelete]
        public ActionResult CancelReservation(int id)
        {
            var reservation = db.VehicleReservations.Find(id);
            var vehicle = db.Vehicles.Find(reservation.VehicleId);
            var user = db.Users.Find(reservation.UserId);
            vehicle.InStock = true;
            if (vehicle.IsTestDriven)
                vehicle.IsTestDriven = false;
            user.ReservedVehicleId = null;
            db.VehicleReservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("VehicleReservations");
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyReservations() 
        {
            var user = db.Users.Where(u => User.Identity.Name.Equals(u.UserName)).FirstOrDefault();
            MyReservationViewModel model = new MyReservationViewModel();
            model.User = user;
            model.TestDriveVehicles = new List<Vehicle>();
            model.ReservedVehicle = db.Vehicles.Find(user.ReservedVehicleId);
            var reservations = db.VehicleReservations.ToList();
            foreach (VehicleReservation res in reservations) 
            {
                if (res.UserId.Equals(user.Id) && res.IsTestDrive) 
                {
                    var vehicle = db.Vehicles.Find(res.VehicleId);
                    model.TestDriveVehicles.Add(vehicle);
                }
            }
            return View(model);
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

using AutoDealership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoDealership.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            ViewBag.Title = "Home Page";
            ViewBag.ActiveNav = "Home";
            ViewData.Model = db.Vehicles.ToList();
            ViewData["Brands"] = db.Brands.ToList();
            return View();
        }

        public ActionResult Inventory(string search)
        {
            var db = new ApplicationDbContext();
            List<Vehicle> vehicles = db.Vehicles.ToList();
            List<Vehicle> toDisplay = new List<Vehicle>();
            var brands = db.Brands.ToList();
            InventoryViewModel model = new InventoryViewModel();
            if (search != null)
                model.SearchQuery = search.Split(' ');
            else
                model.SearchQuery = new string[0];
            if (!String.IsNullOrEmpty(search))
            {
                if (search.Any(Char.IsWhiteSpace))
                {
                    string[] query = search.Split(' ');
                    foreach (string q in query)
                    {
                        int year;
                        Int32.TryParse(q, out year);
                        toDisplay.AddRange(vehicles.Where(v => v.Model.ToLower().Contains(q.ToLower()) || v.Year.Equals(year)));
                        var brand = brands.Where(b => b.Name.ToLower().Contains(q.ToLower())).FirstOrDefault();
                        if(brand != null)
                        {
                            toDisplay.AddRange(toDisplay.Where(v => v.BrandId.Equals(brand.Id)));
                        }
                    }
                }
                else 
                {
                    int year;
                    Int32.TryParse(search, out year);
                    toDisplay.AddRange(vehicles.Where(v => v.Model.ToLower().Contains(search.ToLower()) || v.Year.Equals(year)));
                    var brand = brands.Where(b => b.Name.ToLower().Contains(search.ToLower())).FirstOrDefault();
                    if (brand != null)
                    {
                        toDisplay.AddRange(vehicles.Where(v => v.BrandId.Equals(brand.Id)));
                    }
                }
                model.Inventory = toDisplay.Distinct().ToList();
            }
            else
            {
                model.Inventory = db.Vehicles.ToList();
            }
            model.AllBrands = brands;
            ViewData.Model = model;
            ViewData["Brands"] = db.Brands.ToList();
            if(model.Inventory.Count > 0)
            {
                ViewBag.MinPrice = model.Inventory.Select(v => v.Price).Min();
                ViewBag.MaxPrice = model.Inventory.Select(v => v.Price).Max();
            }
            else
            {
                ViewBag.MinPrice = 0;
                ViewBag.MaxPrice = 10000;
            }
            return View();
        }
        [HttpGet]
        public ActionResult InventoryByBodyStyle(string search)
        {
            var db = new ApplicationDbContext();
            var brands = db.Brands.ToList();
            List<Vehicle> vehicles = db.Vehicles.ToList();
            List<Vehicle> toDisplay = new List<Vehicle>();
            InventoryViewModel model = new InventoryViewModel();
            toDisplay = vehicles.Where(veh => veh.BodyStyle.ToString().ToLower().Equals(search.ToLower())).ToList();
            model.Inventory = toDisplay;
            model.SearchQuery = search.Split(' ');
            model.AllBrands = brands;
            ViewData["Brands"] = db.Brands.ToList();
            return View("Inventory", model);
        }

        public ActionResult About()
        {
            ViewBag.ActiveNav = "About";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.ActiveNav = "Contact";
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static string VehicleToString(Vehicle veh)
        {
            var db = new ApplicationDbContext();
            Brand brand = db.Brands.Find(veh.BrandId);
            return String.Format("{0} {1} {2}", veh.Year, brand.Name, veh.Model);
        }
    }
}
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
            if (!String.IsNullOrEmpty(search))
            {
                if (search.Any(Char.IsWhiteSpace))
                {
                    string[] query = search.Split(' ');
                    foreach (string q in query)
                    {
                        toDisplay.AddRange(vehicles.Where(v => v.Model.ToLower().Contains(q.ToLower())));
                    }
                }
                else 
                {
                    toDisplay.AddRange(vehicles.Where(v => v.Model.ToLower().Contains(search.ToLower())));
                }
                ViewData.Model = toDisplay;
            }
            else
            {
                ViewData.Model = db.Vehicles.ToList();
            }
            ViewData["Brands"] = db.Brands.ToList();
            return View();
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
    }
}
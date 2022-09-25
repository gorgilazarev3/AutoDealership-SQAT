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

        public ActionResult Inventory()
        {
            var db = new ApplicationDbContext();
            ViewBag.ActiveNav = "Home";
            ViewData["Brands"] = db.Brands.ToList();
            return View(db.Vehicles.ToList());
        }
    }
}
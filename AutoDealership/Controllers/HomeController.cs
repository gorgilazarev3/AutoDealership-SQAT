﻿using AutoDealership.Models;
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
            ViewBag.ActiveNav = "Inventory";
            ViewData["ActiveNav"] = "Inventory";
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
            ViewBag.ActiveNav = "Inventory";
            ViewData["ActiveNav"] = "Inventory";
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

        [HttpGet]
        public ActionResult InventoryByPrice(int price, string category)
        {
            ViewBag.ActiveNav = "Inventory";
            ViewData["ActiveNav"] = "Inventory";
            var db = new ApplicationDbContext();
            var brands = db.Brands.ToList();
            List<Vehicle> vehicles = db.Vehicles.ToList();
            List<Vehicle> toDisplay = new List<Vehicle>();
            InventoryViewModel model = new InventoryViewModel();
            if(category == null || (category != null && !category.ToLower().Equals("monthly")))
                toDisplay = vehicles.Where(veh => veh.Price < price).ToList();

            if(category != null && category.ToLower().Equals("family"))
            {
                toDisplay = toDisplay.Where(veh => veh.BodyStyle.ToString().ToLower().Equals("hatchback") || veh.BodyStyle.ToString().ToLower().Equals("sedan") || veh.BodyStyle.ToString().ToLower().Equals("van") || veh.BodyStyle.ToString().ToLower().Equals("suv")).ToList();
            }
            else if(category != null && category.ToLower().Equals("sporty"))
            {
                toDisplay = toDisplay.Where(veh => veh.Horsepower >= 250).ToList();
            }
            else if (category != null && category.ToLower().Equals("suvs"))
            {
                toDisplay = toDisplay.Where(veh => veh.BodyStyle.ToString().ToLower().Equals("suv")).ToList();
            }
            else if (category != null && category.ToLower().Equals("economy"))
            {
                toDisplay = toDisplay.Where(veh => veh.FuelEfficiency <= 6).ToList();
            }
            else if (category != null && category.ToLower().Equals("monthly"))
            {
                toDisplay = vehicles.Where(veh => veh.MonthlyPayment < price && veh.IsForLease).ToList();
            }
            model.Inventory = toDisplay;
            model.SearchQuery = category.Split(' ');
            model.AllBrands = brands;
            ViewData["Brands"] = db.Brands.ToList();
            return View("Inventory", model);
        }

        [HttpGet]
        public ActionResult SpeedInventory()
        {
            ViewBag.ActiveNav = "Inventory";
            ViewData["ActiveNav"] = "Speed - High-Performance Catalog";
            var db = new ApplicationDbContext();
            var brands = db.Brands.ToList();
            List<Vehicle> vehicles = db.Vehicles.ToList();
            List<Vehicle> toDisplay = vehicles.Where(veh => veh.Horsepower >= 550).ToList();
            InventoryViewModel model = new InventoryViewModel();
            model.Inventory = toDisplay;
            model.SearchQuery = new string[] { "High-Performance", "Power", "Speed", "Exotic" };
            model.AllBrands = brands;
            ViewData["Brands"] = db.Brands.ToList();
            return View("Inventory", model);
        }

        public ActionResult About()
        {
            ViewBag.ActiveNav = "About";
            ViewBag.Message = "Your application description page.";
            ViewData["ActiveNav"] = "About";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.ActiveNav = "Contact";
            ViewBag.Message = "Your contact page.";
            ViewData["ActiveNav"] = "Contact";
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
using AutoDealership.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using AutoDealership.Controllers;
using Unity.Injection;

namespace AutoDealership
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<DbContext, ApplicationDbContext>();
            container.RegisterType(typeof(IUserStore<>), typeof(UserStore<>));
            // Will resolve both concrete types
            var userStore1 = container.Resolve<IUserStore<ApplicationUser>>();
            var userStore2 = container.Resolve<IUserStore<IdentityUser>>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
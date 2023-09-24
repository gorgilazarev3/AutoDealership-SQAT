using AutoDealership.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AutoDealership.IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Db = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=AutoDealership-TestDb;Integrated Security=True");
            Db.Open();
            // ... initialize data in the test database ...
            context = new ApplicationDbContext(Db.ConnectionString);
            context.Database.CreateIfNotExists();
            if (context.Database != null)
            {
                roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };
                roleManager.Create(new IdentityRole(TestConstants.UserCustomerRole));
                roleManager.Create(new IdentityRole(TestConstants.UserAdminRole));
                string password = TestConstants.UserPassword;
                var user = new ApplicationUser();
                user.Email = TestConstants.UserEmail;
                user.UserName = user.Email;
                user.FullName = TestConstants.UserFullName;
                try
                {
                    var result = userManager.Create(user, password);
                    if (result.Succeeded)
                    {
                        userManager.AddToRole(user.Id, TestConstants.UserCustomerRole);
                        userManager.AddToRole(user.Id, TestConstants.UserAdminRole);
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);  
                }
                var veh = new Vehicle
                {
                    BrandId = 0,
                    Model = "E63s",
                    FuelType = Fuel.GAS,
                    Features = "Test",
                    BodyStyle = Models.Types.BodyStyle.SEDAN,
                    Price = 100000,
                    Color = "Gray",
                    Horsepower = 612,
                    Year = 2018,
                    Transmission = new Models.Types.Transmission { NumberSpeeds = 9, TransmissionType = Models.Types.TransmissionType.AUTOMATIC }

                };
                var brand = new Brand
                {
                    Id = 0,
                    Name = "Mercedes",
                    Vehicles = new List<Vehicle>
                    { veh}

                };


                context.Brands.Add(brand);
                context.Vehicles.Add(veh);
                context.SaveChanges();

            }
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            var user = userManager.FindByName(TestConstants.UserEmail);
            if(user != null)
            {
                var result = userManager.Delete(user);

            }
            context.Brands.RemoveRange(context.Brands); 
            context.Vehicles.RemoveRange(context.Vehicles);
            context.VehicleReservations.RemoveRange(context.VehicleReservations);
            context.SaveChanges();
            context.Dispose();
            roleManager.Dispose();
            userManager.Dispose();
            Db.Dispose();
        }

        public SqlConnection Db { get; private set; }
        public ApplicationDbContext context { get; private set; }
        public RoleManager<IdentityRole> roleManager;
        public UserManager<ApplicationUser> userManager;
    }

    [CollectionDefinition("TestDatabase")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

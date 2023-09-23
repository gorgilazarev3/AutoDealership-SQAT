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

namespace AutoDealership.SeleniumTests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Db = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=AutoDealership-TestDb;Integrated Security=True");
            Db.Open();
            // ... initialize data in the test database ...
            context = new ApplicationDbContext(Db.ConnectionString);
            if(context.Database != null)
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
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);  
                }
                

            }
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            var user = userManager.FindByName(TestConstants.UserEmail);
            var result = userManager.Delete(user);
            roleManager.Dispose();
            userManager.Dispose();
        }

        public SqlConnection Db { get; private set; }
        public ApplicationDbContext context { get; private set; }
        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
    }

    [CollectionDefinition("TestDatabase")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

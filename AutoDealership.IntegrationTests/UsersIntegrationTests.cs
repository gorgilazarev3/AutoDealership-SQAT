using AutoDealership.Controllers;
using AutoDealership.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace AutoDealership.IntegrationTests
{
    [Collection("TestDatabase")]
    public class UsersIntegrationTests
    {
        DatabaseFixture fixture;
        AccountController controller;

        public UsersIntegrationTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            controller = new AccountController();
        }

        [Fact]
        public async Task Admin_User_Accesses_All_RolesAsync()
        {
            var user = fixture.userManager.Find(TestConstants.UserEmail, TestConstants.UserPassword);
            Assert.NotNull(user);
            var model = new LoginViewModel
            {
                Email = TestConstants.UserEmail,
                Password = TestConstants.UserPassword,
                RememberMe = false
            };
            //Checking if the user is authenticated and that it is in the correct role
            var identity = await fixture.userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var contextUser = new ClaimsPrincipal(identity);
            Assert.True(contextUser.Identity.IsAuthenticated);
            Assert.True(contextUser.IsInRole(TestConstants.UserAdminRole));
            //Let's get all roles and verify that the roles are there
            var resultRoles = controller.Roles(fixture.context) as ViewResult;
            var rolesModel = resultRoles.Model as RolesViewModel;
            Assert.NotNull(rolesModel);
            var rolesStrings = rolesModel.Roles.Select(r => r.Name).ToList();
            Assert.Contains(TestConstants.UserCustomerRole, rolesStrings);
            Assert.Contains(TestConstants.UserAdminRole, rolesStrings);
        }

        [Fact]
        public async Task Admin_User_Can_Create_New_RoleAsync()
        {
            var user = fixture.userManager.Find(TestConstants.UserEmail, TestConstants.UserPassword);
            Assert.NotNull(user);
            var model = new LoginViewModel
            {
                Email = TestConstants.UserEmail,
                Password = TestConstants.UserPassword,
                RememberMe = false
            };
            //Checking if the user is authenticated and that it is in the correct role
            var identity = await fixture.userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var contextUser = new ClaimsPrincipal(identity);
            Assert.True(contextUser.Identity.IsAuthenticated);
            Assert.True(contextUser.IsInRole(TestConstants.UserAdminRole));
            //Let's get all roles and verify that the roles are there
            var modelRoleCreate = new RolesViewModel
            {
                NewRole = "TestRole",
                Roles = fixture.roleManager.Roles
            };
            var resultCreate = controller.Roles(modelRoleCreate, fixture.context) as ViewResult;
            var rolesStrings = fixture.roleManager.Roles.Select(r => r.Name).ToList();
            Assert.Contains("TestRole", rolesStrings);
            //Let's delete the role now
            var resultDelete = controller.DeleteRole("TestRole", fixture.context);
            //Check that the count of roles is back to two
            Assert.Equal(2, fixture.roleManager.Roles.Count());
        }

        [Fact]
        public async Task Admin_User_Accesses_All_UsersAsync()
        {
            var user = fixture.userManager.Find(TestConstants.UserEmail, TestConstants.UserPassword);
            Assert.NotNull(user);
            var model = new LoginViewModel
            {
                Email = TestConstants.UserEmail,
                Password = TestConstants.UserPassword,
                RememberMe = false
            };
            //Checking if the user is authenticated and that it is in the correct role
            var identity = await fixture.userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            var contextUser = new ClaimsPrincipal(identity);
            Assert.True(contextUser.Identity.IsAuthenticated);
            Assert.True(contextUser.IsInRole(TestConstants.UserAdminRole));
            //Let's get all roles and verify that the roles are there
            var result = controller.AllUsers(fixture.context) as ViewResult;
            var allUsers = result.Model as List<ApplicationUser>;
            Assert.NotNull(allUsers);
            //Check if the user is returned with all users since it's the only user
            Assert.Contains(user, allUsers);
            //Since we only have one user, the collection should be a single
            Assert.Single(allUsers);
        }
    }
}

using AutoDealership.Models;
using AutoDealership.Models.Types;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Xunit;

namespace AutoDealership.SeleniumTests
{
    [Collection("SequentialTests")]
    [assembly: CollectionBehavior(DisableTestParallelization = true)]
    public class UserTests
    {

        private void UserLogin(ChromeDriver driver)
        {
            //Find the log in button and click it
            IWebElement logInLink = driver.FindElement(By.CssSelector("body .my-navbar-account-links a[href='/Account/Login']"));
            Assert.True(logInLink.Displayed);
            logInLink.Click();
            //Check if the url is the login url
            string loginUrl = "https://localhost:44378/Account/Login";
            Assert.Equal(loginUrl, driver.Url);
            //Find the inputs
            IWebElement usernameInput = driver.FindElement(By.CssSelector(".card-body #loginForm form input[name='Email']"));
            IWebElement passwordInput = driver.FindElement(By.CssSelector(".card-body #loginForm form input[name='Password']"));
            Assert.True(usernameInput.Displayed);
            Assert.True(passwordInput.Displayed);
            //Enter the data for the user
            usernameInput.SendKeys(TestConstants.UserEmail);
            passwordInput.SendKeys(TestConstants.UserPassword);
            IWebElement logInBtn = driver.FindElement(By.CssSelector("#loginForm > form > div.form-group.mb-3 > div > input"));
            Assert.True(logInBtn.Displayed);
            Assert.Equal("log in", logInBtn.GetAttribute("value").ToLower());
            //Try to log in
            logInBtn.Click();
        }


        [Fact]
        public void User_Cannot_Access_The_User_Dashboard_Without_Logging_In()
        {
            string url = "https://localhost:44378/Manage/Index";
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            //Without logging in, the user will be redirected to the login page
            //this is the url that needs to be shown when user is redirected
            string returnUrl = "https://localhost:44378/Account/Login?ReturnUrl=%2FManage%2FIndex";
            Assert.Equal(returnUrl, driver.Url);
            IWebElement logInHeader = driver.FindElement(By.CssSelector("body .card-body > h6.display-6.text-center"));
            //Let's assert whether this header is visible and the text is log in
            Assert.True(logInHeader.Displayed);
            Assert.Equal("log in", logInHeader.Text.ToLower());
        }

        [Fact]
        public void User_Log_In_Is_Successful()
        {
            string url = "https://localhost:44378";
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            UserLogin(driver);
            //If logged in, the profile-icon should be visible
            IWebElement profileIcon = driver.FindElement(By.CssSelector(".d-flex a#profile-icon"));
            Assert.True(profileIcon.Displayed);
        }

        [Fact]
        public void Admin_User_Has_Access_To_Admin_Dashboard()
        {
            string url = "https://localhost:44378";
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            UserLogin(driver);
            //If logged in, the profile-icon should be visible
            IWebElement profileIcon = driver.FindElement(By.CssSelector(".d-flex a#profile-icon"));
            Assert.True(profileIcon.Displayed);
            //Click the profile icon and go to the dashboard
            profileIcon.Click();
            IWebElement dashboardBtn = driver.FindElement(By.CssSelector("a#manage-dashboard"));
            Assert.True(dashboardBtn.Displayed);
            dashboardBtn.Click();
            //Wait two seconds for the panel to load
            Thread.Sleep(2000);
            //Check if the panel for all users is visible and that the admin user has access to the resource
            IWebElement allUsersPanel = driver.FindElement(By.CssSelector("#manage-div a[href='/Account/AllUsers'].dashboard-panel"));
            Assert.True(allUsersPanel.Displayed);
            allUsersPanel.Click();
            IWebElement allUsersHeader = driver.FindElement(By.CssSelector("#about-header h2"));
            Assert.Equal("list of all users", allUsersHeader.Text.ToLower());
        }

        [Fact]
        public void User_Searching_For_Volkswagen_Golf_Returns_Correct_Vehicles()
        {
            string url = "https://localhost:44378";
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            IWebElement searchBar = driver.FindElement(By.CssSelector(".search-input-div input#search-vehicles"));
            searchBar.SendKeys("golf");
            searchBar.SendKeys(Keys.Enter);
            string searchUrl = "https://localhost:44378/Home/Inventory?search=golf";
            Assert.Equal(searchUrl, driver.Url);
            IWebElement firstVehicleCardTitle = driver.FindElements(By.CssSelector("#vehicles-partial-div .vehicle-card .card-body h5.card-title")).First();
            Assert.True(firstVehicleCardTitle.Displayed);
            Assert.Contains("golf", firstVehicleCardTitle.Text.ToLower());
        }

        [Fact]
        public void User_Has_Recent_Viewed_Vehicles()
        {
            string url = "https://localhost:44378";
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            UserLogin(driver);
            //Search for a vehicle and enter it
            IWebElement searchBar = driver.FindElement(By.CssSelector(".search-input-div input#search-vehicles"));
            searchBar.SendKeys("golf");
            searchBar.SendKeys(Keys.Enter);
            string searchUrl = "https://localhost:44378/Home/Inventory?search=golf";
            Assert.Equal(searchUrl, driver.Url);
            //Validate that the vehicle is there
            IWebElement firstVehicleCardTitle = driver.FindElements(By.CssSelector("#vehicles-partial-div .vehicle-card .card-body h5.card-title")).First();
            Assert.True(firstVehicleCardTitle.Displayed);
            Assert.Contains("golf", firstVehicleCardTitle.Text.ToLower());
            //Open the listing
            IWebElement firstVehicleCard = driver.FindElements(By.CssSelector("#vehicles-partial-div .vehicle-card")).First();
            Assert.True(firstVehicleCard.Displayed);
            firstVehicleCard.Click();
            //Scroll to the bottom of the page to find the also viewed section
            driver.Manage().Window.Maximize();
            IWebElement alsoViewedByYouHeader = driver.FindElement(By.CssSelector("body > div > div.container > div > div.row > h4"));
            Assert.True(alsoViewedByYouHeader.Displayed);
            Assert.Equal("also viewed by you...", alsoViewedByYouHeader.Text.ToLower());
            //Verify that the last viewed section has at least one listing in it
            Assert.True(driver.FindElements(By.CssSelector("body > div > div.container > div > div.row .row-cols-5 .vehicle-card")).Count >= 1);
        }

        [Fact]
        public void User_Reserves_A_Vehicle_Successfully_And_Cancels_The_Reservation()
        {
            string url = "https://localhost:44378";
            ChromeDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl(url);
            UserLogin(driver);
            //If logged in, the profile-icon should be visible
            IWebElement profileIcon = driver.FindElement(By.CssSelector(".d-flex a#profile-icon"));
            Assert.True(profileIcon.Displayed);
            //Click the profile icon and go to the dashboard
            profileIcon.Click();
            IWebElement dashboardBtn = driver.FindElement(By.CssSelector("a#manage-dashboard"));
            Assert.True(dashboardBtn.Displayed);
            dashboardBtn.Click();
            //Wait two seconds for the panel to load
            Thread.Sleep(2000);
            //Check if the panel for all users is visible and that the user has no reservations
            IWebElement myReservationsPanel = driver.FindElement(By.CssSelector("#manage-div a[href='/Vehicle/MyReservations'].dashboard-panel"));
            Assert.True(myReservationsPanel.Displayed);
            //Because the panel is not in the screen, let's scroll to the element
            Actions actions = new Actions(driver);
            driver.Manage().Window.Maximize();
            myReservationsPanel.Click();
            IWebElement myReservationsHeader = driver.FindElement(By.CssSelector("#about-header h2"));
            IWebElement noReservationsText = driver.FindElement(By.CssSelector("body > div > div.container > h4"));
            Assert.Equal("my reservations", myReservationsHeader.Text.ToLower());
            Assert.Equal("you haven't reserved any vehicles", noReservationsText.Text.ToLower());
            //After verifying that the user has no reservations go back to the home page and reserve a vehicle
            IWebElement homeLink = driver.FindElement(By.CssSelector(".navbar-nav #nav-item-home a.nav-link"));
            Assert.True(homeLink.Displayed);
            homeLink.Click();
            //Search for the vehicle
            IWebElement searchBar = driver.FindElement(By.CssSelector(".search-input-div input#search-vehicles"));
            Assert.True(searchBar.Displayed);
            searchBar.SendKeys("golf");
            searchBar.SendKeys(Keys.Enter);
            string searchUrl = "https://localhost:44378/Home/Inventory?search=golf";
            Assert.Equal(searchUrl, driver.Url);
            IWebElement firstVehicleCardTitle = driver.FindElements(By.CssSelector("#vehicles-partial-div .vehicle-card .card-body h5.card-title")).First();
            Assert.True(firstVehicleCardTitle.Displayed);
            Assert.Contains("golf", firstVehicleCardTitle.Text.ToLower());
            IWebElement firstVehicleCard = driver.FindElements(By.CssSelector("#vehicles-partial-div .vehicle-card")).First();
            Assert.True(firstVehicleCard.Displayed);
            firstVehicleCard.Click();
            //Let's make sure that the vehicle isn't reserved and that the reserve btn is visible
            IWebElement reserveBtn = driver.FindElement(By.CssSelector("button#reserve-btn"));
            Assert.True(reserveBtn.Displayed);
            actions.ScrollByAmount(0, 500);
            actions.Perform();
            reserveBtn.Click();
            //Wait half a second to load up the modal
            Thread.Sleep(500);
            //Enter some date for the reservation and reserve the vehicle
            IWebElement modalTitle = driver.FindElement(By.CssSelector(".modal-header h5#reserve-vehicle-modalLabel"));
            Assert.True(modalTitle.Displayed);
            Assert.Equal("reserve this vehicle", modalTitle.Text.ToLower());
            IWebElement reservedUntilInput = driver.FindElement(By.CssSelector(".form-group input#ReservedUntil"));
            Assert.True(reservedUntilInput.Displayed);
            reservedUntilInput.SendKeys("10/31/2023");
            reservedUntilInput.SendKeys(Keys.Tab);
            reservedUntilInput.SendKeys("0520PM");
            IWebElement confirmReservationBtn = driver.FindElement(By.CssSelector(".form-group input#confirm-reservation-btn"));
            Assert.True(confirmReservationBtn.Displayed);
            confirmReservationBtn.Click();
            //Wait two seconds for the redirection and check if the vehicle is reserved(there is no reservation btn)
            Thread.Sleep(2000);
            //IWebElement reservedBtn = driver.FindElement(By.CssSelector("button#reserve-btn"));
            //Assert.True(reservedBtn == null);
            //After this we need to navigate back to the dashboard to check if the reservation is successful
            profileIcon = driver.FindElement(By.CssSelector(".d-flex a#profile-icon"));
            Assert.True(profileIcon.Displayed);
            profileIcon.Click();
            dashboardBtn = driver.FindElement(By.CssSelector("a#manage-dashboard"));
            Assert.True(dashboardBtn.Displayed);
            dashboardBtn.Click();
            //Wait two seconds for the panel to load
            Thread.Sleep(2000);
            //Check if the panel for all users is visible and that the user has one reservation now
            myReservationsPanel = driver.FindElement(By.CssSelector("#manage-div a[href='/Vehicle/MyReservations'].dashboard-panel"));
            Assert.True(myReservationsPanel.Displayed);
            myReservationsPanel.Click();
            //Let's check that the correct vehicle is reserved
            IWebElement reservedVehicleTitle = driver.FindElement(By.CssSelector("div.container div.col h4.h4.fw-bold"));
            Assert.True(reservedVehicleTitle.Displayed);
            Assert.Equal("golf 7", reservedVehicleTitle.Text.ToLower());
            //If this is successful let's cancel the reservation since this is only a test
            IWebElement cancelReservationBtn = driver.FindElement(By.CssSelector("div.d-flex.col button.cancel-reservation-btn"));
            Assert.True(cancelReservationBtn.Displayed);
            cancelReservationBtn.Click();
            //Let's wait half a second for the modal to be displayed
            Thread.Sleep(500);
            IWebElement bootboxBody = driver.FindElement(By.CssSelector("div.bootbox-body"));
            Assert.True(bootboxBody.Displayed);
            IWebElement bootboxBtnYes = driver.FindElement(By.CssSelector("div.modal-footer button.bootbox-accept"));
            Assert.True(bootboxBtnYes.Displayed);
            bootboxBtnYes.Click();
            //Let's wait two seconds for the cancelling to take place and check that the reservation is no longer there
            Thread.Sleep(2000);
            //Also let's refresh for the title to be updated
            driver.Navigate().Refresh();
            myReservationsHeader = driver.FindElement(By.CssSelector("#about-header h2"));
            noReservationsText = driver.FindElement(By.CssSelector("body > div > div.container > h4"));
            Assert.Equal("my reservations", myReservationsHeader.Text.ToLower());
            Assert.Equal("you haven't reserved any vehicles", noReservationsText.Text.ToLower());
        }
    }
}

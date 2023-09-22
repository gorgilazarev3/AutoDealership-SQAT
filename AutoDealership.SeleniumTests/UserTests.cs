using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Xunit;

namespace AutoDealership.SeleniumTests
{
    public class UserTests
    {
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
    }
}

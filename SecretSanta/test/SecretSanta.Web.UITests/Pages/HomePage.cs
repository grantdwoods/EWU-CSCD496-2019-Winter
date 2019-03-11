using OpenQA.Selenium;
using SecretSanta.Web.UITests.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Web.UITests
{
    public class HomePage
    {
        public IWebDriver Driver { get; }

        public GroupsPage GroupPage => new GroupsPage(Driver);
        public UsersPage UsersPage => new UsersPage(Driver);

        //Id, LinkText, CssSelector/XPath
        //public IWebElement GroupsLink => Driver.FindElement(By.CssSelector("a[href=\"/Groups\"]"));
        public IWebElement GroupsLink => Driver.FindElement(By.LinkText("Groups"));
        public IWebElement UsersLink => Driver.FindElement(By.LinkText("Users"));

        public HomePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}

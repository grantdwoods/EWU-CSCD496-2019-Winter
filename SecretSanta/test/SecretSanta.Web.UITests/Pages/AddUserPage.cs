﻿using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Web.UITests.Pages
{
    class AddUserPage
    {
        IWebDriver Driver { get; }
        public const string Slug = UsersPage.Slug + "/Add";
        public IWebElement FirstNameTextBox => Driver.FindElement(By.Id("FirstName"));
        public IWebElement LastNameTextBox => Driver.FindElement(By.Id("LastName"));
        public IWebElement HomePageLink => Driver.FindElement(By.LinkText("Secret Santa"));
        public IWebElement CancelLink => Driver.FindElement(By.LinkText("Cancel"));
        public IWebElement ErrorElement => 
            Driver.FindElement(By.CssSelector("div.container>div>ul>li"));

        public IWebElement SubmitButton =>
            Driver
                .FindElements(By.CssSelector("button.is-primary"))
                .Single(x => x.Text == "Submit");

        public AddUserPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Threading;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        private const string WinAppDriver = @"C:\Program Files\Windows Application Driver\WinAppDriver.exe";
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string applicationFullPathName = @"C:\FRC\2023SummerProject\codeGenerator\src\FRCrobotCodeGen302\bin\Debug\FRCrobotCodeGen302.exe";

        private static WindowsDriver<WindowsElement> Session { get; set; }
        private static Process theWinAppDriverProcess = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            theWinAppDriverProcess = Process.Start(WinAppDriver);
            Thread.Sleep(TimeSpan.FromSeconds(2)); // to allow time for the win app driver to start 

            AppiumOptions options = new AppiumOptions();
            options.AddAdditionalCapability("app", applicationFullPathName);
            options.AddAdditionalCapability("appArguments", "enableAutomation");
            options.AddAdditionalCapability("deviceName", "WindowsPC");

            Session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), options);
            Assert.IsNotNull(Session);

            // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
            Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Close the application and delete the session
            if (Session != null)
            {
                Session.Quit();
                Session = null;
            }

            if (theWinAppDriverProcess != null)
                theWinAppDriverProcess.Kill();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Session.FindElementByName("Configuration").Click();

            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // time to switch tabs

            Session.FindElementByAccessibilityId("nodePathSelectorTextBox").Click();
            Session.FindElementByAccessibilityId("nodePathSelectorTextBox").SendKeys("Robot Variant/robots/Robot #1");
            Session.FindElementByAccessibilityId("selectNodeButton").Click();

            Thread.Sleep(TimeSpan.FromSeconds(5)); // to give us time to see what is going on 
        }

        [TestMethod]
        public void TestMethod2()
        {
            Session.FindElementByName("Configuration").Click();

            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // time to switch tabs

            Session.FindElementByAccessibilityId("nodePathSelectorTextBox").Click();
            Session.FindElementByAccessibilityId("nodePathSelectorTextBox").Clear();
            Session.FindElementByAccessibilityId("nodePathSelectorTextBox").SendKeys("Robot Variant/mechanisms/Turret");
            Session.FindElementByAccessibilityId("selectNodeButton").Click();

            Thread.Sleep(TimeSpan.FromSeconds(15)); // to give us time to see what is going on 
        }
    }
}

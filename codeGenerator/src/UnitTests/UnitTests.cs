using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

        //============================ configurable settings
        private static string testRobotConfigurationFile = "testRobotConfiguration.xml";
        private static string configurationFileReference = "configurationFileReference.xml";
        private static string configurationFile = "configuration.xml";

        private static string baseTestDirectory = "guiTests";
        private static string referenceFilesDirectory = "referenceFiles";
        private static string configDirectory = "configuration";
        private static string robotConfigDirectory = "robotConfiguration";

        // debug settings
        private double pauseAtEndOfEachTest = 1;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            string codeGeneratorDir = Path.GetFullPath(Path.Combine(testContext.TestDir, "..", "..", ".."));
            
            baseTestDirectory = Path.Combine(codeGeneratorDir, baseTestDirectory);
            referenceFilesDirectory = Path.Combine(baseTestDirectory, referenceFilesDirectory);
            robotConfigDirectory = Path.Combine(baseTestDirectory, robotConfigDirectory);
            configDirectory = Path.Combine(baseTestDirectory, configDirectory);

            testRobotConfigurationFile = Path.Combine(robotConfigDirectory, testRobotConfigurationFile);
            configurationFileReference = Path.Combine(baseTestDirectory, configurationFileReference);
            configurationFile = Path.Combine(configDirectory, configurationFile);

            Console.WriteLine(codeGeneratorDir);
            Console.WriteLine(baseTestDirectory);
            Console.WriteLine(referenceFilesDirectory);
            Console.WriteLine(robotConfigDirectory);

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

        /// <summary>
        /// Creates a new Robot variants configuration in a clean directory
        /// </summary>
        [TestMethod]
        public void TestMethod_00000_CreateNewConfiguration()
        {
            #region directory cleanup
            if (Directory.Exists(robotConfigDirectory))
                Directory.Delete(robotConfigDirectory, true);
            Directory.CreateDirectory(robotConfigDirectory);

            if (Directory.Exists(configDirectory))
                Directory.Delete(configDirectory, true);
            Directory.CreateDirectory(configDirectory);
            #endregion

            File.Copy(configurationFileReference, configurationFile);

            Session.FindElementByName("Main").Click();

            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // time to switch tabs

            Session.FindElementByAccessibilityId("configurationBrowseButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1)); // wait a bit for the browse file dialog to pop up
            Session.FindElementByXPath("//Edit[@Name='File name:']").SendKeys(configurationFile);
            Thread.Sleep(TimeSpan.FromSeconds(1.5));
            Session.FindElementByXPath("//Edit[@Name='File name:']").SendKeys(Keys.Enter);

            Thread.Sleep(TimeSpan.FromSeconds(3)); // time for config to load

            Session.FindElementByAccessibilityId("createNewRobotVariantsConfigButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1)); // wait a bit for the save file dialog to pop up
            Session.FindElementByXPath("//Edit[@Name='File name:']").SendKeys(testRobotConfigurationFile);
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            Session.FindElementByName("Save").Click();

            Thread.Sleep(TimeSpan.FromSeconds(3)); // wait for the file to be written
            Assert.IsTrue(File.Exists(testRobotConfigurationFile));
        }

        [TestMethod]
        public void TestMethod_00001_SetRobotNumber()
        {
            Session.FindElementByName("Configuration").Click();

            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // time to switch tabs

            selectTreeNodeAndCheck(@"Robot Variant\robots\Robot #1\robotID (1)");

            setNumericUpDown(2);

            selectTreeNodeAndCheck(@"Robot Variant\robots\Robot #2\robotID (2)");

            clickSave();
        }

        [TestMethod]
        public void TestMethod_00002_AddAMechanismTemplate()
        {
            Session.FindElementByName("Configuration").Click();

            Thread.Sleep(TimeSpan.FromSeconds(0.5)); // time to switch tabs

            selectTreeNodeAndCheck(@"Robot Variant");

            addRobotElement("mechanism");
            

            selectTreeNodeAndCheck(@"Robot Variant\mechanisms\UNKNOWN\name (UNKNOWN)");
            setTextInput("Super_Intake");
            selectTreeNodeAndCheck(@"Robot Variant\mechanisms\Super_Intake\name (Super_Intake)");

            selectTreeNodeAndCheck(@"Robot Variant\mechanisms\Super_Intake");
            addRobotElement("Falcon_Motor");
            //selectTreeNodeAndCheck(@"Robot Variant\mechanisms\Super_Intake");

            selectTreeNodeAndCheck(@"Robot Variant\mechanisms\Super_Intake");
            addRobotElement("solenoid");



            clickSave();
        }

        [TestCleanup]
        public void AfterEveryTest()
        {
            Thread.Sleep(TimeSpan.FromSeconds(pauseAtEndOfEachTest)); // to give us time to see what is going on 
        }

        #region ====================================== Helper functions ================================================
        private void setNumericUpDown(double value)
        {
            Session.FindElementByAccessibilityId("valueNumericUpDown").Click();
            Session.FindElementByAccessibilityId("valueNumericUpDown").SendKeys(Keys.Delete);
            Session.FindElementByAccessibilityId("valueNumericUpDown").SendKeys(value.ToString());
        }

        private void setTextInput(string value)
        {
            Session.FindElementByAccessibilityId("valueTextBox").Click();
            Session.FindElementByAccessibilityId("valueTextBox").Clear();
            Session.FindElementByAccessibilityId("valueTextBox").SendKeys(value);
        }

        private void clickSave()
        {
            Session.FindElementByAccessibilityId("saveConfigBbutton").Click();
        }

        /// <summary>
        /// click on something else so that when we click on the item that we want to check next, the path textbox is updated (since the onSelect callback will fire)
        /// </summary>
        private void clickOnSomethingElse()
        {
            selectTreeNode(@"Robot Variant");
        }

        private void selectTreeNodeAndCheck(string path)
        {
            selectTreeNode(path);
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual(path, getSelectedTreeNodeFullPath());
        }
        private void selectTreeNode(string path)
        {
            Session.FindElementByAccessibilityId("infoIOtextBox").Click();
            Session.FindElementByAccessibilityId("infoIOtextBox").Clear();
            Session.FindElementByAccessibilityId("infoIOtextBox").SendKeys(path);
            Session.FindElementByAccessibilityId("selectNodeButton").Click();
        }
        private string getSelectedTreeNodeFullPath()
        {
            Session.FindElementByAccessibilityId("getSelectedTreeElementPathButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            return Session.FindElementByAccessibilityId("infoIOtextBox").Text;
        }
        private List<string> getListOfAvailableRobotElements()
        {
            List<string> names = new List<string>();

            Session.FindElementByAccessibilityId("getCheckBoxListItemsButton").Click();
            string items = Session.FindElementByAccessibilityId("infoIOtextBox").Text;
            names = items.Trim('#').Split('#').ToList();
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                string searchString = "Robot.";
                if (name.Length > searchString.Length)
                {
                    int index = name.IndexOf(searchString);
                    name = name.Substring(index + searchString.Length);
                    name = name.Trim(']');
                }
                names[i] = name;
            }
            return names;
        }

        private void checkmarkRobotElement(string name)
        {
            List<string> names = getListOfAvailableRobotElements();
            int index = names.IndexOf(name);
            Assert.IsTrue(index >= 0);
            Session.FindElementByAccessibilityId("infoIOtextBox").SendKeys(index.ToString());

            Session.FindElementByAccessibilityId("checkCheckBoxListItemButton").Click();
        }

        private void addRobotElement(string name)
        {
            checkmarkRobotElement(name);

            Session.FindElementByAccessibilityId("addTreeElementButton").Click();
        }

        #endregion
    }
}

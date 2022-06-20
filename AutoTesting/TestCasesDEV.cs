using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTesting
{
    public partial class UI
    {
        [DataTestMethod, TestCategory("Batch-Testing")]
        [DataRow("12345678", DisplayName = "Item: 12345678")]
        [DataRow("87654321", DisplayName = "Item: 87654321")]
        [DataRow("ABCDEFGH", DisplayName = "Item: ABCDEFGH")]
        public void UITest_DEV_SITE0(string item)
        {
            Setup.set_local_setup("DEV", "SITE0", "N/A");
            Setup.set_user_data("", "", item, "", "");

            Setup.override_checks(check_admin: false, check_stuff: false);
            Setup.override_browser(headless: false);

            UI_Chrome();
            UI_Edge();
        }

        [DataTestMethod, TestCategory("Batch-Testing")]
        [DataRow("12345678", DisplayName = "Item: 12345678")]
        [DataRow("87654321", DisplayName = "Item: 87654321")]
        [DataRow("ABCDEFGH", DisplayName = "Item: ABCDEFGH")]
        public void UITest_DEV_SITE1(string item)
        {
            Setup.set_local_setup("DEV", "SITE1", "N/A");
            Setup.set_user_data("", "", item, "", "");

            Setup.override_checks(check_admin: false, check_stuff: false);
            Setup.override_browser(headless: false);

            UI_Chrome();
            UI_Edge();
        }
    }
}

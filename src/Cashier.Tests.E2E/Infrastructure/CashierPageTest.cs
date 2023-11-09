using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.E2E.Infrastructure
{
    public class CashierPageTest : PageTest
    {
        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions()
            {
                //ColorScheme = ColorScheme.Light,
                //ViewportSize = new()
                //{
                //    Width = 1920,
                //    Height = 1080
                //},
                BaseURL = TestConfig.BaseURL,
            };
        }
    }
}

using Cashier.Tests.E2E.Infrastructure;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.E2E.Tests
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class BasicFunctionality : PageTest
    {
        [SetUp]
        public async Task Setup()
        {
            // Always start here
            await Page.GotoAsync(TestConfig.BaseURL);
        }

        /*
         * This sucks because the navbar is not actually hidden but only moved left out of the viewable area.
         * This makes it still visible to any Playwright selector.
        [Test]
        public async Task NavigationOnOff()
        {
            // Click on the menu button
            await Page.GetByRole(AriaRole.Button).First.ClickAsync();

            // confirm there is no Asset Allocation text on the page.
            var aaLink = Page.GetByRole(AriaRole.Link, new() { Name = "Asset Allocation" });
            // await Expect(aaLink).ToBeHiddenAsync();
            // await Expect(aaLink).ToHaveCountAsync(0);
        }
        */
    }
}

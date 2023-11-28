using Cashier.Tests.E2E.Infrastructure;
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
    public class JournalCardTests : PageTest
    {
        [SetUp]
        public async Task Setup()
        {
            // Always start here
            await Page.GotoAsync(TestConfig.BaseURL);
        }
    }
}

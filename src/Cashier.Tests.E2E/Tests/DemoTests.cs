using Cashier.Tests.E2E.Infrastructure;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace Cashier.Tests.E2E.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class DemoTests : PageTest
{
    [SetUp]
    public async Task Setup()
    {
        // Always start here
        await Page.GotoAsync(TestConfig.BaseURL);
    }

    [Test]
    public async Task HasTitle()
    {
        //var title = await Page.TitleAsync();
        //Console.WriteLine("Checking for title: {0}", title);

        await Expect(Page).ToHaveTitleAsync("Cashier");
    }
}
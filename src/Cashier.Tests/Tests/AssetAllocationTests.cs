using Cashier.Lib;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class AssetAllocationTests
    {
        [Fact]
        public void TestTomlParsing()
        {
            var definition = Get6040Allocation();
            var aa = new AssetAllocation(definition);

            Assert.Equal(3, aa.classes.Count);
        }

        [Fact]
        public void TestParsingSubclasses() {
            var definition = Get6040Allocation();
            var aa = new AssetAllocation(definition);

            var root = aa.classes.First();
            Assert.NotNull(root);

            var firstChild = aa.classes.Skip(1).First();
            Assert.NotNull(firstChild);
        }

        [Fact]
        public void TestParsingSymbols() {
            var definition = Get6040Allocation();
            var aa = new AssetAllocation(definition);
            var equity = aa.classes.Skip(1).First();
            Assert.NotNull(equity);

            Assert.NotEmpty(equity.Symbols);
            Assert.Single(equity.Symbols);
            Assert.Equal("VTS", equity.Symbols.First());
        }

        [Fact]
        public void TestOutput()
        {
            var definition = Get6040Allocation();
            var aa = new AssetAllocation(definition);

            var actual = aa.GetCalculation();
            Assert.NotNull(actual);
        }

        private string Get6040Allocation()
        {
            return @"
[Allocation]
allocation = 100

[Allocation.Equity]
allocation = 60
symbols = [""VTS""]

[Allocation.Fixed]
allocation = 40
symbols = [""BND""]

";
        }
    }
}

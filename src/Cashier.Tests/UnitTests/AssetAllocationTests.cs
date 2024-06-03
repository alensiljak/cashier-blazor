using Cashier.Data;
using Cashier.Model;
using Cashier.Services;
using Cashier.Tests.Infrastructure;
using Microsoft.JSInterop;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class AssetAllocationTests
    {
        private IJSRuntime _fakeRuntime;
        private IDexieDAL _dal;
        private ISettingsService _settings;
        private IAccountService _accountService;

        public AssetAllocationTests() {
            _fakeRuntime = new Mock<IJSRuntime>().Object;

            var helper = new TestHelpers();
            _settings = helper.CreateMockSettings().Object;
            _dal = TestHelpers.CreateMockDAL().Object;
            _accountService = helper.CreateMockAccountService().Object;
        }

        [Fact]
        public async Task TestTomlParsing()
        {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            Assert.Equal(3, aa.Classes.Count);
        }

        [Fact]
        public async Task TestParsingSubclasses() {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            var root = aa.Classes.First();
            Assert.NotNull(root);

            var firstChild = aa.Classes.Skip(1).First();
            Assert.NotNull(firstChild);
        }

        [Fact]
        public async Task TestParsingSymbols() {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);
            
            var equity = aa.Classes.Skip(1).First();
            Assert.NotNull(equity);

            Assert.NotEmpty(equity.Symbols);
            Assert.Single(equity.Symbols);
            Assert.Equal("VTS", equity.Symbols.First());
        }

        [Fact]
        public async Task TestOutput()
        {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            var actual = aa.GetTextReport(aa.Classes);
            Assert.NotNull(actual);
        }

        [Fact]
        public async Task TestCurrentValues()
        {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            var actualEquity = aa.Classes.First(x => x.FullName == "Allocation:Equity").CurrentValue;
            Assert.Equal(600, actualEquity.Quantity);
            Assert.Equal("EUR", actualEquity.Currency);

            var actualFixed = aa.Classes.First(x => x.FullName == "Allocation:Fixed").CurrentValue;
            Assert.Equal(400, actualFixed.Quantity);
            Assert.Equal("EUR", actualFixed.Currency);
        }

        [Fact]
        public async Task TestGroupSums()
        {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            var actual = aa.Classes.First(ac => ac.FullName == "Allocation").CurrentValue;
            Assert.Equal(1000, actual.Quantity);
        }

        // Private

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
symbols = [""BND"", ""EUR""]

";
        }

        private AssetAllocationService CreateAssetAllocation()
        {
            return new AssetAllocationService(_settings, _dal, _accountService);
        }
    }
}

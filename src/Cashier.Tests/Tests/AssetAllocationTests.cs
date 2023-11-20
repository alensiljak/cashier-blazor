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
            _dal = helper.CreateMockDAL().Object;
            _accountService = helper.CreateMockAccountService().Object;
        }

        [Fact]
        public async Task TestTomlParsing()
        {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            Assert.Equal(3, aa.classes.Count);
        }

        [Fact]
        public async Task TestParsingSubclasses() {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            var root = aa.classes.First();
            Assert.NotNull(root);

            var firstChild = aa.classes.Skip(1).First();
            Assert.NotNull(firstChild);
        }

        [Fact]
        public async Task TestParsingSymbols() {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);
            
            var equity = aa.classes.Skip(1).First();
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

            var actual = aa.GetTextReport();
            Assert.NotNull(actual);
        }

        [Fact]
        public async Task TestCurrentValues()
        {
            var definition = Get6040Allocation();
            var aa = CreateAssetAllocation();
            await aa.loadFullAssetAllocation(definition);

            var actual = aa.classes.First(x => x.FullName == "Allocation:Equity").CurrentValue;
            Assert.Equal(new Money(0, "EUR"), actual);
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

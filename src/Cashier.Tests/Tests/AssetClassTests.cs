using Cashier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class AssetClassTests
    {
        private AssetClass _assetClass;

        public AssetClassTests() {
            _assetClass = new AssetClass();
        }

        [Fact]
        public void TestEmptyName()
        {
            Assert.Equal("", _assetClass.ParentName);
        }

        [Fact]
        public void TestParentName()
        {
            _assetClass.FullName = "Equity:International:Growth";

            var actual = _assetClass.ParentName;
            Assert.Equal("Equity:International", actual);
        }

        [Fact]
        public void TestName() { 
            _assetClass.FullName = "Equity:International:Growth";

            var actual = _assetClass.Name;
            Assert.Equal("Growth", actual);
        }

        [Fact]
        public void TestDepth3() {
            _assetClass.FullName = "Equity:International:Growth";

            Assert.Equal(3, _assetClass.Depth);
        }

        [Fact]
        public void TestDepth2()
        {
            _assetClass.FullName = "Equity:International";

            Assert.Equal(2, _assetClass.Depth);
        }

        [Fact]
        public void TestDepth1()
        {
            _assetClass.FullName = "Equity";

            Assert.Equal(1, _assetClass.Depth);
        }
    }
}

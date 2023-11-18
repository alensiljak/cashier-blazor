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
            var definition = @"
";
            var aa = new AssetAllocation(definition);

            // todo: test
        }
    }
}

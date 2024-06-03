using Cashier.Lib;
using Cashier.Tests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class ForecastTests
    {
        private ForecastCalculator _uut;

        public ForecastTests() 
        {
            // dal
            var db = TestHelpers.CreateMockDAL();
            var currency = "EUR";
            _uut = new ForecastCalculator(db.Object, currency);
        }

        [Fact]
        public void CanInstantiate()
        {
            Assert.NotNull( _uut );
        }
    }
}

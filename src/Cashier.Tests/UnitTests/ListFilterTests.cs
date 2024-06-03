using Cashier.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class ListFilterTests
    {
        private ListSearch _out;

        public ListFilterTests()
        {
            _out = new ListSearch();
        }

        [Fact]
        public void TestSearchOneTerm()
        {
            var account = "Assets:Bank:Checking";
            var searchTerm = "Check";
            var regex = _out.getRegex(searchTerm);

            var actual = regex.Match(account);

            Assert.True(actual.Success);
        }

        [Fact]
        public void TestSearchTwoTerms()
        {
            var account = "Assets:Bank:Checking";
            var searchTerm = "Check ass";
            var regex = _out.getRegex(searchTerm);

            var actual = regex.Match(account);

            Assert.True(actual.Success);
        }

        [Fact]
        public void TestSearchTwoTermsInOrder()
        {
            var account = "Assets:Bank:Checking";
            var searchTerm = "ass chec";
            var regex = _out.getRegex(searchTerm);

            var actual = regex.Match(account);

            Assert.True(actual.Success);
        }

        [Fact]
        public void TestSearch3Terms()
        {
            var account = "Assets:Bank:Checking";
            var searchTerm = "Check ass ba";
            var regex = _out.getRegex(searchTerm);

            var actual = regex.Match(account);

            Assert.True(actual.Success);
        }
    }
}

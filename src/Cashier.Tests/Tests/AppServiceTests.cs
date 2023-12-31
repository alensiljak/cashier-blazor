﻿using Bunit.TestDoubles;
using Cashier.Services;
using Cashier.Tests.Infrastructure;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests.Tests
{
    public class AppServiceTests
    {
        private AppServiceTestClass _appService;

        public AppServiceTests()
        {
            //var helper = new TestHelpers();
            //var dal = helper.CreateMockDAL();
            _appService = new AppServiceTestClass();
        }

        [Fact]
        public void TestAccountParsing()
        {
            var input = @"
          104.10 EUR  Assets:Active:Cash EUR
                   0  Assets: Bank Accounts:Checking Account
        3,574.38 EUR  Assets:Bank Accounts:gratis account
3675.162105 FSS_IntFix  Assets:Retirement:Superannuation:IntFix";
            var lines = input.Split(new[] { '\n' }).ToList();

            //var result = await _appService.importBalanceSheet(lines);
            var result = _appService.PublicParseAccounts(lines);

            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void TestParsingMultipleBalanceLines()
        {
            var input = @"
        27204.53 BAM
        3,408.36 EUR  Assets:Fixed Assets:Life Insurance";
            var lines = input.Split(new[] { '\n' }).ToList();

            //var result = await _appService.importBalanceSheet(lines);
            var result = _appService.PublicParseAccounts(lines);

            Assert.NotNull(result);
            Assert.Single(result);
        }

    }
}

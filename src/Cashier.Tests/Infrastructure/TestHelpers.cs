using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Cashier.Data;
using Cashier.Services;
using BlazorDexie.Database;
using Cashier.Model;

namespace Cashier.Tests.Infrastructure
{
    internal class TestHelpers
    {
        public Mock<HttpMessageHandler> CreateMockHttpHandler()
        {
            var mockHttp = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttp.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK))
                .Verifiable();

            return mockHttp;
        }

        public Mock<IJSRuntime> CreateMockJSRuntime()
        {
            var mock = new Mock<IJSRuntime>();
            //mock.Protected().Setup<Task>("InvokeAsync");

            return mock;
        }

        public Mock<IDexieDAL> CreateMockDAL()
        {
            var mock = new Mock<IDexieDAL>();
            // var db = new DexieDAL();

            var accounts = new BlazorDexie.Database.Store<Model.Account, string>("name");
            accounts.Add(new Model.Account("Assets:Equity"));

            mock.Setup(db => db.Accounts)
                .Returns(accounts);

            return mock;
        }

        public Mock<ISettingsService> CreateMockSettings()
        {
            var mock = new Mock<ISettingsService>();
            mock.Setup<Task<string>>(x => x.GetDefaultCurrency())
                .ReturnsAsync("EUR");
            mock.Setup<Task<string?>>(x => x.GetRootInvestmentAccount())
                .ReturnsAsync("Assets:Investments");

            return mock;
        }

        public Mock<IAccountService> CreateMockAccountService()
        {
            var mock = new Mock<IAccountService>();
            var invAccounts = new List<Account>()
            {
                new Account("Assets:Investment:Cash") { 
                    CurrentValue = "1",
                    CurrentCurrency = "EUR"
                },
                new Account("Assets:Investment:Equity") {
                    CurrentValue = "600",
                    CurrentCurrency = "EUR",
                    Balances = [
                            new Money(60, "VTS")
                        ]
                },
                new Account("Assets:Investment:Cash") { 
                    CurrentValue = "400",
                    CurrentCurrency = "EUR",
                    Balances = [
                        new Money(40, "BND")
                    ]
                },
            };
            mock.Setup(x => x.LoadInvestmentAccounts(It.IsAny<ISettingsService>(), It.IsAny<IDexieDAL>()))
                .ReturnsAsync(invAccounts);

            var acctSvc = new AccountService();
            mock.Setup(x => x.GetAccountBalance(It.IsAny<Account>(), It.IsAny<string>()))
                .Returns((Account account, string currency) => acctSvc.GetAccountBalance(account, currency) );

            return mock;
        }
    }
}

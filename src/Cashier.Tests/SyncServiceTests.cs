using AngleSharp.Dom;
using AngleSharp.Io;
using Cashier.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cashier.Tests
{
    public class SyncServiceTests
    {
        private SyncService _syncService;

        public SyncServiceTests() {
            var mockHttp = new TestHelpers().CreateMockHttpHandler();
            var httpClient = new HttpClient(mockHttp.Object);
            _syncService = new SyncService(httpClient, "http://localhost");
        }

        /// <summary>
        /// See if the mock works.
        /// </summary>
        [Fact]
        public async Task TestHttpMock()
        {
            var mockHttp = new TestHelpers().CreateMockHttpHandler();
            var httpClient = new HttpClient(mockHttp.Object);
            
            var response = await httpClient.GetAsync("http://localhost");

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}

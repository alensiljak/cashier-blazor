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
            return mock;
        }
    }
}

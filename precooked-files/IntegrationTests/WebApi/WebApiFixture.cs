using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Benday.Presidents.MvcIntegrationTests.AngularUi
{
    [TestClass]
    public class WebApiFixture
    {
        private WebApplicationFactory<Benday.Presidents.AngularUi.Startup> _SystemUnderTest;
        public WebApplicationFactory<Benday.Presidents.AngularUi.Startup> SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new WebApplicationFactory<Benday.Presidents.AngularUi.Startup>();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void GetStuff()
        {
            // var temp = GetInstanceOf<IOptions<MvcJsonOptions>>();

            var temp = GetInstanceOf<IOptions<MvcOptions>>();

            Console.WriteLine();          

        }

        [TestMethod]
        public async Task GetAllPresidents_DefaultMediaTypeReturnedIsJson()
        {
            var client =
                SystemUnderTest.CreateDefaultClient();

            // NOTE: don't specify any special Accept header values
            //       in order to verify default behavior

            var response =
                await client.GetAsync(
                    "/api/president");

            Assert.IsTrue(response.IsSuccessStatusCode,
                "Should be a success status code but got '{0}'.",
                response.StatusCode);

            Assert.IsNotNull(response, "Response should not be null.");

            Assert.AreNotEqual<string>(
                String.Empty, response.Content.ToString(),
                "response.Content should not be empty.");

            Assert.AreEqual<string>("application/json",
                response.Content.Headers.ContentType.MediaType,
                "Media type header was wrong.");

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);

            Assert.AreNotEqual<string>(String.Empty, 
                responseBody, "Response body should not be empty.");
        }

        [TestMethod]
        public async Task GetAllPresidents_Xml()
        {
            var client =
                SystemUnderTest.CreateDefaultClient();

            var expectedContentType = "application/xml";

            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue(
                      expectedContentType));

            var response =
                await client.GetAsync(
                    "/api/president");

            Assert.IsTrue(response.IsSuccessStatusCode,
                "Should be a success status code but got '{0}'.",
                response.StatusCode);

            Assert.IsNotNull(response, "Response should not be null.");

            Assert.AreNotEqual<string>(
                String.Empty, response.Content.ToString(),
                "response.Content should not be empty.");

            Assert.AreEqual<string>(expectedContentType,
                response.Content.Headers.ContentType.MediaType,
                "Media type header was wrong.");

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);

            Assert.AreNotEqual<string>(String.Empty,
                responseBody, "Response body should not be empty.");
        }

        [TestMethod]
        public async Task GetAllPresidents_WhenRequestedContentTypeIsJsonReturnsResultsAsJson()
        {
            var client =
                SystemUnderTest.CreateDefaultClient();

            var expectedContentType = "application/json";

            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue(
                      expectedContentType));

            var response =
                await client.GetAsync(
                    "/api/president");

            Assert.IsTrue(response.IsSuccessStatusCode,
                "Should be a success status code but got '{0}'.",
                response.StatusCode);

            Assert.IsNotNull(response, "Response should not be null.");

            Assert.AreNotEqual<string>(
                String.Empty, response.Content.ToString(),
                "response.Content should not be empty.");

            Assert.AreEqual<string>(expectedContentType,
                response.Content.Headers.ContentType.MediaType,
                "Media type header was wrong.");

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);

            Assert.AreNotEqual<string>(String.Empty,
                responseBody, "Response body should not be empty.");
        }

        private T GetInstanceOf<T>()
        {
            var client = SystemUnderTest.CreateDefaultClient();

            var hostServices = SystemUnderTest.Server.Host.Services;

            var scopeFactory = hostServices.GetService(
                typeof(IServiceScopeFactory)) as IServiceScopeFactory;

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var returnValue =
                    scope.ServiceProvider.GetService<T>();

                Assert.IsNotNull(returnValue, "Could not get an instance of {0}.", typeof(T).Name);

                return returnValue;
            }
        }


    }
}

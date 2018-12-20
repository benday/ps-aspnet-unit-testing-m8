using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Benday.Presidents.MvcIntegrationTests
{
    [TestClass]
    public class PresidentEncodingFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private WebApplicationFactory<Benday.Presidents.WebUi.Startup> _SystemUnderTest;
        public WebApplicationFactory<Benday.Presidents.WebUi.Startup> SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new WebApplicationFactory<Benday.Presidents.WebUi.Startup>();
                }

                return _SystemUnderTest;
            }
        }


        [TestMethod]
        public async Task GetAllPresidents_DefaultMediaTypeReturnedIsJson()
        {
            // arrange
            var client = SystemUnderTest.CreateDefaultClient();
            var expectedContentType = "application/json";

            // act
            var response =
                await client.GetAsync(
                    "/api/president"
                );

            // assert
            Assert.IsTrue(response.IsSuccessStatusCode,
                "Should be a success status code but got '{0}'.",
                response.StatusCode
            );

            Assert.AreEqual<string>(
                expectedContentType,
                response.Content.Headers.ContentType.MediaType,
                "Media type header was wrong."
            );

            var responseBody =
                await response.Content.ReadAsStringAsync();

            Assert.AreNotEqual<string>(String.Empty, responseBody,
                "Response body should not be empty.");
        }

        [TestMethod]
        public async Task GetAllPresidents_Xml()
        {
            // arrange
            var client = SystemUnderTest.CreateDefaultClient();
            var expectedContentType = "application/xml";

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(
                    expectedContentType
                ));

            // act
            var response =
                await client.GetAsync(
                    "/api/president"
                );

            // assert
            Assert.IsTrue(response.IsSuccessStatusCode,
                "Should be a success status code but got '{0}'.",
                response.StatusCode
            );

            Assert.AreEqual<string>(
                expectedContentType,
                response.Content.Headers.ContentType.MediaType,
                "Media type header was wrong."
            );

            var responseBody =
                await response.Content.ReadAsStringAsync();

            Assert.AreNotEqual<string>(String.Empty, responseBody,
                "Response body should not be empty.");
        }

        [TestMethod]
        public async Task GetAllPresidents_WhenRequestedContentTypeIsJsonReturnsResultsAsJson()
        {
            // arrange
            var client = SystemUnderTest.CreateDefaultClient();
            var expectedContentType = "application/json";

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(
                    expectedContentType
                ));

            // act
            var response =
                await client.GetAsync(
                    "/api/president"
                );

            // assert
            Assert.IsTrue(response.IsSuccessStatusCode,
                "Should be a success status code but got '{0}'.",
                response.StatusCode
            );

            Assert.AreEqual<string>(
                expectedContentType,
                response.Content.Headers.ContentType.MediaType,
                "Media type header was wrong."
            );

            var responseBody =
                await response.Content.ReadAsStringAsync();

            Assert.AreNotEqual<string>(String.Empty, responseBody,
                "Response body should not be empty.");
        }

        [TestMethod]
        public async Task GetAllPresidents_TermDoesNotHaveIsDeleted()
        {
            // arrange
            var client = SystemUnderTest.CreateDefaultClient();
            var expectedContentType = "application/json";

            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue(
                    expectedContentType
                ));

            // act
            var response =
                await client.GetAsync(
                    "/api/president"
                );

            // assert
            var responseBody =
                await response.Content.ReadAsStringAsync();

            Assert.AreNotEqual<string>(String.Empty, responseBody,
                "Response body should not be empty.");

            var presidentAsJson = JArray.Parse(responseBody)[0];

            var terms = presidentAsJson["terms"] as JArray;

            Assert.IsNotNull(terms, "Terms was null.");
            Assert.AreEqual<int>(1, terms.Count, "Unexpected term count.");

            var term = terms[0];

            var isDeleted = term["isDeleted"];

            Assert.IsNull(isDeleted, "IsDeleted property should not exist.");
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
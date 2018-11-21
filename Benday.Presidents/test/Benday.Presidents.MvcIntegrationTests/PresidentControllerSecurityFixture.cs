using Benday.Presidents.Api.Services;
using Benday.Presidents.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Benday.Presidents.MvcIntegrationTests
{
    [TestClass]
    public class PresidentControllerSecurityFixture
    {
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

        private async Task PopulateTestData()
        {
            var client = SystemUnderTest.CreateDefaultClient();

            var response = await client.GetAsync("/president/VerifyDatabaseIsPopulated");

            Assert.IsNotNull(response, "Response was null.");

            int statusCodeAsInt = (int)response.StatusCode;

            Assert.IsTrue(statusCodeAsInt < 400,
                "Got an error response from populating test data.");
        }

        private int GetPresidentIdByName(string firstName, string lastName)
        {
            var client = SystemUnderTest.CreateDefaultClient();
            
            var hostServices = SystemUnderTest.Server.Host.Services;

            var scopeFactory = hostServices.GetService(
                typeof(IServiceScopeFactory)) as IServiceScopeFactory;

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var presidentService = 
                    scope.ServiceProvider.GetService(
                        typeof(IPresidentService)) as IPresidentService;

                Assert.IsNotNull(
                    presidentService, 
                    "President service instance was null or unexpected type.");

                var match = presidentService.Search(
                    firstName, lastName).FirstOrDefault();

                Assert.IsNotNull(match, "Could not load president named {0} {1}.", firstName, lastName);

                return match.Id;
            }           
        }

        [TestMethod]
        public async Task Utility_GetPresidentIdByFirstNameLastName()
        {
            var client =
                SystemUnderTest.CreateDefaultClient();

            await PopulateTestData();

            var actual = GetPresidentIdByName("jimmy", "carter");

            Assert.IsTrue(actual > 0, "President id was not a positive integer.");
        }

        [TestMethod]
        public async Task EditPresidentDetailById_RejectsUnauthenticatedUser()
        {
            var client = 
                SystemUnderTest.CreateDefaultClient();

            await PopulateTestData();

            int presidentId = 
                GetPresidentIdByName("George", "Washington");

            Assert.AreNotEqual<int>(0, presidentId, "President Id was an unexpected value");

            var response = 
                await client.GetAsync(
                    String.Format("/president/edit/{0}", presidentId));
                                
            Assert.IsNotNull(response, "Response should not be null.");

            // should redirect to login page
            Assert.IsFalse(response.IsSuccessStatusCode, "Should not be an HTTP success code.");
        }

        /*
         * 
         * THIS DOESN'T WORK...CAN'T FIGURE OUT HOW TO AUTHENTICATE
         * WITHOUT SCRIPTING OUT THE FULL AUTH PROCESS
         * 
        [TestMethod]
        public async Task EditPresidentDetailById_AcceptsAuthenticatedAdminUser()
        {
            var client =
                SystemUnderTest.CreateDefaultClient();

            await PopulateTestData();

            int presidentId =
                GetPresidentIdByName("George", "Washington");

            Assert.AreNotEqual<int>(0, presidentId, "President Id was an unexpected value");

            var response =
                await client.GetAsync(
                    String.Format("/president/edit/{0}", presidentId));

            Assert.IsNotNull(response, "Response should not be null.");

            // should redirect to login page
            Assert.IsFalse(response.IsSuccessStatusCode, "Should not be an HTTP success code.");
        }
        */
    }
}

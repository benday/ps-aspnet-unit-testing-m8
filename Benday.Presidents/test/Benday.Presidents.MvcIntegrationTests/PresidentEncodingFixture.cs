using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        /*
        [TestMethod]
        public async Task GetAllPresidents_DefaultMediaTypeReturnedIsJson()
        {
        }
        */

        /*
        [TestMethod]
        public async Task GetAllPresidents_Xml()
        {

        }
        */

        /*
        [TestMethod]
        public async Task GetAllPresidents_WhenRequestedContentTypeIsJsonReturnsResultsAsJson()
        {
        }
        */

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
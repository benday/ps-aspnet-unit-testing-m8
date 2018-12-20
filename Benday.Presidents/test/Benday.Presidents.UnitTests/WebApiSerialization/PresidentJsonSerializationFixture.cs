using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Benday.Presidents.UnitTests.WebApiSerialization
{
    [TestClass]
    public class PresidentJsonSerializationFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private JsonSerializer _SystemUnderTest;
        public JsonSerializer SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    var contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            OverrideSpecifiedNames = false
                        }
                    };

                    _SystemUnderTest = new JsonSerializer();
                    _SystemUnderTest.ContractResolver = contractResolver;
                }

                return _SystemUnderTest;
            }
        }

        private void InitializeForPascalCase()
        {
            _SystemUnderTest = new JsonSerializer();
        }

        private void InitializeForCamelCase()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    OverrideSpecifiedNames = false
                }
            };

            _SystemUnderTest = new JsonSerializer();
            _SystemUnderTest.ContractResolver = contractResolver;
        }

        /*
        [TestMethod]
        public void TermIsDeletedIsNotSerializedIntoJson_CamelCase()
        {
        }
        */

        /*
        [TestMethod]
        public void TermIsDeletedIsNotSerializedIntoJson_PascalCase()
        {
        }
        */
    }
}
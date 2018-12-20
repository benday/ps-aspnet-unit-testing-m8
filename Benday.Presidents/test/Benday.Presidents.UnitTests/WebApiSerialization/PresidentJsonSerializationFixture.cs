using System;
using System.IO;
using System.Text;
using Benday.Presidents.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                Assert.IsNotNull(_SystemUnderTest,
                    "Initialize hasn't been called.");

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

        [TestMethod]
        public void TermIsDeletedIsNotSerializedIntoJson_CamelCase()
        {
            // arrange
            InitializeForCamelCase();

            var serializeThis =
                UnitTestUtility.GetGroverClevelandAsPresident();

            // act
            var json = SerializeToJsonString(serializeThis);

            // assert
            var presidentAsJson = JObject.Parse(json);

            var terms = presidentAsJson["terms"] as JArray;

            Assert.IsNotNull(terms, "Terms was null.");
            Assert.AreEqual<int>(2, terms.Count, "Unexpected term count.");

            var term = terms[0];

            var isDeleted = term["isDeleted"];

            Assert.IsNull(isDeleted, "IsDeleted property should not exist.");
        }

        [TestMethod]
        public void TermIsDeletedIsNotSerializedIntoJson_PascalCase()
        {
            // arrange
            InitializeForPascalCase();

            var serializeThis =
                UnitTestUtility.GetGroverClevelandAsPresident();

            // act
            var json = SerializeToJsonString(serializeThis);

            // assert
            var presidentAsJson = JObject.Parse(json);

            var terms = presidentAsJson["Terms"] as JArray;

            Assert.IsNotNull(terms, "Terms was null.");
            Assert.AreEqual<int>(2, terms.Count, "Unexpected term count.");

            var term = terms[0];

            var isDeleted = term["IsDeleted"];

            Assert.IsNull(isDeleted, "IsDeleted property should not exist.");
        }

        private string SerializeToJsonString(President serializeThis)
        {
            var builder = new StringBuilder();

            SystemUnderTest.Serialize(
                new StringWriter(builder),
                serializeThis
            );

            return builder.ToString();
        }
    }
}
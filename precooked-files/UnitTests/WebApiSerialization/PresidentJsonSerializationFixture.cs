using Benday.Presidents.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Benday.Presidents.UnitTests.WebApiSerialization
{
    [TestClass]
    public class PresidentJsonSerializationFixture
    {
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

        private string SerializeToJsonString(President value)
        {
            var builder = new StringBuilder();

            SystemUnderTest.Serialize(
                new StringWriter(builder), value);

            return builder.ToString();
        }

        [TestMethod]
        public void TermIsDeletedIsNotSerializedIntoJson()
        {
            var fromValue =
                UnitTestUtility.GetGroverClevelandAsPresident();

            var json = SerializeToJsonString(fromValue);

            var presidentAsJson = JObject.Parse(json);

            var terms = presidentAsJson["Terms"] as JArray;

            Assert.IsNotNull(terms, "Terms was null or not an array.");
            Assert.AreEqual<int>(2, terms.Count, "Unexpected term count");

            var term = terms[0];

            var isDeleted = term["IsDeleted"];

            Assert.IsNull(isDeleted, "IsDeleted value should not exist.");
        }

        [TestMethod]
        public void TermIsDeletedIsNotSerializedIntoJson_v2()
        {
            var fromValue =
                UnitTestUtility.GetGroverClevelandAsPresident();

            var json = SerializeToJsonString(fromValue);

            var presidentAsJson = JObject.Parse(json);

            var terms = presidentAsJson["terms"] as JArray;

            Assert.IsNotNull(terms, "Terms was null or not an array.");
            Assert.AreEqual<int>(2, terms.Count, "Unexpected term count");

            var term = terms[0];

            var isDeleted = term["isDeleted"];

            Assert.IsNull(isDeleted, "IsDeleted value should not exist.");
        }


    }
}

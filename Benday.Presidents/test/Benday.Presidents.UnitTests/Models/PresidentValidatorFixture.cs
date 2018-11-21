using System;
using Benday.Presidents.Api;
using Benday.Presidents.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Presidents.UnitTests.Models
{
    [TestClass]
    public class PresidentValidatorFixture
    {
        private DefaultValidatorStrategy<President> _SystemUnderTest;
        public DefaultValidatorStrategy<President> SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DefaultValidatorStrategy<President>();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void ValidatePresident_DefaultConstructor_ReturnsFalse()
        {
            var president = new President();

            var result = SystemUnderTest.IsValid(president);

            Assert.IsFalse(result, "Should not pass validation.");
        }

        [TestMethod]
        public void ValidatePresident_PopulatedPresident_ReturnsTrue()
        {
            var president = UnitTestUtility.GetThomasJeffersonAsPresident();

            var result = SystemUnderTest.IsValid(president);

            Assert.IsTrue(result, "Should be valid.");
        }

        [TestMethod]
        public void ValidatePresident_NonDeadPresident_ReturnsTrue()
        {
            var president = UnitTestUtility.GetThomasJeffersonAsPresident();

            president.DeathDate = default(DateTime);

            var result = SystemUnderTest.IsValid(president);

            Assert.IsTrue(result, "Should be valid.");
        }

        [TestMethod]
        public void ValidatePresident_BirthDateAfterDeathDate_ReturnsFalse()
        {
            var president = UnitTestUtility.GetThomasJeffersonAsPresident();

            president.BirthDate = president.DeathDate.AddYears(1);

            var result = SystemUnderTest.IsValid(president);

            Assert.IsFalse(result, "Should be invalid.");
        }


        [TestMethod]
        public void ValidatePresident_DeathDateBeforeBirthDate_ReturnsFalse()
        {
            var president = UnitTestUtility.GetThomasJeffersonAsPresident();

            president.DeathDate = president.BirthDate.AddYears(-1);

            var result = SystemUnderTest.IsValid(president);

            Assert.IsFalse(result, "Should be invalid.");
        }

        [TestMethod]
        public void ValidatePresident_PresidentWithZeroTerms_ReturnsFalse()
        {
            var president = UnitTestUtility.GetGroverClevelandAsPresident();

            president.Terms.Clear();

            var result = SystemUnderTest.IsValid(president);

            Assert.IsFalse(result, "Should not be valid.");
        }

        [TestMethod]
        public void ValidatePresident_PresidentWithThreeTerms_ReturnsFalse()
        {
            var president = UnitTestUtility.GetGroverClevelandAsPresident();

            president.AddTerm(PresidentsConstants.President,
                DateTime.Now, DateTime.Now.AddYears(4), 46);

            var result = SystemUnderTest.IsValid(president);

            Assert.IsFalse(result, "Should not be valid.");
        }

    }
}
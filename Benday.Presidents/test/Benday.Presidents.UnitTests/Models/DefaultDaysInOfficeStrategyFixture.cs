using Benday.Presidents.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Benday.Presidents.UnitTests.Models
{
    [TestClass]
    public class DefaultDaysInOfficeStrategyFixture
    {
        private IDaysInOfficeStrategy _SystemUnderTest;
        public IDaysInOfficeStrategy SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DefaultDaysInOfficeStrategy();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void DaysInOffice_ZeroTermsInOffice()
        {
            var terms = new List<Term>();

            int actual = SystemUnderTest.GetDaysInOffice(terms);

            Assert.AreEqual<int>(0, actual, "Days in office was wrong.");
        }

        [TestMethod]
        public void DaysInOffice_SingleTermOfOneDay()
        {
            var terms = new List<Term>();

            var term = new Term()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            terms.Add(term);

            int actual = SystemUnderTest.GetDaysInOffice(terms);

            Assert.AreEqual<int>(1, actual, "Days in office was wrong.");
        }

        [TestMethod]
        public void DaysInOffice_SingleTermOfFourYears()
        {
            var terms = new List<Term>();

            var term = new Term()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddYears(4)
            };

            terms.Add(term);

            int actual = SystemUnderTest.GetDaysInOffice(terms);

            Assert.AreEqual<int>(DurationOfFourYearTerm, actual, "Days in office was wrong.");
        }

        [TestMethod]
        public void DaysInOffice_TwoTermOfOneDay()
        {
            var terms = new List<Term>();

            var term1 = new Term()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            var term2 = new Term()
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            terms.Add(term1);
            terms.Add(term2);

            int actual = SystemUnderTest.GetDaysInOffice(terms);

            Assert.AreEqual<int>(2, actual, "Days in office was wrong.");
        }

        private const int DurationOfFourYearTerm = (365 * 4) + 1;
    }
}

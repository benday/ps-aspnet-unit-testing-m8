using Benday.Presidents.WebUi;
using Benday.Presidents.WebUi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Claims;

namespace Benday.Presidents.UnitTests.Security
{
    [TestClass]
    public class DefaultUserAuthorizationStrategyFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _PrincipalProvider = null;
        }

        private DefaultUserAuthorizationStrategy _SystemUnderTest;
        public DefaultUserAuthorizationStrategy SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest =
                        new DefaultUserAuthorizationStrategy(
                            PrincipalProvider);
                }

                return _SystemUnderTest;
            }
        }

        private MockUserClaimsPrincipalProvider _PrincipalProvider;
        public MockUserClaimsPrincipalProvider PrincipalProvider
        {
            get
            {
                if (_PrincipalProvider == null)
                {
                    _PrincipalProvider = new MockUserClaimsPrincipalProvider();
                }

                return _PrincipalProvider;
            }
        }

        [TestMethod]
        public void IsAuthorizedForSearch_ReturnsFalseForNoClaims()
        {
            Assert.IsFalse(SystemUnderTest.IsAuthorizedForSearch,
                "Should not be authorized for search.");
        }

        [TestMethod]
        public void IsAuthorizedForSearch_ReturnsTrueForAdministrator()
        {
            PrincipalProvider.AddClaim(
                ClaimTypes.Role,
                SecurityConstants.RoleName_Admin);

            Assert.IsTrue(SystemUnderTest.IsAuthorizedForSearch,
                "Should be authorized for search.");
        }

        [TestMethod]
        public void IsAuthorizedForSearch_ReturnsTrueForBasicSubscription()
        {
            PrincipalProvider.AddClaim(
                SecurityConstants.Claim_SubscriptionType, 
                SecurityConstants.SubscriptionType_Basic);

            Assert.IsTrue(SystemUnderTest.IsAuthorizedForSearch,
                "Should be authorized for search.");
        }

        [TestMethod]
        public void IsAuthorizedForSearch_ReturnsTrueForUltimateSubscription()
        {
            PrincipalProvider.AddClaim(
                SecurityConstants.Claim_SubscriptionType, 
                SecurityConstants.SubscriptionType_Ultimate);

            Assert.IsTrue(SystemUnderTest.IsAuthorizedForSearch,
                "Should be authorized for search.");
        }


        [TestMethod]
        public void IsAuthorizedForImages_ReturnsFalseForNoClaims()
        {
            Assert.IsFalse(SystemUnderTest.IsAuthorizedForImages,
                "Should not be authorized for search.");
        }

        [TestMethod]
        public void IsAuthorizedForImages_ReturnsTrueForAdministrator()
        {
            PrincipalProvider.AddClaim(
                ClaimTypes.Role,
                SecurityConstants.RoleName_Admin);

            Assert.IsTrue(SystemUnderTest.IsAuthorizedForImages,
                "Should be authorized for search.");
        }

        [TestMethod]
        public void IsAuthorizedForImages_ReturnsFalseForBasicSubscription()
        {
            PrincipalProvider.AddClaim(
                SecurityConstants.Claim_SubscriptionType,
                SecurityConstants.SubscriptionType_Basic);

            Assert.IsFalse(SystemUnderTest.IsAuthorizedForImages,
                "Should not be authorized for search.");
        }

        [TestMethod]
        public void IsAuthorizedForImages_ReturnsTrueForUltimateSubscription()
        {

            PrincipalProvider.AddClaim(
                SecurityConstants.Claim_SubscriptionType,
                SecurityConstants.SubscriptionType_Ultimate);

            Assert.IsTrue(SystemUnderTest.IsAuthorizedForImages,
                "Should be authorized for search.");
        }

    }
}
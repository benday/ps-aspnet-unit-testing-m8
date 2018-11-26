using System;
using Benday.Presidents.Api.Models;
using Benday.Presidents.WebUi;
using Benday.Presidents.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Presidents.UnitTests.Security
{
    [TestClass]
    public class PresidentControllerSecurityFixture
    {
        private Type SystemUnderTest
        {
            get
            {
                return typeof(PresidentController);
            }
        }

        [TestMethod]
        public void EditMethodRequiresAdministratorRole_Int32()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributeRolesOnMethod(
                SecurityConstants.RoleName_Admin, SystemUnderTest, "Edit", typeof(int)
            );
        }

        [TestMethod]
        public void EditMethodRequiresAdministratorRole_President()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributeRolesOnMethod(
                SecurityConstants.RoleName_Admin, SystemUnderTest, "Edit", typeof(President)
            );
        }

    }
}
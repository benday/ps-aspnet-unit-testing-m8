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

/*
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
*/

        [TestMethod]
        public void EditMethodRequiresEditPresidentPolicy_Int32()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributePolicyOnMethod(
                SecurityConstants.PolicyName_EditPresident, 
                SystemUnderTest, "Edit", typeof(int)
            );
        }

        [TestMethod]
        public void EditMethodRequiresEditPresidentPolicy_President()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributePolicyOnMethod(
                SecurityConstants.PolicyName_EditPresident, 
                SystemUnderTest, "Edit", typeof(President)
            );
        }

/*
        [TestMethod]
        public void ControllerRequiresAdministratorRole()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributeRolesOnClass(
                SecurityConstants.RoleName_Admin, SystemUnderTest
            );
        }
*/

        [TestMethod]
        public void IndexMethodAllowsAnonymous()
        {
            SecurityAttributeUtility.AssertAllowAnonymousAttributeOnMethod(
                SystemUnderTest, "Index");
        }

        [TestMethod]
        public void DetailsMethodAllowsAnonymous()
        {
            SecurityAttributeUtility.AssertAllowAnonymousAttributeOnMethod(
                   SystemUnderTest, "Details", typeof(int));
        }



    }
}
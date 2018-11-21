using Benday.Presidents.Api.Models;
using Benday.Presidents.WebUi;
using Benday.Presidents.WebUI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.Presidents.UnitTests.Presentation
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
        public void ControllerRequiresAdministratorsRole()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributeRolesOnClass(
                SecurityConstants.RoleName_Admin, SystemUnderTest);
        }
        */

        [TestMethod]
        public void IndexMethodAllowsAnonymous()
        {
            SecurityAttributeUtility.AssertAllowAnonymousAttributeOnMethod(
                SystemUnderTest, "Index");
        }

        /*
        [TestMethod]
        public void EditMethodRequiresAdministratorsRole_Int32()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributeRolesOnMethod(
                SecurityConstants.RoleName_Admin, SystemUnderTest, "Edit", typeof(int));
        }
        
        [TestMethod]
        public void EditMethodRequiresAdministratorsRole_President()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributeRolesOnMethod(
                SecurityConstants.RoleName_Admin, SystemUnderTest, "Edit", typeof(President));
        }
        */

        [TestMethod]
        public void EditMethodRequiresEditorPolicy_Int32()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributePolicyOnMethod(
                SecurityConstants.PolicyName_EditPresident, SystemUnderTest, "Edit", typeof(int));
        }

        [TestMethod]
        public void EditMethodRequiresEditorPolicy_President()
        {
            SecurityAttributeUtility.AssertAuthorizeAttributePolicyOnMethod(
                SecurityConstants.PolicyName_EditPresident, SystemUnderTest, "Edit", typeof(President));
        }
    }
}

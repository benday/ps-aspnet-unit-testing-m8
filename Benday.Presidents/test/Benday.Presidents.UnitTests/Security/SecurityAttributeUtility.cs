using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Benday.Presidents.UnitTests.Security
{
    public static class SecurityAttributeUtility
    {
        public static void AssertAuthorizeAttributeRolesOnMethod(
            string expectedRoles,
            Type containingDataType, string methodName,
            params Type[] methodParameters
            )
        {
            var attribute =
                GetAttributeFromMethod<AuthorizeAttribute>(
                    containingDataType, methodName, methodParameters);

            Assert.IsNotNull(attribute,
                "Method '{0}' on class '{1}' does not have an authorize attribute.",
                methodName, containingDataType.FullName);

            Assert.AreEqual<string>(expectedRoles,
                attribute.Roles,
                "Roles contains the wrong value.");
        }

        public static void AssertAuthorizeAttributePolicyOnMethod(
            string expectedPolicy,
            Type containingDataType, string methodName,
            params Type[] methodParameters
            )
        {
            var attribute =
                GetAttributeFromMethod<AuthorizeAttribute>(
                    containingDataType, methodName, methodParameters);

            Assert.IsNotNull(attribute,
                "Method '{0}' on class '{1}' does not have an authorize attribute.",
                methodName, containingDataType.FullName);

            Assert.AreEqual<string>(expectedPolicy,
                attribute.Policy,
                "Policy contains the wrong value.");
        }

        public static void AssertAllowAnonymousAttributeOnMethod(
            Type containingDataType,
            string methodName,
            params Type[] methodParameters
    )
        {
            var attribute =
                GetAttributeFromMethod<AllowAnonymousAttribute>(
                    containingDataType, methodName, methodParameters);

            Assert.IsNotNull(attribute,
                "Method '{0}' on class '{1}' does not have an allow anonymous attribute.",
                methodName, containingDataType.FullName);
        }

        public static void AssertAuthorizeAttributeRolesOnClass(
            string expectedRoles,
            Type containingDataType
            )
        {
            var attribute =
                GetAttributeFromClass<AuthorizeAttribute>(
                    containingDataType);

            Assert.IsNotNull(attribute,
                "Class '{0}' does not have an authorize attribute.",
                containingDataType.FullName);

            Assert.AreEqual<string>(expectedRoles,
                attribute.Roles,
                "Roles contains the wrong value.");
        }

        private static T GetAttributeFromMethod<T>(
            Type containingDataType,
            string methodName,
            params Type[] methodArgs) where T : Attribute
        {
            var method = containingDataType.GetMethod(
                methodName, methodArgs);

            Assert.IsNotNull(method,
                "Could not locate a method named '{0}' with matching parameters.",
                methodName);

            var attribute = method.GetCustomAttributes(
                typeof(T), true).FirstOrDefault();

            return attribute as T;
        }

        private static T GetAttributeFromClass<T>(
            Type containingDataType)
            where T : Attribute
        {

            var attribute = containingDataType.GetCustomAttributes(
                typeof(T), true).FirstOrDefault();

            return attribute as T;
        }
    }
}

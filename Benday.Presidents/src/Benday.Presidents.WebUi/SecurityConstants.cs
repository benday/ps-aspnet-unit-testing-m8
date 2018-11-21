using System;
using System.Linq;

namespace Benday.Presidents.WebUi
{
    public static class SecurityConstants
    {
        public const string RoleName_Admin = "Presidents.Admin";
        public const string RoleName_User = "Presidents.User";

        public const string PermissionName_View = "President.View";
        public const string PermissionName_Edit = "President.Edit";

        public const string Username_Admin = "admin@test.org";
        public const string Username_User1 = "user1@test.org";
        public const string Username_User2 = "user2@test.org";
        public const string Username_Subscriber1 = "subscriber1@test.org";
        public const string Username_Subscriber2 = "subscriber2@test.org";
        
        public const string DefaultPassword = "password";

        public const string PolicyName_EditPresident = "EditPresidentPolicy";

        public const string SubscriptionType_Basic = "Basic";
        public const string SubscriptionType_Ultimate = "Ultimate";

        public const string Claim_SubscriptionType = "SubscriptionType";
    }
}

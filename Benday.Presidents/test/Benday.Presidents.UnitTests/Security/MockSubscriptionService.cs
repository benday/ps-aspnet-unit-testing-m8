using System;
using Benday.Presidents.Api.Services;

namespace Benday.Presidents.UnitTests.Security
{
    public class MockSubscriptionService : ISubscriptionService
    {

        public MockSubscriptionService()
        {
            SubscriptionTypeReturnValue = null;
        }

        public void AddSubscription(string username, string subscriptionType)
        {
            throw new NotImplementedException();
        }

        public string SubscriptionTypeReturnValue { get; set; }

        public string GetSubscriptionType(string username)
        {
            return SubscriptionTypeReturnValue;
        }

        public void RemoveSubscription(string username)
        {
            throw new NotImplementedException();
        }
    }
}
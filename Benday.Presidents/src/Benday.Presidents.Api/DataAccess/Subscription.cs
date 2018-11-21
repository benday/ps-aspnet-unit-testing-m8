using Benday.Presidents.Api.Models;
using System;
using System.Linq;

namespace Benday.Presidents.Api.DataAccess
{
    public class Subscription : Int32Identity
    {
        public string Username { get; set; }
        public string SubscriptionLevel { get; set; }
    }
}

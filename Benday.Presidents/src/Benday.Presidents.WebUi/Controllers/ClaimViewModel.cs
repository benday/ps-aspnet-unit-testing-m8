using System;
using System.Linq;

namespace Benday.Presidents.WebUI.Controllers
{
    public class ClaimViewModel
    {
        public ClaimViewModel(string id, string username, string permission, string presidentName)
        {
            Id = id;
            Username = username;
            Permission = permission;
            PresidentName = presidentName;
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Permission { get; set; }
        public string PresidentName { get; set; }
    }
}
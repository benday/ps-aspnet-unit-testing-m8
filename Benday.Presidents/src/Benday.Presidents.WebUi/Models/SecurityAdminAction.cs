using Benday.Presidents.Api.Models;
using Benday.Presidents.WebUI.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.WebUI.Models
{
    public class SecurityAdminAction
    {
        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Presidents { get; set; }

        public List<SelectListItem> Permissions { get; set; }

        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string PresidentId { get; set; }
        public string Permission { get; set; }
        public List<ClaimViewModel> Claims { get; set; }
        public string SubscriptionType { get; set; }
        public List<SelectListItem> SubscriptionTypes { get; set; }

    }
}

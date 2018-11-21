using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Models;
using Benday.Presidents.Api.Services;
using Benday.Presidents.WebUi;
using Benday.Presidents.WebUi.Data;
using Benday.Presidents.WebUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Benday.Presidents.WebUI.Controllers
{
    public class SecurityAdminController : Controller
    {
        private UserManager<IdentityUser> _UserManager;
        private IPresidentService _PresidentService;
        private RoleManager<IdentityRole> _RoleManager;
        private ApplicationDbContext _Context;
        private ISubscriptionService _SubscriptionService;

        public SecurityAdminController(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, IPresidentService presidentService,
            ApplicationDbContext dbContext, ISubscriptionService subscriptionService)
        {
            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager), $"{nameof(roleManager)} is null.");
            }

            if (presidentService == null)
            {
                throw new ArgumentNullException("presidentService", "Argument cannot be null.");
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager), $"{nameof(userManager)} is null.");
            }

            _UserManager = userManager;
            _PresidentService = presidentService;
            _RoleManager = roleManager;
            _Context = dbContext;
            _SubscriptionService = subscriptionService;
        }

        public IActionResult Index()
        {
            var model = new SecurityAdminAction();

            model.Presidents =
                AdaptToSelectListItems(_PresidentService.GetPresidents());

            model.Roles = AdaptToSelectListItems(_RoleManager.Roles.ToList());

            model.Users = AdaptToSelectListItems(_UserManager.Users.OrderBy(x => x.UserName).ToList());

            model.Permissions = new List<SelectListItem>();

            model.Permissions.Add(new SelectListItem()
            {
                Value = SecurityConstants.PermissionName_Edit,
                Text = SecurityConstants.PermissionName_Edit
            });

            model.Permissions.Add(new SelectListItem()
            {
                Value = SecurityConstants.PermissionName_View,
                Text = SecurityConstants.PermissionName_View
            });

            model.Claims = GetAllClaims();

            model.SubscriptionTypes = new List<SelectListItem>();

            model.SubscriptionTypes.Add(new SelectListItem()
            {
                Value = SecurityConstants.SubscriptionType_Basic,
                Text = SecurityConstants.SubscriptionType_Basic
            });

            model.SubscriptionTypes.Add(new SelectListItem()
            {
                Value = SecurityConstants.SubscriptionType_Ultimate,
                Text = SecurityConstants.SubscriptionType_Ultimate
            });

            return View(model);
        }
        private List<ClaimViewModel> GetAllClaims()
        {
            var users = (from temp in _Context.Users
                         select temp);

            Dictionary<string, string> usersDictionary = new Dictionary<string, string>();

            foreach(var usr in users)
            {
                usersDictionary.Add(usr.Id, usr.UserName);
            }

            Dictionary<string, string> presidentsDictionary = new Dictionary<string, string>();

            foreach (var president in _PresidentService.GetPresidents())
            {
                presidentsDictionary.Add(president.Id.ToString(),
                    String.Format("{0} {1}", president.FirstName, president.LastName)
                    );
            }

            var claims = (
                from temp in _Context.UserClaims
                select temp
                );

            List<ClaimViewModel> returnValues = new List<ClaimViewModel>();

            foreach (var claim in claims)
            {
                var temp = new ClaimViewModel(claim.Id.ToString(), usersDictionary[claim.UserId],
                    claim.ClaimType, presidentsDictionary[claim.ClaimValue]);

                returnValues.Add(temp);
            }

            return returnValues;
        }

        [HttpPost]
        public async Task<ActionResult> AddToGroup(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.RoleId != null &&
                securityAdminAction.UserId != null)
            {
                var role = await _RoleManager.FindByIdAsync(securityAdminAction.RoleId);
                var user = await _UserManager.FindByIdAsync(securityAdminAction.UserId);

                await _UserManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromGroup(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.RoleId != null &&
                securityAdminAction.UserId != null)
            {
                var role = await _RoleManager.FindByIdAsync(securityAdminAction.RoleId);
                var user = await _UserManager.FindByIdAsync(securityAdminAction.UserId);

                await _UserManager.RemoveFromRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddSubscription(
            SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.UserId != null)
            {
                _SubscriptionService.AddSubscription(
                    GetSelectedUsername(securityAdminAction), 
                    securityAdminAction.SubscriptionType);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveSubscription(
            SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.UserId != null)
            {
                _SubscriptionService.RemoveSubscription(
                    GetSelectedUsername(securityAdminAction));
            }

            return RedirectToAction("Index");
        }

        private string GetSelectedUsername(SecurityAdminAction securityAdminAction)
        {
            var allUsers = _UserManager.Users.ToList();

            var username = (from temp in allUsers
                            where temp.Id == securityAdminAction.UserId
                            select temp).FirstOrDefault();

            if (username != null)
            {
                return username.UserName;
            }
            else
            {
                return securityAdminAction.UserId;
            }
        }

        private List<SelectListItem> AdaptToSelectListItems(List<IdentityRole> fromValues)
        {
            List<SelectListItem> toValues = new List<SelectListItem>();

            foreach (var fromValue in fromValues)
            {
                toValues.Add(new SelectListItem()
                {
                    Value = fromValue.Id,
                    Text = fromValue.Name
                });
            }

            return toValues;
        }

        private List<SelectListItem> AdaptToSelectListItems(IList<President> fromValues)
        {
            List<SelectListItem> toValues = new List<SelectListItem>();

            foreach (var fromValue in fromValues.OrderBy(x => x.LastName))
            {
                toValues.Add(new SelectListItem()
                {
                    Value = fromValue.Id.ToString(),
                    Text = String.Format("{0}, {1}", fromValue.LastName, fromValue.FirstName)
                });
            }

            return toValues;
        }

        private List<SelectListItem> AdaptToSelectListItems(List<IdentityUser> fromValues)
        {
            List<SelectListItem> toValues = new List<SelectListItem>();

            foreach (var fromValue in fromValues)
            {
                toValues.Add(new SelectListItem()
                {
                    Value = fromValue.Id,
                    Text = fromValue.UserName
                });
            }

            return toValues;
        }

        [HttpPost]
        public async Task<ActionResult> AddPermission(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.PresidentId != null &&
                securityAdminAction.UserId != null)
            {
                var user = await _UserManager.FindByIdAsync(securityAdminAction.UserId);

                var claim = new Claim(
                    securityAdminAction.Permission,
                    securityAdminAction.PresidentId);

                var claims = await _UserManager.GetClaimsAsync(user);

                /*
                var removeThisClaim = (
                    from temp in claims
                    where temp.Type == SecurityConstants.Claim_LevelOfAwesomeness
                    select temp
                    ).FirstOrDefault();

                if (removeThisClaim != null)
                {
                    await _UserManager.RemoveClaimAsync(user, removeThisClaim);
                }
                */

                await _UserManager.AddClaimAsync(user, claim);
            }

            return RedirectToAction("Index");
        }

        public ActionResult RemovePermission(int id)
        {
            var removeThis = (from temp in _Context.UserClaims
                              where temp.Id == id
                              select temp).FirstOrDefault();

            if (removeThis != null)
            {
                _Context.UserClaims.Remove(removeThis);
                _Context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> RemovePermission(SecurityAdminAction securityAdminAction)
        {
            if (securityAdminAction != null &&
                securityAdminAction.PresidentId != null &&
                securityAdminAction.UserId != null)
            {
                var user = await _UserManager.FindByIdAsync(securityAdminAction.UserId);

                var claims = await _UserManager.GetClaimsAsync(user);

                var removeThisClaim = (
                    from temp in claims
                    where temp.Type == securityAdminAction.Permission
                    select temp
                    ).FirstOrDefault();

                if (removeThisClaim != null)
                {
                    await _UserManager.RemoveClaimAsync(user, removeThisClaim);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
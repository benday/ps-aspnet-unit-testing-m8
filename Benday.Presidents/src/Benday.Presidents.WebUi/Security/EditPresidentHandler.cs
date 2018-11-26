using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Benday.Presidents.WebUi.Security
{
    public class EditPresidentHandler :
        AuthorizationHandler<EditPresidentRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            EditPresidentRequirement requirement)
        {
            if (context.Resource is 
                Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext)
            {
                if (mvcContext.RouteData.Values.ContainsKey("id") == false)
                {
                    context.Fail();
                }
                else
                {
                    int id = Convert.ToInt32(mvcContext.RouteData.Values["id"]);

                    var utility = new SecurityUtility(context.User.Identity, context.User);

                    if (utility.IsAuthorized(SecurityConstants.PermissionName_Edit, id) == true)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
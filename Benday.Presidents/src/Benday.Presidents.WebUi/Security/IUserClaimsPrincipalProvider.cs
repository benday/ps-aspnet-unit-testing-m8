using System.Security.Claims;

namespace Benday.Presidents.WebUi.Security
{
    public interface IUserClaimsPrincipalProvider
    {
        ClaimsPrincipal GetUser();
    }
}
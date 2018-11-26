namespace Benday.Presidents.WebUi.Security
{
    public interface IUserAuthorizationStrategy
    {
        bool IsAuthorizedForSearch { get; }
        bool IsAuthorizedForImages { get; }
    }
}
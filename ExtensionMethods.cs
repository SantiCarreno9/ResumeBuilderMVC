using System.Security.Claims;

namespace ResumeBuilder
{
    public static class ExtensionMethods
    {
        public static string GetId(this ClaimsPrincipal User)
        {
            return User.Claims.First().Value;
        }

        public static int ToInt(this string value)
        {
            return int.Parse(value);
        }
    }
}

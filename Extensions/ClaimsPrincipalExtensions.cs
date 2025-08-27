using System.Security.Claims;

namespace KOAHome.Extensions
{
  public static class ClaimsPrincipalExtensions
  {
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("UserID");
        return claim != null ? int.Parse(claim.Value) : 0;
    }
    public static int GetSiteId(this ClaimsPrincipal user)
    {
      var claim = user.FindFirst("SiteId");
      return claim != null ? int.Parse(claim.Value) : 0;
    }
  }
}

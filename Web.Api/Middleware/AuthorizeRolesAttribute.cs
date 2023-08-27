using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Web.Http.Controllers;

namespace WebApi.Middleware
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }

        public void OnAuthorization(HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal;
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Unauthorize(actionContext);
                return;
            }

            var values = actionContext.RequestContext.RouteData.Values;
            object pathObj;
            if (!values.TryGetValue("path", out pathObj))
            {
                return;
            }

            //var path = (pathObj as string) ?? string.Empty;
            //var defaultProvider = ReportProviderManager.Current.Get("") as FlexReportProvider;
            //if (defaultProvider == null)
            //{
            //    return;
            //}

            var roles = GetRoles();
            if (!roles.Any())
            {
                return;
            }

            if (!roles.Any(r => principal.IsInRole(r)))
            {
                Unauthorize(actionContext);
            }
        }

        private static void Unauthorize(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        private List<string> GetRoles()
        {
            return new List<string>() { "admin", "user" };
        }
    }
}

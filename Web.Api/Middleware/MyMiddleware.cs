using System.Globalization;

namespace WebApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var cultureQuery = httpContext.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                //var culture = new CultureInfo(cultureQuery);

                //CultureInfo.CurrentCulture = culture;
                //CultureInfo.CurrentUICulture = culture;

                //httpContext.Response.Redirect("/login");

                //httpContext.Response.StatusCode = 401;
                //return;
            }
            else
            {
                await _next(httpContext); // calling next middleware
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TelefonOzellikleri.Data;

namespace TelefonOzellikleri.Routing;

public class PageSlugConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (httpContext == null) return false;
        if (!values.TryGetValue(routeKey, out var slugObj) || slugObj is not string slug)
            return false;

        var context = httpContext.RequestServices.GetRequiredService<TelefonOzellikleriDbContext>();
        return context.Pages.Any(p => p.Slug == slug);
    }
}

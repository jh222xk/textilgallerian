using System.Web.Mvc;
using System.Web.Routing;

namespace AdminView
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Authorization", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute("Logout", "{controller}/{action}/{id}",
                new {controller = "Authorization", action = "Logout", id = UrlParameter.Optional}
                );
        }
    }
}
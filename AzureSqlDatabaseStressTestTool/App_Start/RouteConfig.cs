using System.Web.Mvc;
using System.Web.Routing;

namespace AzureSqlDatabaseStressTestTool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Testing", action = "Index", id = UrlParameter.Optional }
            );

            routes.Add(new Route("Testing/Index", new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(new { controller = "Testing", action = "Index" }),
                DataTokens = new RouteValueDictionary(new { scheme = "https" })
            });
        }
    }
}

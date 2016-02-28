namespace OmegleR
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    ///     Configure routes.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        ///     Register site routes.
        /// </summary>
        /// <param name="routes">A <see cref="T:System.Web.Routing.RouteCollection"/>.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}

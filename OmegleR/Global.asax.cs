namespace OmegleR
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <inheritdoc/>
    public class MvcApplication : HttpApplication
    {
        /// <inheritdoc/>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}

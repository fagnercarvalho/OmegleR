using Microsoft.Owin;

[assembly: OwinStartup(typeof(OmegleR.Startup))]

namespace OmegleR
{
    using Owin;

    /// <summary>
    ///     OWIN Startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Configuring middlewares in the OWIN pipeline.
        /// </summary>
        /// <param name="app">Reference to object where you add OWIN middlewares.</param>
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}

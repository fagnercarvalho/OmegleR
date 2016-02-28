namespace OmegleR.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    ///     Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        ///     Gets the chat view.
        /// </summary>
        /// <returns>Operation result.</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
using System.Web.Mvc;

using Unity.Attributes;

using GlancePay.OmnivoreIntegration.Logging;

namespace GlancePay.OmnivoreIntegration.UI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(INLogManager nLogManager) : base() { }

        [Dependency]
        public INLogManager Logger { get; set; }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}

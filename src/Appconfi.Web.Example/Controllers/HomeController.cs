using System.Web.Mvc;

namespace Appconfi.Web.Example.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(IFeatureManager manager)
        {
            Manager = manager;
        }

        public IFeatureManager Manager { get; }

        public ActionResult Index()
        {
            var isFeatureEnable = Manager.IsEnable("my_awesome_feature");
            ViewBag.FeatureEnable = isFeatureEnable;
            return View();
        }
    }
}
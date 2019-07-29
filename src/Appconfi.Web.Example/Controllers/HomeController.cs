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
            return View(Manager);
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace EmulatorsRequester.Controllers
{
    public class SOHOController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

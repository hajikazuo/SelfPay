using Microsoft.AspNetCore.Mvc;

namespace SelfPay.Mvc.Controllers
{
    public class ClientInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

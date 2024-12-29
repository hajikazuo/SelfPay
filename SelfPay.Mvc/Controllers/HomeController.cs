using Microsoft.AspNetCore.Mvc;
using SelfPay.Common.Repositories.Interface;
using System.Diagnostics;

namespace SelfPay.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public HomeController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceRepository.GetAllAsync();
            return View(services);
        }
    }
}

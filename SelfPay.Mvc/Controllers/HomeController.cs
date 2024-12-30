using Microsoft.AspNetCore.Mvc;
using RT.Comb;
using SelfPay.Common.Models;
using SelfPay.Common.Repositories.Interface;
using System.Diagnostics;

namespace SelfPay.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IClientInfoRepository _clientInfoRepository;
        private readonly ICombProvider _comb;

        public HomeController(IServiceRepository serviceRepository, IClientInfoRepository clientInfoRepository, ICombProvider comb)
        {
            _serviceRepository = serviceRepository;
            _clientInfoRepository = clientInfoRepository;
            _comb = comb;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientInfo clientInfo)
        {
            if (ModelState.IsValid)
            {
                clientInfo.Id = _comb.Create();
                await _clientInfoRepository.CreateAsync(clientInfo);

                TempData["Confirm"] = " <script>$(document).ready(function(){MostraConfirm('Sucesso','Cadastrado com sucesso');})</script>";
                return RedirectToAction(nameof(Service), new { id = clientInfo.Id });
            }
            return View(clientInfo);
        }

        public async Task<IActionResult> Service(Guid? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClientId = id;
            var services = await _serviceRepository.GetAllAsync();
            return View(services);
        }
    }
}

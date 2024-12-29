using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RT.Comb;
using SelfPay.Common.Data;
using SelfPay.Common.Models;
using SelfPay.Common.Repositories.Interface;
using SelfPay.Mvc.Services.Interface;

namespace SelfPay.Mvc.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ServicesController : Controller
    {      
        private readonly IServiceRepository _serviceRepository;
        private readonly IUploadService _uploadService;
        private readonly ICombProvider _comb;

        public ServicesController(IServiceRepository serviceRepository, IUploadService uploadService, ICombProvider comb)
        {
            _serviceRepository = serviceRepository;
            _uploadService = uploadService;
            _comb = comb;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceRepository.GetAllAsync();
            ViewBag.Confirm = TempData["Confirm"];
            return View(services);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service =  await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                service.Id = _comb.Create();
                service.ImageUrl = await _uploadService.UploadPhoto(file);
                await _serviceRepository.CreateAsync(service);

                TempData["Confirm"] = " <script>$(document).ready(function(){MostraConfirm('Sucesso','Cadastrado com sucesso');})</script>";
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);

            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Service service, IFormFile? file)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    service.ImageUrl = await _uploadService.UploadPhoto(file);
                }

                await _serviceRepository.UpdateAsync(service);
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Atualizado com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            if (service != null)
            {
                await _serviceRepository.DeleteAsync(id);
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Excluído com sucesso!');})</script>";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SelfPay.Common.Repositories.Interface;
using SelfPay.Common.ViewModels;

namespace SelfPay.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public OrderController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IActionResult> Create(Guid id, decimal price)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            var response = new OrderViewModel();
            response.Name = service.Name;
            response.Description = service.Description; 
            response.Price = price;
            return View(response);
        }
    }
}

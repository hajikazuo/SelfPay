using Microsoft.EntityFrameworkCore;
using SelfPay.Common.Data;
using SelfPay.Common.Models;
using SelfPay.Common.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Repositories.Implementation
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly SelfPayContext _context;

        public ServiceRepository(SelfPayContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service> GetByIdAsync(Guid id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Service> CreateAsync(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            return service;
        }

        public async Task<Service> UpdateAsync(Service service)
        {
            var existingItem = await _context.Services.FirstOrDefaultAsync(s => s.Id == service.Id);
            if (existingItem is null)
            {
                return null;
            }

            _context.Entry(existingItem).CurrentValues.SetValues(service);

            await _context.SaveChangesAsync();

            return service;
        }

        public async Task<Service> DeleteAsync(Guid id)
        {
            var existingItem = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (existingItem is null)
            {
                return null;
            }

            _context.Services.Remove(existingItem);
            await _context.SaveChangesAsync();

            return existingItem;
        }
    }
}

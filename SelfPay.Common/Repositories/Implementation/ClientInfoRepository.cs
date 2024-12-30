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
    public class ClientInfoRepository : IClientInfoRepository
    {
        private readonly SelfPayContext _context;

        public ClientInfoRepository(SelfPayContext context)
        {
            _context = context;
        }

        public async Task<List<ClientInfo>> GetAllAsync()
        {
            return await _context.ClientInfos.ToListAsync();
        }

        public async Task<ClientInfo> GetByIdAsync(Guid id)
        {
            return await _context.ClientInfos.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<ClientInfo> CreateAsync(ClientInfo clientInfo)
        {
            await _context.ClientInfos.AddAsync(clientInfo);
            await _context.SaveChangesAsync();

            return clientInfo;
        }

        public async Task<ClientInfo> UpdateAsync(ClientInfo clientInfo)
        {
            var existingItem = await _context.ClientInfos.FirstOrDefaultAsync(s => s.Id == clientInfo.Id);
            if (existingItem is null)
            {
                return null;
            }

            _context.Entry(existingItem).CurrentValues.SetValues(clientInfo);

            await _context.SaveChangesAsync();

            return clientInfo;
        }

        public async Task<ClientInfo> DeleteAsync(Guid id)
        {
            var existingItem = await _context.ClientInfos.FirstOrDefaultAsync(s => s.Id == id);
            if (existingItem is null)
            {
                return null;
            }

            _context.ClientInfos.Remove(existingItem);
            await _context.SaveChangesAsync();

            return existingItem;
        }
    }
}

using SelfPay.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Repositories.Interface
{
    public interface IClientInfoRepository
    {
        Task<List<ClientInfo>> GetAllAsync();
        Task<ClientInfo> GetByIdAsync(Guid id);
        Task<ClientInfo> CreateAsync(ClientInfo clientInfo);
        Task<ClientInfo> UpdateAsync(ClientInfo clientInfo);
        Task<ClientInfo> DeleteAsync(Guid id);
    }
}

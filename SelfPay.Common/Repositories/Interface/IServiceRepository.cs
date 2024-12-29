using Microsoft.AspNetCore.Identity;
using SelfPay.Common.Models;
using SelfPay.Common.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Repositories.Interface
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetAllAsync();
        Task<Service> GetByIdAsync(Guid id);
        Task<Service> CreateAsync(Service service);
        Task<Service> UpdateAsync(Service service);
        Task<Service> DeleteAsync(Guid id);
    }
}

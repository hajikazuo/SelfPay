using Microsoft.AspNetCore.Identity;
using SelfPay.Common.Models.Users;
using SelfPay.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<IdentityResult> CreateAsync(User usuario, string password);
        Task<IdentityResult> UpdateAsync(User usuario);
        Task<IdentityResult> DeleteAsync(User usuario);

        Task<List<SelectGenericList>> GetAllRolesAsync();
    }
}

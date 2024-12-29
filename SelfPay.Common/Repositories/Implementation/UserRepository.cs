using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SelfPay.Common.Data;
using SelfPay.Common.Models.Users;
using SelfPay.Common.Repositories.Interface;
using SelfPay.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Repositories.Implementation
{
    public class UsuarioRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SelfPayContext _context;

        public UsuarioRepository(UserManager<User> userManager, SelfPayContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IdentityResult> CreateAsync(User usuario, string password)
        {
            return await _userManager.CreateAsync(usuario, password);
        }

        public async Task<IdentityResult> UpdateAsync(User usuario)
        {
            return await _userManager.UpdateAsync(usuario);
        }

        public async Task<IdentityResult> DeleteAsync(User usuario)
        {
            return await _userManager.DeleteAsync(usuario);
        }

        public async Task<List<SelectGenericList>> GetAllRolesAsync()
        {
            return await _context.Roles.Select(role => new SelectGenericList
            {
                Id = role.Name,
                Name = role.Name
            }).ToListAsync();
        }
    }
}

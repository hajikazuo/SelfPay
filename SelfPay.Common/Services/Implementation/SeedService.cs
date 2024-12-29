using Microsoft.AspNetCore.Identity;
using SelfPay.Common.Data;
using SelfPay.Common.Models.Users;
using SelfPay.Common.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfPay.Common.Services.Implementation
{
    public class SeedService : ISeedService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SelfPayContext _context;

        private const string Admin = "Admin";
        private const string User = "Usuario";

        public SeedService(UserManager<User> userManager, RoleManager<Role> roleManager, SelfPayContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Seed()
        {
            CreateRole(Admin).GetAwaiter().GetResult();
            CreateRole(User).GetAwaiter().GetResult();
            CreateUser("admin@selfpay.com.br", name: "Administrador", "Teste@2024", roles: new List<string> { Admin, User }).GetAwaiter().GetResult();
        }

        private async Task<IdentityResult> CreateUser(string email, string name, string password, IEnumerable<string> roles)
        {
            var retorno = await _userManager.FindByEmailAsync(email);
            if (retorno == null)
            {
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, roles);
                }

                return result;
            }
            else
            {
                return default;
            }
        }

        private async Task<IdentityResult> CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role
                {
                    Name = roleName
                };
                return await _roleManager.CreateAsync(role);
            }
            return default;
        }
    }
}

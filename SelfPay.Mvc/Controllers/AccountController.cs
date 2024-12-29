using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RT.Comb;
using SelfPay.Common.Models.Users;
using SelfPay.Common.Repositories.Implementation;
using SelfPay.Common.Repositories.Interface;
using SelfPay.Common.ViewModels.AccountViewModels;
using System;

namespace SelfPay.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            ViewBag.Confirm = TempData["Confirm"];
            return View(users);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            ViewData["Role"] = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                };

                var result = await _userRepository.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    if (model.SelectedRole != null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    }

                    TempData["Confirm"] = " <script>$(document).ready(function(){MostraConfirm('Sucesso','Cadastrado com sucesso');})</script>";
                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            }

            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, string returnUrl = null)
        {
            var userDb = await _userRepository.GetByIdAsync(id);
            if (userDb == null)
            {
                return NotFound();
            }

            var role = await _userManager.GetRolesAsync(userDb);

            var model = new RegisterViewModel
            {
                Id = userDb.Id,
                UserName = userDb.UserName,
                SelectedRole = role.FirstOrDefault(),
            };

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterViewModel model, Guid id, IFormFile? file, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.Email = model.UserName;

            
                var result = await _userRepository.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (model.SelectedRole != null)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    }

                    TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Atualizado com sucesso!');})</script>";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Funcoes"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                var result = await _userRepository.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Excluído com sucesso!');})</script>";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid id)
        {
            var userDb = await _userRepository.GetByIdAsync(id);
            if (userDb == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel
            {
                Id = userDb.Id,
            };

            ViewBag.Confirm = TempData["Confirm"];
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    AddErrors(removePasswordResult);
                    return View(model);
                }

                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    AddErrors(addPasswordResult);
                    return View(model);
                }
            }

            var updateResult = await _userRepository.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                TempData["Confirm"] = "<script>$(document).ready(function () {MostraConfirm('Sucesso', 'Senha atualizada com sucesso!');})</script>";
                return RedirectToAction(nameof(Index));
            }
            AddErrors(updateResult);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}

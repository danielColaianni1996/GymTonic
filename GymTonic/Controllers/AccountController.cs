using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GymTonic.DataBase;
using GymTonic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymTonic.Controllers
{
    public class AccountController : Controller
    {
        private readonly GymDataContest gymDataContest;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(GymDataContest gymDataContest, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.gymDataContest = gymDataContest;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result= await signInManager.PasswordSignInAsync(model.Mail, model.Password, isPersistent: model.Ricordami, false);
                if (result.Succeeded)
                { 
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }
    }
}
using System.Security.Claims;
using BeatsShop.DAL.DomainModels;
using BeatsShop.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeatsShop.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        if (User.Identity!.IsAuthenticated)
            return Redirect(returnUrl ?? Url.Content("~/"));

        var vm = new LoginViewModel
        {
            ReturnUrl = returnUrl ?? Url.Content("~/"),
            ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync()
        };

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View(vm);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ReturnUrl ??= Url.Content("~/");

        var user = await _userManager.FindByNameAsync(model.UserName ?? throw new Exception("Unexpected error"));

        if(user is null)
        {
            model.StatusMessage = "Wrong login or password";
            model.IsSuccess = false;

            return View(model);
        }

        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password ?? throw new Exception("Unexpected error"), model.RememberLogin, true);

        switch (result.Succeeded)
        {
            case true when Url.IsLocalUrl(model.ReturnUrl):
                return Redirect(model.ReturnUrl);
            case true:
                model.StatusMessage = "Invalid return URL";
                break;
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity!.IsAuthenticated)
            await _signInManager.SignOutAsync();

        return Redirect("/");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string? returnUrl = null)
    {
        var model = new RegisterViewModel
        {
            ReturnUrl = returnUrl ?? Url.Content("~/"),
            ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync()
        };

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.StatusMessage = "Registration error";

            return View(model);
        }

        var newUser = new User
        {
            UserName = model.UserName,
            NormalizedUserName = model.UserName!.ToUpper(),
            Email = model.Email,
            NormalizedEmail = model.Email!.ToUpper(),
            PhoneNumber = model.PhoneNumber
        };

        var result = await _userManager.CreateAsync(newUser, model.Password ?? throw new ArgumentException("Unexpected error"));

        if (!result.Succeeded)
            throw new ArgumentException(result.Errors.First().Description);

        result = await _userManager.AddClaimsAsync(newUser,
                                                   new []
                                                   {
                                                       new Claim("email", model.Email!)
                                                   });

        if (!result.Succeeded)
        {
            model.StatusMessage = "Registration error";

            return View(model);
        }

        await _userManager.AddToRoleAsync(newUser, "User");
        var user = await _userManager.FindByEmailAsync(newUser.Email ?? throw new Exception("Email not found"));

        model.ReturnUrl ??= Url.Content("~/");

        if(user?.Email is null)
            throw new Exception("User not found");

        await _signInManager.SignInAsync(user, model.RememberLogin);

        return Redirect(model.ReturnUrl);
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tyres.Data.Models;
using System.Text.RegularExpressions;

namespace Tyres.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private const string ReferrerRouteKey = "route";

        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            //returnUrl = returnUrl ?? Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            //ReturnUrl = returnUrl;

            TempData[ReferrerRouteKey] = ExtractRefererRoute();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");

            returnUrl = TempData[ReferrerRouteKey].ToString();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }

        private string ExtractRefererRoute()
        {
            var RefererRequestHeaderKey = "Referer";
            var HostRequestHeaderKey = "Host";

            var headers = HttpContext.Request.Headers;

            var refererUrl = headers[RefererRequestHeaderKey];
            var host = headers[HostRequestHeaderKey];

            var regex = $"(?<={host}).*$";
            var match = Regex.Match(refererUrl, regex);

            if (!match.Success)
            {
                return null;
            }

            var route = match.Value;
            return route;
        }

    }
}

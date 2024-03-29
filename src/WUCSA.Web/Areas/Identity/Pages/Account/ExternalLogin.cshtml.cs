using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly ImageHelper _imageHelper;

        public ExternalLoginModel(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ILogger<ExternalLoginModel> logger,
            ImageHelper imageHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _imageHelper = imageHelper;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            var xEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            var xUser = await _userManager.FindByEmailAsync(xEmail);
            if (xUser != null && xUser.IsBlocked)
            {
                return RedirectToPage("./Lockout");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    string firstsName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    string firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                    string lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                    string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    var user = new AppUser
                    {
                        UserName = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        EmailConfirmed = true
                    };
                    user.ProfilePhotoPath = _imageHelper.GenerateImage($"{user.Id}_profile", "profile_imgs");

                    var resultU = await _userManager.CreateAsync(user);
                    if (resultU.Succeeded)
                    {
                        resultU = await _userManager.AddLoginAsync(user, info);
                        if (resultU.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in resultU.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    //Input = new InputModel
                    //{
                    //    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    //};
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                string firstsName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                string firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                string lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                AppUser user;
                user = new AppUser
                {
                    UserName = Input.Email,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = Input.Email
                };
                user.ProfilePhotoPath = _imageHelper.GenerateImage($"{user.Id}_profile", "profile_imgs");
                
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}

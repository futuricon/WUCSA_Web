using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ImageHelper _imageHelper;
        private readonly IEmailService _emailSender;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ImageHelper imageHelper,
            IEmailService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _imageHelper = imageHelper;
        }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            public bool generateImg { get; set; } = false;

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string ProfilePhotoUrl { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            IsEmailConfirmed = user.EmailConfirmed;

            Input = new InputModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePhotoUrl = user.ProfilePhotoPath
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var prevImg = user.ProfilePhotoPath;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.UserName = Input.Username;
            user.Email = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

            //if (Input.UploadPhoto != null)
            //{
            //    user.ProfilePhotoPath = _imageHelper.UploadCoverImage(Input.UploadPhoto, $"{user.Id}_profile", "profile_imgs");
            //}
            if(Input.generateImg)
            {
                user.ProfilePhotoPath = _imageHelper.GenerateImage($"{user.Id}_profile", "profile_imgs");
            }
            //if (user.ProfilePhotoPath != prevImg)
            //{
            //    _imageHelper.RemoveImage(prevImg, "profile_imgs");
            //}
            var result = await _userManager.UpdateAsync(user);

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateImgAsync(string base64str)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || base64str == null || base64str.Length < 6)
            {
                return BadRequest($"Something went wrong!");
            }

            user.ProfilePhotoPath = _imageHelper.UploadPostCoverImage(base64str, $"{user.Id}_profile");
            var result = await _userManager.UpdateAsync(user);

            foreach (var error in result.Errors)
            {
                return BadRequest(error.Description);
            }

            return new OkObjectResult(user.ProfilePhotoPath);
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new { userId = user.Id, code },
                Request.Scheme);

            await _emailSender.SendAsync(
                user.Email,
                "Confirm your email wucsa.com",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Admin.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ImageHelper _imageHelper;

        public EditModel(UserManager<AppUser> userManager,
            RoleManager<UserRole> roleManager, ImageHelper imageHelper,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _imageHelper = imageHelper;
        }

        [BindProperty]
        public AppUser AppUser { get; set; }

        [BindProperty]
        public bool IsAdmin { get; set; }

        [BindProperty]
        public IFormFile ProfilePhoto { get; set; }

        [BindProperty]
        public bool GenerateImg { get; set; } = false;

        public UserManager<AppUser> UserManager => _userManager;

        public async Task<IActionResult> OnGetAsync(string id, string mode)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await UserManager.FindByIdAsync(id);

            if (AppUser == null)
            {
                return NotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(AppUser);
            if (userRoles.Contains(Role.Admin.ToString()))
            {
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }

            ViewData["mode"] = mode.ToLower();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string mode)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await UserManager.FindByIdAsync(AppUser.Id);

            if (user == null)
            {
                return NotFound();
            }

            var UserRolesName = new List<string>();

            user.UserName = AppUser.UserName;
            user.FirstName = AppUser.FirstName;
            user.LastName = AppUser.LastName;
            user.IsBlocked = AppUser.IsBlocked;

            if (user.IsBlocked)
            {
                var endDate = DateTime.Now.AddMinutes(20);
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, endDate);
                await _userManager.UpdateSecurityStampAsync(user);
            }
            if (IsAdmin)
            {
                UserRolesName.Add("Admin");
            }

            await _userRepository.UpdateUserRolesAsync(user, UserRolesName);


            if (ProfilePhoto != null)
            {
                user.ProfilePhotoPath = _imageHelper.UploadCoverImage(ProfilePhoto, $"{user.Id}_profile", "profile_imgs");
            }
            else if (GenerateImg)
            {
                user.ProfilePhotoPath = _imageHelper.GenerateImage($"{user.Id}_profile", "profile_imgs");
            }

            await UserManager.UpdateAsync(user);
            return RedirectToPage("./Details", new {mode, id = AppUser.Id});
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Web.Pages.Admin.UserRoles
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<UserRole> _roleManager;

        public IndexModel(RoleManager<UserRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            UserManager = userManager;
        }

        public IList<UserRole> UserRoles { get; set; }
        public UserManager<AppUser> UserManager { get; }

        public async Task<IActionResult> OnGetAsync()
        {
            UserRoles = await _roleManager.Roles.ToListAsync();

            return Page();
        }
    }
}

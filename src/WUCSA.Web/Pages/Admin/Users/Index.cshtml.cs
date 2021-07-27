using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Web.Pages.Admin.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public PaginatedList<AppUser> Users { get; set; }

        public void OnGet(string mode, int pageIndex = 1)
        {
            ViewData["mode"] = mode.ToLower();
            var users = _userManager.Users;
            Users = PaginatedList<AppUser>.Create(users, pageIndex, 20);
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Admin.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class DetailsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlogRepository _blogRepository;

        public DetailsModel(UserManager<AppUser> userManager, IBlogRepository blogRepository)
        {
            _userManager = userManager;
            _blogRepository = blogRepository;
        }

        public AppUser AppUser { get; set; }

        public bool IsAdmin { get; set; }

        public List<Core.Entities.BlogModel.Comment> Comments { get; set; }

        public List<Core.Entities.BlogModel.Blog> Blogs { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string mode)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["mode"] = mode.ToLower();

            AppUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (AppUser == null)
            {
                return NotFound();
            }
            
            var userRoles = await _userManager.GetRolesAsync(AppUser);

            if (userRoles.Contains(Role.Admin.ToString()))
            {
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }

            Blogs = await _blogRepository.GetListAsync<Core.Entities.BlogModel.Blog>(i => i.Author == AppUser);
            Comments = await _blogRepository.GetListAsync<Core.Entities.BlogModel.Comment>(i => i.Author == AppUser);

            return Page();
        }
    }
}

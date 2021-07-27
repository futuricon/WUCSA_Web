using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Admin.StaticPages
{
    [Authorize(Roles = "SuperAdmin")]
    public class DeleteModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public DeleteModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        [BindProperty]
        public Core.Entities.BlogModel.Blog Blog { get; set; }
        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id != null)
            {
                Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);
                Tags = Tag.JoinTags(Blog.BlogTags.Select(i => i.Tag));

                if (Blog == null)
                {
                    return NotFound();
                }

                return Page();
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);

            if (Blog != null)
            {
                await _blogRepository.DeleteBlogAsync(Blog);
            }

            return (Blog.StaticPage.ToString().ToLower()) switch
            {
                "about" => RedirectToPage("/About"),
                "history" => RedirectToPage("/History"),
                _ => RedirectToPage("/Privacy"),
            };
        }
    }
}

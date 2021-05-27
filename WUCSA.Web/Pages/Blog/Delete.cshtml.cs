using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;

        public DeleteModel(ImageHelper imageHelper, IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _imageHelper = imageHelper;
        }

        [BindProperty]
        public Core.Entities.BlogModel.Blog Blog { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);
            Tags = Tag.JoinTags(Blog.BlogTags.Select(i => i.Tag));

            if (Blog == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("SuperAdmin"))
            {
                if (Blog.IsDeleted)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);

            if (User.IsInRole("SuperAdmin"))
            {
                if (Blog != null)
                {
                    await _blogRepository.DeleteBlogAsync(Blog);
                    _imageHelper.RemoveImage(Blog.CoverPhotoPath, "post_imgs");
                }
            }
            else
            {
                Blog.IsDeleted = true;
            }
            return RedirectToPage("/Blog/List");
        }
    }
}

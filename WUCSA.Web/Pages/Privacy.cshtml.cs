using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public PrivacyModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public string RCName { get; set; }
        public Core.Entities.BlogModel.Blog Blog { get; set; }

        public async Task<IActionResult> OnGet()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;

            var blog = await _blogRepository.GetListAsync<Core.Entities.BlogModel.Blog>(i => i.IsDeleted == true && i.StaticPage == Core.Entities.BlogModel.StaticPage.Privacy);
            if (blog != null)
            {
                Blog = blog.ToList().OrderByDescending(i => i.PostedDate).FirstOrDefault();
            }
            return Page();
        }
    }
}

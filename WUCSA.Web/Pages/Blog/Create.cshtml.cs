using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;

        public CreateModel(UserManager<AppUser> userManager,
            ImageHelper imageHelper,
            IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _userManager = userManager;
            _imageHelper = imageHelper;
        }


        public class InputModel
        {
            public Core.Entities.BlogModel.Blog Blog { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var tags = Tag.ParseTags(Input.Tags);
            Input.Blog.Slug = Input.Blog.Title.Slugify();
            Input.Blog.Author = currentUser;

            if (Input.UploadCoverPhoto != null)
            {
                Input.Blog.CoverPhotoPath = _imageHelper.UploadImage(Input.UploadCoverPhoto, $"{Input.Blog.Id}_blog_cover");
            }

            await _blogRepository.UpdateTagsAsync(Input.Blog, false, tags);
            await _blogRepository.AddBlogAsync(Input.Blog);
            return RedirectToPage("/Blog/Index", new { slug = Input.Blog.Slug });
        }
    }
}

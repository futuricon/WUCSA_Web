using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Blog
{
    [Authorize(Roles = "SuperAdmin,Admin,Editor")]
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IBlogRepository _blogRepository;

        public EditModel(ImageHelper imageHelper, IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
            _imageHelper = imageHelper;
        }

        public class InputModel
        {
            public Core.Entities.BlogModel.Blog Blog { get; set; }
            public string UploadCoverPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);

            Input = new InputModel()
            {
                Blog = blog,
                Tags = Tag.JoinTags(blog.BlogTags.Select(i => i.Tag))
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(Input.Blog.Id);
            blog.Title = Input.Blog.Title;
            blog.TitleRu = Input.Blog.TitleRu;
            blog.TitleUz = Input.Blog.TitleUz;
            blog.Content = Input.Blog.Content;
            blog.ContentRu = Input.Blog.ContentRu;
            blog.ContentUz = Input.Blog.ContentUz;
            blog.IsCommentsLocked = Input.Blog.IsCommentsLocked;
            blog.Slug = Input.Blog.Title.Slugify();
            var tags = Tag.ParseTags(Input.Tags);

            if (Input.UploadCoverPhoto != null && Input.UploadCoverPhoto.Length > 6)
            {
                Input.Blog.CoverPhotoPath = _imageHelper.UploadPostCoverImage(Input.UploadCoverPhoto, $"{Input.Blog.Id}_blog_cover");
            }

            await _blogRepository.UpdateTagsAsync(blog, false, tags);
            await _blogRepository.UpdateBlogAsync(blog);
            return RedirectToPage("/Blog/Index", new { slug = blog.Slug });
        }
    }
}

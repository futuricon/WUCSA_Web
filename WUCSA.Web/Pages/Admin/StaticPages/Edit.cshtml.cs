using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Admin.StaticPages
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public EditModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public class InputModel
        {
            public Core.Entities.BlogModel.Blog Blog { get; set; }
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

            ViewData.Add("toolbar", new[]
            {
                "Bold", "Italic", "Underline", "StrikeThrough",
                "FontName", "FontSize", "FontColor", "BackgroundColor", "|",
                "Formats", "Alignments", "OrderedList", "UnorderedList",
                "Outdent", "Indent", "|", "CreateTable", "CreateLink", "Image", "|",
                "ClearFormat", "SourceCode", "FullScreen", "|", "Undo", "Redo"
            });

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
            blog.Slug = Input.Blog.Title.Slugify();
            var tags = Tag.ParseTags(Input.Tags);

            await _blogRepository.UpdateTagsAsync(blog, false, tags);
            await _blogRepository.UpdateBlogAsync(blog);

            return (blog.StaticPage.ToString().ToLower()) switch
            {
                "about" => RedirectToPage("/About"),
                "history" => RedirectToPage("/History"),
                _ => RedirectToPage("/Privacy"),
            };
        }
    }
}

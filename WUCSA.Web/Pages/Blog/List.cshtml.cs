using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Blog
{
    public class ListModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;
    
        public ListModel(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
    
        public PaginatedList<Core.Entities.BlogModel.Blog> Blogs { get; set; }
        public Core.Entities.BlogModel.Blog[] PopularArticles { get; set; }
        public string[] PopularTags { get; set; }
    
        public async Task<IActionResult> OnGetAsync(int pageIndex = 1, string tag = null)
        {
            var blogs = _blogRepository.GetAll<Core.Entities.BlogModel.Blog>();
    
            if (tag != null)
            {
                var taggedBlogs = (await _blogRepository.GetListAsync<Core.Entities.BlogModel.Blog>())
                    .SelectMany(i => i.BlogTags).Where(i => i.Tag.Name.ToLower() == tag.ToLower())
                    .OrderByDescending(i => i.Blog.PostedDate).Select(i => i.Blog);
    
                Blogs = PaginatedList<Core.Entities.BlogModel.Blog>.Create(taggedBlogs, pageIndex, 5);
            }
            else
            {
                Blogs = PaginatedList<Core.Entities.BlogModel.Blog>.Create(blogs.OrderByDescending(i => i.PostedDate), pageIndex, 5);
            }
    
            PopularArticles = blogs.OrderByDescending(i => i.ViewCount).Take(5).ToArray();
            PopularTags = await GetPopularTagsAsync(blogs);
            return Page();
        }
    
        public Task<string[]> GetPopularTagsAsync(IQueryable<Core.Entities.BlogModel.Blog> blogs)
        {
            return Task.Run(() =>
            {
                var tags = new List<string>();
    
                foreach (var blog in blogs)
                {
                    tags.AddRange(blog.BlogTags.Select(i => i.Tag.Name));
                }
    
                var popularTags = tags.GroupBy(str => str)
                    .Select(i => new { Name = i.Key, Count = i.Count() })
                    .OrderByDescending(k => k.Count).Select(i => i.Name).Take(10).ToArray();
    
                return popularTags;
            });
        }
    }
}

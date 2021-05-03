using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class BlogRepository : Repository, IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //////////////// Blog ////////////////
        public Task AddBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return AddAsync(blog);
        }

        public Task UpdateBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return UpdateAsync(blog);
        }

        public async Task DeleteBlogAsync(Blog blog)
        {
            foreach (var comment in blog.Comments)
            {
                await DeleteCommentAsync(comment, false);
            }

            await DeleteAsync(blog);
        }

        private string GetVerifiedBlogSlug(Blog slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Blog>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }


        //////////////// Comment ////////////////
        
        public Task AddCommentAsync(Blog blog, Comment comment)
        {
            blog.Comments.Add(comment);
            return UpdateAsync(blog);
        }

        public async Task DeleteCommentAsync(Comment comment, bool saveChanges = true)
        {
            await RemoveChildrenCommentsAsync(comment);
            var rootComment = _context.Set<Comment>().FirstOrDefault(i => i.Id == comment.Id);

            if (rootComment != null)
            {
                _context.Remove(rootComment);
            }

            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }

        public Task AddReplyToCommentAsync(Comment parentComment, Comment childComment)
        {
            parentComment.Replies.Add(childComment);
            return UpdateAsync(parentComment);
        }       

        private async Task RemoveChildrenCommentsAsync(Comment comment)
        {
            foreach (var reply in comment.Replies)
            {
                await RemoveChildrenCommentsAsync(reply);
                _context.Remove(reply);
            }
        }


        //////////////// Tag ////////////////

        public async Task UpdateTagsAsync(Blog blog, bool saveChanges = true, params Tag[] tags)
        {
            List<string> nameList = new List<string>();
            List<Tag> tagList = new List<Tag>();
            for (int i = 0; i < tags.Count(); i++)
            {
                nameList.Add(tags[i].Name.ToLower());
            }
            foreach (var blogTag in blog.BlogTags)
            {
                if (nameList.Contains(blogTag.Tag.Name.ToLower()))
                {
                    tags.ToList().RemoveAll(x => x.Name == blogTag.Tag.Name.ToLower());
                }
                else
                {
                    var originTag = await GetAsync<Tag>(i => i.Name.ToLower() == blogTag.Tag.Name.ToLower());
                    if (originTag.BlogTags.Count<=1)
                    {
                        tagList.Add(blogTag.Tag);
                    }
                    _context.Set<BlogTag>().Remove(blogTag);
                }
            }

            foreach (var tag in tagList)
            {
                await DeleteAsync(tag);
            }

            foreach (var tag in tags)
            {
                var originTag = await GetAsync<Tag>(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new Tag(tag);
                    await _context.Set<Tag>().AddAsync(originTag);
                }

                if (blog.BlogTags.Any(i => i.Tag.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                blog.BlogTags.Add(new BlogTag
                {
                    Tag = originTag
                });
            }
            

            if (saveChanges)
            {
                await UpdateAsync(blog);
            }
        }
               
        
    }
}

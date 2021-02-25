using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Blog> Blogs => throw new NotImplementedException();

        public IQueryable<Comment> Comments => throw new NotImplementedException();

        public Task AddBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return AddAsync(blog);
        }

        public Task AddCommentAsync(Blog blog, Comment comment)
        {
            blog.Comments.Add(comment);
            return UpdateAsync(blog);
        }

        public Task AddReplyToCommentAsync(Comment parentComment, Comment childComment)
        {
            parentComment.Replies.Add(childComment);
            return UpdateAsync(parentComment);
        }

        public async Task DeleteBlogAsync(Blog blog)
        {
            foreach (var comment in blog.Comments)
            {
                await DeleteCommentAsync(comment, false);
            }

            await DeleteAsync(blog);
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

        public Task GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTagsAsync(Blog blog, params Tag[] tags)
        {
            foreach (var tag in tags)
            {
                // ReSharper disable once SpecifyStringComparison
                var originTag = await GetAsync<Tag>(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new Tag(tag);
                    await _context.Set<Tag>().AddAsync(originTag);
                }

                // ReSharper disable once SpecifyStringComparison
                if (blog.BlogTags.Any(i => i.Tag.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                blog.BlogTags.Add(new BlogTag
                {
                    Tag = originTag
                });
            }
        }

        public Task UpdateBlogAsync(Blog blog)
        {
            blog.Slug = GetVerifiedBlogSlug(blog);
            return UpdateAsync(blog);
        }

        public virtual Task<TEntity> GetAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public virtual Task DeleteAsync(Blog entity)
        {
            var sourceEntity = _context.Set<Blog>().FirstOrDefault(i => i.Id == entity.Id);

            if (sourceEntity == null)
                return Task.CompletedTask;

            _context.Remove(sourceEntity);
            return _context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        private async Task RemoveChildrenCommentsAsync(Comment comment)
        {
            foreach (var reply in comment.Replies)
            {
                await RemoveChildrenCommentsAsync(reply);
                _context.Remove(reply);
            }
        }

        private string GetVerifiedBlogSlug(ISlugifiedEntity slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Blog>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }
    }
}

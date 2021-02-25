using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.BlogModel;

namespace WUCSA.Core.Interfaces
{
    public interface IBlogRepository
    {
        Task GetByIdAsync(string id);
        IQueryable<Blog> Blogs { get; }
        Task AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(Blog blog);
        Task UpdateTagsAsync(Blog blog, params Tag[] tags);

        IQueryable<Comment> Comments { get; }
        Task AddCommentAsync(Blog blog, Comment comment);
        Task AddReplyToCommentAsync(Comment parentComment, Comment childComment);
        Task DeleteCommentAsync(Comment comment, bool saveChanges = true);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.BlogModel;

namespace WUCSA.Core.Interfaces
{
    public interface IBlogRepository : IRepository
    {
        Task AddBlogAsync(Blog blog);
        Task AddCommentAsync(Blog blog, Comment comment);
        Task AddReplyToCommentAsync(Comment parentComment, Comment childComment);
        Task UpdateBlogAsync(Blog blog);
        Task UpdateTagsAsync(Blog blog, bool saveChanges = true, params Tag[] tags);
        Task DeleteBlogAsync(Blog blog);
        Task DeleteCommentAsync(Comment comment, bool saveChanges = true);
    }
}

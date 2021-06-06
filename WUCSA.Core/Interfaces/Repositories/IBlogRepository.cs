using System.Threading.Tasks;
using WUCSA.Core.Entities.BlogModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IBlogRepository : IRepository
    {
        Task AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(Blog blog);
        Task AddCommentAsync(Blog blog, Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task AddReplyToCommentAsync(Comment parentComment, Comment childComment);
        Task DeleteCommentAsync(Comment comment, bool saveChanges = true);
        Task UpdateTagsAsync(Blog blog, bool saveChanges = true, params Tag[] tags);
    }
}

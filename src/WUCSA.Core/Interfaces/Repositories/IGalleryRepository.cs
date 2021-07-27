using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IGalleryRepository : IRepository
    {
        Task AddMediaAsync(Media media);
        Task UpdateMediaAsync(Media media);
        Task DeleteMediaAsync(Media media);
        Task UpdateTagsAsync(Media media, bool saveChanges = true, params MTag[] tags);
        Task AddLikeAsync(Media media, AppUser user);
        Task RemoveLikeAsync(Media media, AppUser user);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;

namespace WUCSA.Core.Interfaces
{
    public interface IGalleryRepository
    {
        Task GetByIdAsync(string id);
        IQueryable<Media> Medias { get; }
        Task AddBlogAsync(Media media);
        Task UpdateBlogAsync(Media media);
        Task DeleteBlogAsync(Media media);
        Task UpdateTagsAsync(Media media, params MTag[] tags);

        IQueryable<MediaTag> MediaTags { get; }
    }
}

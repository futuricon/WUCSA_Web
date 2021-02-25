using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Infrastructure.Repositories
{
    public class GalleryRepository : IGalleryRepository
    {
        public IQueryable<Media> Medias => throw new NotImplementedException();

        public IQueryable<MediaTag> MediaTags => throw new NotImplementedException();

        public Task AddBlogAsync(Media media)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBlogAsync(Media media)
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBlogAsync(Media media)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTagsAsync(Media media, params MTag[] tags)
        {
            throw new NotImplementedException();
        }
    }
}

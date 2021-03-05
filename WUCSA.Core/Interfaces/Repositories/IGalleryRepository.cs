using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IGalleryRepository : IRepository
    {
        Task AddMediaAsync(Media media);
        Task UpdateMediaAsync(Media media);
        Task DeleteMediaAsync(Media media);
        Task UpdateTagsAsync(Media media, bool saveChanges = true, params MTag[] tags);
    }
}

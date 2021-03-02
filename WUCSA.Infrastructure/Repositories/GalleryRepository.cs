using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Interfaces;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class GalleryRepository : Repository, IGalleryRepository
    {
        private readonly ApplicationDbContext _context;

        public GalleryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Task AddMediaAsync(Media media)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMediaAsync(Media media)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMediaAsync(Media media)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTagsAsync(Media media, bool saveChanges = true, params MTag[] tags)
        {
            throw new NotImplementedException();
        }
    }
}

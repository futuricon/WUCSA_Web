using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Interfaces;
using WUCSA.Core.Interfaces.Repositories;
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
            return AddAsync(media);
        }

        public async Task DeleteMediaAsync(Media media)
        {
            await DeleteAsync(media);
        }

        public Task UpdateMediaAsync(Media media)
        {
            return UpdateAsync(media);
        }

        public async Task UpdateTagsAsync(Media media, bool saveChanges = true, params MTag[] tags)
        {
            foreach (var tag in tags)
            {
                var originTag = await GetAsync<MTag>(i => i.Name.ToLower() == tag.Name.ToLower());

                if (originTag == null)
                {
                    originTag = new MTag(tag);
                    await _context.Set<MTag>().AddAsync(originTag);
                }

                if (media.MediaTags.Any(i => i.MTag.Name.ToLower() == originTag.Name.ToLower()))
                {
                    continue;
                }

                media.MediaTags.Add(new MediaTag
                {
                    MTag = originTag
                });
            }

            if (saveChanges)
            {
                await UpdateAsync(media);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Entities.UserModel;
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
            List<string> nameList = new List<string>();
            List<MTag> tagList = new List<MTag>();
            for (int i = 0; i < tags.Count(); i++)
            {
                nameList.Add(tags[i].Name.ToLower());
            }
            foreach (var mediaTag in media.MediaTags)
            {
                if (nameList.Contains(mediaTag.MTag.Name.ToLower()))
                {
                    tags.ToList().RemoveAll(x => x.Name == mediaTag.MTag.Name.ToLower());
                }
                else
                {
                    var originTag = await GetAsync<MTag>(i => i.Name.ToLower() == mediaTag.MTag.Name.ToLower());
                    if (originTag.MediaTags.Count <= 1)
                    {
                        tagList.Add(mediaTag.MTag);
                    }
                    _context.Set<MediaTag>().Remove(mediaTag);
                }
            }

            foreach (var tag in tagList)
            {
                await DeleteAsync(tag);
            }

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


        public Task AddLikeAsync(Media media, AppUser user)
        {
            if (media == null || user == null)
            {
                return Task.CompletedTask;
            }

            if (media.LikesCount.All(i => i.UserId != user.Id && i.MediaId == media.Id))
            {
                media.LikesCount.Add(new MediaLike()
                {
                    Media = media,
                    AppUser = user
                });
            }

            return UpdateAsync(media);
        }

        public Task RemoveLikeAsync(Media media, AppUser user)
        {
            if (media == null || user == null)
            {
                return Task.CompletedTask;
            }

            var blogLike = _context.Set<MediaLike>().FirstOrDefault(i => i.UserId == user.Id && i.MediaId == media.Id);

            if (blogLike == null)
                return Task.CompletedTask;

            media.LikesCount.Remove(blogLike);
            return UpdateAsync(media);
        }

    }
}

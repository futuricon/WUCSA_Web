using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Gallery
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGalleryRepository _galleryRepository;

        public IndexModel(UserManager<AppUser> userManager, 
            IGalleryRepository galleryRepository)
        {
            _userManager = userManager;
            _galleryRepository = galleryRepository;
        }

        public PaginatedList<Core.Entities.GalleryModel.Media> Medias { get; set; }
        public string[] PopularTags { get; set; }
        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortBy, string tag, int pageIndex = 1)
        {
            ViewData["tagData"] = tag;
            ViewData["sortByData"] = sortBy;
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var medias = await GetSortedAsync(sortBy);

            if (tag != null)
            {
                var taggedMedias = medias.SelectMany(i => i.MediaTags).Where(i => i.MTag.Name.ToLower() == tag.ToLower())
                    .OrderByDescending(i => i.Media.PostedDate).Select(i => i.Media);

                Medias = PaginatedList<Core.Entities.GalleryModel.Media>.Create(taggedMedias, pageIndex, 12);
            }
            else
            {
                Medias = PaginatedList<Core.Entities.GalleryModel.Media>.Create(medias, pageIndex, 12);
            }

            PopularTags = await GetPopularTagsAsync(medias);
            return Page();
        }

        public async Task<IActionResult> OnPostLikeAsync(string mediaId)
        {
            var media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(mediaId);
            var user = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(mediaId) || media == null || User.Identity.IsAuthenticated == false)
            {
                return BadRequest($"Something went wrong!");
            }

            var alreadyLiked = media.LikesCount.Any(i => i.UserId == user.Id);

            if (alreadyLiked)
            {
                await _galleryRepository.RemoveLikeAsync(media, user);
            }
            else
            {
                await _galleryRepository.AddLikeAsync(media, user);
            }

            return new OkObjectResult(media.LikesCount.Count);
        }

        public Task<string[]> GetPopularTagsAsync(IEnumerable<Core.Entities.GalleryModel.Media> medias)
        {
            return Task.Run(() =>
            {
                var tags = new List<string>();

                foreach (var media in medias)
                {
                    tags.AddRange(media.MediaTags.Select(i => i.MTag.Name));
                }

                var popularTags = tags.GroupBy(str => str)
                    .Select(i => new { Name = i.Key, Count = i.Count() })
                    .OrderByDescending(k => k.Count).Select(i => i.Name).Take(12).ToArray();

                return popularTags;
            });
        }

        private async Task<IEnumerable<Core.Entities.GalleryModel.Media>> GetSortedAsync(string sortBy)
        {
            IEnumerable<Core.Entities.GalleryModel.Media> medias;
            if (sortBy != null)
            {
                switch (sortBy.ToLower())
                {
                    case "popular":
                        medias = (await _galleryRepository.GetListAsync<Core.Entities.GalleryModel.Media>()).Where(i => i.IsDeleted == false).OrderByDescending(i => i.LikesCount);
                        break;
                    case "video":
                        medias = (await _galleryRepository.GetListAsync<Core.Entities.GalleryModel.Media>()).Where(i => i.IsDeleted == false && i.MediaType.ToString().ToLower() == "video");
                        break;
                    case "images":
                        medias = (await _galleryRepository.GetListAsync<Core.Entities.GalleryModel.Media>()).Where(i => i.IsDeleted == false && i.MediaType.ToString().ToLower() == "image");
                        break;
                    default:
                        medias = (await _galleryRepository.GetListAsync<Core.Entities.GalleryModel.Media>()).Where(i => i.IsDeleted == false).OrderByDescending(i => i.PostedDate);
                        break;
                }
            }
            else
            {
                medias = (await _galleryRepository.GetListAsync<Core.Entities.GalleryModel.Media>()).Where(i => i.IsDeleted == false).OrderByDescending(i => i.PostedDate);
            }
            return medias;
        }
    }
}

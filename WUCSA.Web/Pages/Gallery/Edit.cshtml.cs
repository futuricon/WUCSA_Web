using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IGalleryRepository _galleryRepository;

        public EditModel(ImageHelper imageHelper, IGalleryRepository galleryRepository)
        {
            _imageHelper = imageHelper;
            _galleryRepository = galleryRepository;
        }

        public class InputModel
        {
            public Core.Entities.GalleryModel.Media Media { get; set; }
            public IFormFile UploadPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(id);

            if (!User.IsInRole("SuperAdmin"))
            {
                if (media.IsDeleted)
                {
                    return NotFound();
                }
            }
            Input = new InputModel()
            {
                Media = media,
                Tags = MTag.JoinTags(media.MediaTags.Select(i => i.MTag))
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(Input.Media.Id);
            var tags = MTag.ParseTags(Input.Tags);

            if (Input.UploadPhoto != null)
            {
                Input.Media.MediaPath = _imageHelper.UploadCoverImage(Input.UploadPhoto, $"{Input.Media.Id}_media", "gallery_imgs");
                Input.Media.MediaType = Core.Entities.GalleryModel.MediaType.Image;
            }
            else
            {
                _imageHelper.DeleteFile(media.MediaPath);
                Input.Media.MediaPath = Input.Media.MediaPath.Split('/').TakeLast(1).FirstOrDefault();
                Input.Media.MediaType = Core.Entities.GalleryModel.MediaType.Video;
            }

            await _galleryRepository.UpdateTagsAsync(media, false, tags);
            await _galleryRepository.UpdateMediaAsync(media);
            return RedirectToPage("/Gallery/Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IGalleryRepository _galleryRepository;

        public DeleteModel(ImageHelper imageHelper, IGalleryRepository galleryRepository)
        {
            _imageHelper = imageHelper;
            _galleryRepository = galleryRepository;
        }

        [BindProperty]
        public Core.Entities.GalleryModel.Media Media { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(id);
            Tags = MTag.JoinTags(Media.MediaTags.Select(i => i.MTag));

            if (Media == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("SuperAdmin"))
            {
                if (Media.IsDeleted)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(id);

            if (User.IsInRole("SuperAdmin"))
            {
                if (Media != null)
                {
                    await _galleryRepository.DeleteMediaAsync(Media);
                    _imageHelper.DeleteFile(Media.MediaPath);
                }
            }
            else
            {
                Media.IsDeleted = true;
            }
            return RedirectToPage("/Gallery/Index");
        }
    }
}

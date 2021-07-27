using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Gallery
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ImageHelper _imageHelper;
        private readonly IGalleryRepository _galleryRepository;

        public CreateModel(UserManager<AppUser> userManager,
            ImageHelper imageHelper, IGalleryRepository galleryRepository)
        {
            _userManager = userManager;
            _imageHelper = imageHelper;
            _galleryRepository = galleryRepository;
        }

        public class InputModel
        {
            public Media Media { get; set; }
            public IFormFile UploadPhoto { get; set; }
            public string Tags { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            AppUser currentUser = await _userManager.GetUserAsync(User);
            MTag[] tags = MTag.ParseTags(Input.Tags);
            Input.Media.Author = currentUser;

            if (Input.UploadPhoto != null)
            {
                Input.Media.MediaPath = _imageHelper.UploadCoverImage(Input.UploadPhoto, $"{Input.Media.Id}_media", "gallery_imgs");
                Input.Media.MediaType = MediaType.Image;
            }
            else if (!String.IsNullOrWhiteSpace(Input.Media.MediaPath))
            {
                if (Input.Media.MediaPath.Contains("https://youtu.be/")) // YouTube Video
                {
                    Input.Media.MediaPath = Input.Media.MediaPath.Split('/').TakeLast(1).FirstOrDefault();
                    Input.Media.MediaType = MediaType.Video;
                }
                else if (Input.Media.MediaPath.Contains("https://live.staticflickr.com/")) // Flickr Image
                {
                    var halfString = Input.Media.MediaPath.Split("https://live.staticflickr.com/").TakeLast(1).FirstOrDefault();
                    Input.Media.MediaPath = "https://live.staticflickr.com/" + halfString.Split("\"").FirstOrDefault();
                    Input.Media.MediaType = MediaType.Image;
                }
            }
            else
            {
                return Page();
            }

            await _galleryRepository.UpdateTagsAsync(Input.Media, false, tags);
            await _galleryRepository.AddMediaAsync(Input.Media);
            return RedirectToPage("/Gallery/Index");
        }
    }
}

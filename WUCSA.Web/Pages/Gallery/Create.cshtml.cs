using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            public Core.Entities.GalleryModel.Media Media { get; set; }
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
                Input.Media.MediaType = Core.Entities.GalleryModel.MediaType.Image;
            }
            else
            {
                Input.Media.MediaPath = Input.Media.MediaPath.Split('/').TakeLast(1).FirstOrDefault();
                Input.Media.MediaType = Core.Entities.GalleryModel.MediaType.Video;
            }

            await _galleryRepository.UpdateTagsAsync(Input.Media, false, tags);
            await _galleryRepository.AddMediaAsync(Input.Media);
            return RedirectToPage("/Gallery/Index");
        }
    }
}

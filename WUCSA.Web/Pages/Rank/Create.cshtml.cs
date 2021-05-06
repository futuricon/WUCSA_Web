using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Rank
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public CreateModel(UserManager<AppUser> userManager, 
            IRankRepository rankRepository, PDFFileHelper pdfFileHelper)
        {
            _rankRepository = rankRepository;
            _userManager = userManager;
            _pdfFileHelper = pdfFileHelper;
        }

        public IEnumerable<Core.Entities.RankModel.SportType> SportTypes { get; set; }
        public SelectList Options { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>();
            Options = new SelectList(SportTypes, nameof(Core.Entities.RankModel.SportType.Id), nameof(Core.Entities.RankModel.SportType.Name));
            return Page();
        }

        public class InputModel
        {
            public Core.Entities.RankModel.Rank Rank { get; set; }
            public IFormFile UploadPdf { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            var tempSlug = $"{Input.Rank.Title.ToString()}_{Input.Rank.RankDate.ToString("yyyy_MM_dd")}";
            Input.Rank.Slug = tempSlug.Slugify();
            if (Input.UploadPdf.Length > 0)
            {
                Input.Rank.RankPartsFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdf, Input.Rank.Slug, "ranks");
            }
            Input.Rank.Author = currentUser;

            await _rankRepository.AddRankAsync(Input.Rank);
            return RedirectToPage("/Rank/Index", new { slug = Input.Rank.Slug });
        }
    }
}

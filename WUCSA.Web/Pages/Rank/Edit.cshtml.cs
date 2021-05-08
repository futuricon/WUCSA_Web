using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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
    public class EditModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public EditModel(UserManager<AppUser> userManager,
            IRankRepository rankRepository, PDFFileHelper pdfFileHelper)
        {
            _rankRepository = rankRepository;
            _userManager = userManager;
            _pdfFileHelper = pdfFileHelper;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.Rank Rank { get; set; }
            public IFormFile UploadPdf { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string RCName { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please select kind of sport")]
        public string SelectedSTypeId { set; get; }
        public List<SelectListItem> Options { set; get; }
        public string rankSportType { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            await GetOptionAsync();
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);
            if (rank.RankPartsFilePath != null)
            {
                ViewData["PDFFilePath"] = rank.RankPartsFilePath;
            }
            Input = new InputModel()
            {
                Rank = rank
            };

            switch (RCName)
            {
                case "ru":
                    rankSportType = rank.SportType.NameRu;
                    break;
                case "uz":
                    rankSportType = rank.SportType.NameUz;
                    break;
                default:
                    rankSportType = rank.SportType.Name;
                    break;
            }
            SelectedSTypeId = rank.SportType.Id;

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await GetOptionAsync();
                return Page();
            }
            var sportType = await _rankRepository.GetAsync<Core.Entities.RankModel.SportType>(i => i.Id == SelectedSTypeId);
            if (sportType == null)
            {
                await GetOptionAsync();
                return Page();
            }
            Input.Rank.SportType = sportType;

            var tempSlug = $"{Input.Rank.RankLocation.ToString()}-{Input.Rank.SportType.Name.ToString()}-{Input.Rank.RankDate.ToString("yyyy-MM-dd")}";
            
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(Input.Rank.Id);
            rank.Title = Input.Rank.Title;
            rank.TitleRu = Input.Rank.TitleRu;
            rank.TitleUz = Input.Rank.TitleUz;
            rank.Description = Input.Rank.Description;
            rank.DescriptionRu = Input.Rank.DescriptionRu;
            rank.DescriptionUz = Input.Rank.DescriptionUz;
            rank.RankLocation = Input.Rank.RankLocation;
            rank.RankDate = Input.Rank.RankDate;
            rank.Slug = tempSlug.Slugify();

            if (Input.UploadPdf != null)
            {
                if (rank.RankPartsFilePath != null)
                {
                    _pdfFileHelper.DeleteFile(rank.RankPartsFilePath, "ranks");
                }
                rank.RankPartsFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdf, rank.Slug, "ranks");
            }

            await _rankRepository.UpdateRankAsync(rank);
            return RedirectToPage("/Rank/SubList", new { loc = Input.Rank.RankLocation.ToString().ToLower(), stype = Input.Rank.SportType.Name.ToLower() });
        }
        private async Task GetOptionAsync()
        {
            var SportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>();

            switch (RCName)
            {
                case "ru":
                    Options = SportTypes.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.NameRu
                    }).ToList(); ;
                    break;
                case "uz":
                    Options = SportTypes.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.NameUz
                    }).ToList(); ;
                    break;
                default:
                    Options = SportTypes.Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    }).ToList(); ;
                    break;
            }
        }
    }
}

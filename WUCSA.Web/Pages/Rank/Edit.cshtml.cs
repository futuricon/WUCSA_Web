using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Rank
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly IRankRepository _rankRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public EditModel(IRankRepository rankRepository, PDFFileHelper pdfFileHelper)
        {
            _rankRepository = rankRepository;
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
        [Required(ErrorMessage = "Please select Ranking List")]
        public string[] SelectedStypesId { get; set; }
        public List<Complex> SportTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            await GetOptionAsync();
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);

            if (!User.IsInRole("SuperAdmin"))
            {
                if (rank.IsDeleted)
                {
                    return NotFound();
                }
            }

            if (rank.RankPartsFilePath != null)
            {
                ViewData["PDFFilePath"] = rank.RankPartsFilePath;
            }

            Input = new InputModel()
            {
                Rank = rank
            };

            SelectedStypesId = rank.RankSportTypes.Where(i => i.SportType.IsDeleted == false).Select(i => i.SportType.Id).ToArray();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await GetOptionAsync();
                return Page();
            }

            var tempSlug = $"{Input.Rank.RankLocation}-{Input.Rank.Title}-{Input.Rank.RankDate:yyyy-MM-dd}";
            
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

            await _rankRepository.UpdateSportTypesAsync(rank, true, SelectedStypesId);
            await _rankRepository.UpdateRankAsync(rank);
            return RedirectToPage("/Rank/List", new { loc = Input.Rank.RankLocation.ToString().ToLower(), gender = "man" });
        }

        private async Task GetOptionAsync()
        {
            var sportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>(i => i.IsDeleted == false);
            SportTypes = new Complex().GetData(sportTypes, RCName);
        }

        public class Complex
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<Complex> GetData(List<Core.Entities.RankModel.SportType> SportTypes, string rcName)
            {
                List<Complex> data = new List<Complex>();
                switch (rcName)
                {
                    case "ru":
                        foreach (var sportType in SportTypes)
                        {
                            data.Add(new Complex() { Id = sportType.Id, Name = sportType.NameRu });
                        }
                        break;
                    case "uz":
                        foreach (var sportType in SportTypes)
                        {
                            data.Add(new Complex() { Id = sportType.Id, Name = sportType.NameUz });
                        }
                        break;
                    default:
                        foreach (var sportType in SportTypes)
                        {
                            data.Add(new Complex() { Id = sportType.Id, Name = sportType.Name });
                        }
                        break;
                }

                return data;
            }
        }
    }
}

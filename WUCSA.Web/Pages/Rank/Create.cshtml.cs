using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            _userManager = userManager;
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

        [BindProperty]
        [Required(ErrorMessage = "Please select Types of Sport List")]
        public string[] SelectedStypesId { get; set; }
        public List<Complex> SportTypes { get; set; }

        //[Required(ErrorMessage = "Please select location")]
        //public string SelectedLocation { set; get; }
        //public List<SelectListItem> LocationOptions { get; set; }

        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            await GetOptionAsync();
            //GetLocationOptions("world");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await GetOptionAsync();
                //GetLocationOptions("world");
                return Page();
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);

            var tempSlug = $"{Input.Rank.RankLocation}-{Input.Rank.RankDate:yyyy-MM-dd}";
            Input.Rank.Slug = tempSlug.Slugify();

            if (Input.UploadPdf != null)
            {
                Input.Rank.RankPartsFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdf, Input.Rank.Slug, "ranks");
            }

            Input.Rank.Author = currentUser;
            await _rankRepository.UpdateSportTypesAsync(Input.Rank, false, SelectedStypesId);
            await _rankRepository.AddRankAsync(Input.Rank);
            return RedirectToPage("/Rank/List", new { loc = Input.Rank.RankLocation, gender = "Man"});
        }

        //private void GetLocationOptions(string location)
        //{
        //    var Location = Core.Entities.RankModel.RankLocation.World;
        //    if (Core.Entities.RankModel.RankLocation.National.ToString().ToLower() == location.ToLower())
        //    {
        //        Location = Core.Entities.RankModel.RankLocation.National;
        //    }
        //    LocationOptions = RCName switch
        //    {
        //        "ru" => new List<SelectListItem>
        //            {
        //                new SelectListItem{ Value = "Word",  Text = "Мировой"},
        //                new SelectListItem { Value = "National", Text = "Национальный" }
        //            },
        //        "uz" => new List<SelectListItem>
        //            {
        //                new SelectListItem{ Value = "Word",  Text = "Jaxon"},
        //                new SelectListItem { Value = "National", Text = "Milliy" }
        //            },
        //        _ => new List<SelectListItem>
        //            {
        //                new SelectListItem{ Value = "Word",  Text = "Word"},
        //                new SelectListItem { Value = "National", Text = "National" }
        //            },
        //    };
        //    SelectedLocation = Location.ToString();
        //}

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

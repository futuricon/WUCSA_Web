using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Core.Entities.Base;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WUCSA.Web.Pages.Rank
{
    public class ListModel : PageModel
    {
        private readonly IRankRepository _rankRepository;

        public ListModel(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }

        public PaginatedList<Core.Entities.RankModel.Rank> Ranks { get; set; }
        public string RCName { get; set; }
        public string BasePath { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Please select gender")]
        public string SelectedGender { set; get; }
        public List<SelectListItem> GenderOptions { get; set; }

        
        [Required(ErrorMessage = "Please select location")]
        public string SelectedLocation { set; get; }
        public List<SelectListItem> LocationOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(string loc, string gender, int pageIndex = 1)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            
            var rankLoc = RouteData.Values["loc"].ToString().ToLower();
            var rankGen = RouteData.Values["gender"].ToString().ToLower();
            BasePath = $"list/{rankLoc}/{rankGen}";
            GetGenderOptions(rankGen);
            GetLocationOptions(rankLoc);

            var ranks = (await _rankRepository.GetListAsync<Core.Entities.RankModel.Rank>())
                .Where(i => i.RankLocation.ToString().ToLower() == rankLoc
                && i.RankParticipants.Where(z => z.Gender.ToString().ToLower() == rankGen) != null
                && i.IsDeleted == false)
                .OrderByDescending(i => i.RankDate); // debug this !!!

            Ranks = PaginatedList<Core.Entities.RankModel.Rank>.Create(ranks, pageIndex, 6);
            ViewData["RankListUrl"] = $"http://wucsa.com/rank/list/{loc}/{gender}";
            ViewData["ThisLocation"] = rankLoc;
            ViewData["ThisGender"] = rankGen;
            return Page();
        }
        
        private void GetGenderOptions(string gender)
        {
            var Gender = Core.Entities.RankModel.Gender.Man;
            if (Core.Entities.RankModel.Gender.Woman.ToString().ToLower() == gender.ToLower())
            {
                Gender = Core.Entities.RankModel.Gender.Woman;
            }
            GenderOptions = RCName switch
            {
                "ru" => new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Man",  Text = "Мужчины"},
                        new SelectListItem { Value = "Woman", Text = "Женщины" }
                    },
                "uz" => new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Man",  Text = "Erkaklar"},
                        new SelectListItem { Value = "Woman", Text = "Ayollar" }
                    },
                _ => new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Man",  Text = "Men"},
                        new SelectListItem { Value = "Woman", Text = "Women" }
                    },
            };
            SelectedGender = Gender.ToString();
        }

        private void GetLocationOptions(string location)
        {
            var Location = Core.Entities.RankModel.RankLocation.World;
            if (Core.Entities.RankModel.RankLocation.National.ToString().ToLower() == location.ToLower())
            {
                Location = Core.Entities.RankModel.RankLocation.National;
            }
            LocationOptions = RCName switch
            {
                "ru" => new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Word",  Text = "Мировой"},
                        new SelectListItem { Value = "National", Text = "Национальный" }
                    },
                "uz" => new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Word",  Text = "Jaxon"},
                        new SelectListItem { Value = "National", Text = "Milliy" }
                    },
                _ => new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Word",  Text = "Word"},
                        new SelectListItem { Value = "National", Text = "National" }
                    },
            };
            SelectedLocation = Location.ToString();
        }
    }
}

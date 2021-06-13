using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Rank
{
    public class IndexModel : PageModel
    {
        private readonly IRankRepository _rankRepository;

        public IndexModel(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }

        public string RCName { get; set; }
        public Core.Entities.RankModel.Rank Rank { get; set; }
        public List<Core.Entities.RankModel.RankParticipant> RankParticipants { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            var gender = RouteData.Values["gender"].ToString();
            var loc = RouteData.Values["loc"].ToString();
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            ViewData["RankUrl"] = $"http://wucsa.net/staff/{loc}/{gender}/{slug}";
            Rank = await _rankRepository.GetAsync<Core.Entities.RankModel.Rank>(i => i.Slug == slug && i.IsDeleted == false);
            if (Rank == null) { return NotFound(); }

            var rankParticipants = Rank.RankParticipants.Where(i => i.IsDeleted == false && i.Gender.ToString().ToLower() == gender.ToLower()).ToList();

            if (rankParticipants.Count > 0)
            {
                RankParticipants = rankParticipants.OrderByDescending(x => x.Weight).ToList();
            }
            if (Rank.RankPartsFilePath != null)
            {
                ViewData["PDFFilePath"] = Rank.RankPartsFilePath;
            }

            switch (RCName.ToLower())
            {
                case "ru":
                    ViewData["RankDescription"] = string.Concat(Rank.DescriptionRu.Take(200));
                    ViewData["RankTitle"] = Rank.TitleRu;
                    break;
                case "uz":
                    ViewData["RankDescription"] = string.Concat(Rank.DescriptionUz.Take(200));
                    ViewData["RankTitle"] = Rank.TitleUz;
                    break;
                default:
                    ViewData["RankDescription"] = string.Concat(Rank.Description.Take(200));
                    ViewData["RankTitle"] = Rank.Title;
                    break;
            }
            return Page();
        }
    }
}

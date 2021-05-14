using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Rank
{
    public class ListModel : PageModel
    {
        private readonly IRankRepository _rankRepository;

        public ListModel(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }
        public ICollection<Core.Entities.RankModel.SportType> SportTypes { get; set; }
        public int WorldRanksCount { get; set; }
        public int NationalRanksCount { get; set; }
        public string RCName { get; set; }

        public IActionResult OnGet()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var ranks = _rankRepository.GetAll<Core.Entities.RankModel.Rank>().Where(i => i.IsDeleted == false).ToList();
            WorldRanksCount = ranks.Where(x => x.RankLocation == Core.Entities.RankModel.RankLocation.World).Count();
            NationalRanksCount = ranks.Where(x => x.RankLocation == Core.Entities.RankModel.RankLocation.National).Count();
            SportTypes = _rankRepository.GetAll<Core.Entities.RankModel.SportType>().Where(i=>i.IsDeleted == false).ToList();
            return Page();
        }
    }
}

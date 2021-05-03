using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Rank
{
    public class ListModel : PageModel
    {
        private readonly IRankRepository _rankRepository;
        private readonly ISportTypeRepository _sportTypeRepository;

        public ListModel(IRankRepository rankRepository, ISportTypeRepository sportTypeRepository)
        {
            _rankRepository = rankRepository;
            _sportTypeRepository = sportTypeRepository;
        }
        public ICollection<Core.Entities.RankModel.SportType> SportTypes { get; set; }
        public int WorldRanksCount { get; set; }
        public int NationalRanksCount { get; set; }

        public IActionResult OnGet()
        {
            var ranks = _rankRepository.GetAll<Core.Entities.RankModel.Rank>().ToList();
            WorldRanksCount = ranks.Where(x => x.RankLocation == Core.Entities.RankModel.RankLocation.World).Count();
            NationalRanksCount = ranks.Where(x => x.RankLocation == Core.Entities.RankModel.RankLocation.National).Count();
            SportTypes = _sportTypeRepository.GetAll<Core.Entities.RankModel.SportType>().ToList();
            return Page();
        }
    }
}

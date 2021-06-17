using System.Linq;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Core.Entities.Base;
using System.Threading.Tasks;

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
        public string SetClass { get; set; }
        
        public async Task<IActionResult> OnGetAsync(string location, int pageIndex = 1)
        {
            SetClass = location.ToLower();
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            
            BasePath = $"./{location}";

            var ranks = (await _rankRepository.GetListAsync<Core.Entities.RankModel.Rank>())
                .Where(i => i.RankLocation.ToString().ToLower() == location.ToLower()
                && i.IsDeleted == false)
                .OrderByDescending(i => i.RankDate); // debug this !!!

            Ranks = PaginatedList<Core.Entities.RankModel.Rank>.Create(ranks, pageIndex, 6);
            ViewData["ThisLocation"] = location;
            return Page();
        }
    }
}

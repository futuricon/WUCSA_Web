using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Rank
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;

        public IndexModel(UserManager<AppUser> userManager, IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
            _userManager = userManager;
        }

        public PaginatedList<Core.Entities.RankModel.Rank> Ranks { get; set; }
        public string RCName { get; set; }
        public string BasePath { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var rankLoc = RouteData.Values["loc"].ToString();
            var rankSType = RouteData.Values["stype"].ToString();
            BasePath = $"{rankLoc}_{rankSType}";
            var ranks = (await _rankRepository.GetListAsync<Core.Entities.RankModel.Rank>())
                .Where(i => i.RankLocation.ToString().ToLower() == rankLoc.ToLower() 
                && i.SportType.Name.ToLower() == rankSType.ToLower()
                && i.IsDeleted != true)
                .OrderByDescending(i => i.RankDate);
            Ranks = PaginatedList<Core.Entities.RankModel.Rank>.Create(ranks, pageIndex, 6);
            return Page();
        }
    }
}

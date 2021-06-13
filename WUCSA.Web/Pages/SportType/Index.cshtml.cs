using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.SportType
{
    public class IndexModel : PageModel
    {
        private readonly IRankRepository _rankRepository;

        public IndexModel(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }

        public Core.Entities.RankModel.SportType SportType { get; set; }
        public string RCName { get; set; }

        public async Task OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var sportTypeSlug = RouteData.Values["slug"].ToString();
            SportType = await _rankRepository.GetAsync<Core.Entities.RankModel.SportType>(i => i.Slug == sportTypeSlug);

            switch (RCName.ToLower())
            {
                case "ru":
                    ViewData["SportName"] = SportType.NameRu;
                    ViewData["SportDescription"] = string.Concat(SportType.DescriptionRu.Take(200));
                    break;
                case "uz":
                    ViewData["SportName"] = SportType.NameUz;
                    ViewData["SportDescription"] = string.Concat(SportType.DescriptionUz.Take(200));
                    break;
                default:
                    ViewData["SportName"] = SportType.Name;
                    ViewData["SportDescription"] = string.Concat(SportType.Description.Take(200));
                    break;
            }
             
        }
    }
}

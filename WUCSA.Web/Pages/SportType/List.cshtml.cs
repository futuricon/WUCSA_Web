using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.SportType
{
    public class ListModel : PageModel
    {
        private readonly IRankRepository _rankRepository;

        public ListModel(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }
        public ICollection<Core.Entities.RankModel.SportType> SportTypes { get; set; }
        public string RCName { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            SportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>();
            return Page();
        }
    }
}

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
        private readonly ISportTypeRepository _sportTypeRepository;

        public ListModel(ISportTypeRepository sportTypeRepository)
        {
            _sportTypeRepository = sportTypeRepository;
        }
        public ICollection<Core.Entities.RankModel.SportType> SportTypes { get; set; }
        public string RCName { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            SportTypes = await _sportTypeRepository.GetListAsync<Core.Entities.RankModel.SportType>();
            return Page();
        }
    }
}

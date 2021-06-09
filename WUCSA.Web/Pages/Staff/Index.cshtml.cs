using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Staff
{
    public class IndexModel : PageModel
    {
        private readonly IStaffRepository _staffRepository;

        public IndexModel(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }
        public Core.Entities.StaffModel.Staff Staff { get; set; }
        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var staffSlug = RouteData.Values["slug"].ToString();
            Staff = await _staffRepository.GetAsync<Core.Entities.StaffModel.Staff>(i => i.Slug == staffSlug);

            switch (RCName.ToLower())
            {
                case "ru":
                    ViewData["StaffDescription"] = Staff.DescriptionRu;
                    ViewData["StaffPosition"] = Staff.PositionRu;
                    break;
                case "uz":
                    ViewData["StaffDescription"] = Staff.DescriptionUz;
                    ViewData["StaffPosition"] = Staff.PositionUz;
                    break;
                default:
                    ViewData["StaffDescription"] = Staff.Description;
                    ViewData["StaffPosition"] = Staff.Position;
                    break;
            }

            return Page();
        }
    }
}

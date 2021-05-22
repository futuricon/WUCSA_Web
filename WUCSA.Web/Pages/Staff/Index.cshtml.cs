using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Core.Entities.StaffModel.Staff> Staffs { get; set; }
        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var staffs = (await _staffRepository.GetListAsync<Core.Entities.StaffModel.Staff>()).Where(i => i.IsDeleted == false && i.IsMember == true);

            Staffs = staffs.OrderByDescending(i => i.Location).ToList();
            
            return Page();
        }
    }
}

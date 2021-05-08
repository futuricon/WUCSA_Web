using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.RankParticipant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;

        public CreateModel(UserManager<AppUser> userManager, IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
            _userManager = userManager;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.RankParticipant RankParticipant { get; set; }
            public Core.Entities.StaffModel.Participant Participant { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var x = await _rankRepository.GetListAsync<Core.Entities.RankModel.RankParticipant>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IList partsList)
        {

            return RedirectToPage();
        }
    }
}

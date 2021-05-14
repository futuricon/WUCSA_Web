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
    public class DeleteModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;

        public DeleteModel(UserManager<AppUser> userManager, IRankRepository rankRepository)
        {
            _userManager = userManager;
            _rankRepository = rankRepository;
        }

        [BindProperty]
        public Core.Entities.RankModel.RankParticipant RankParticipant { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RankParticipant = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(id);

            if (RankParticipant == null)
            {
                return NotFound();
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            var currentRole = await _userManager.GetRolesAsync(currentUser);

            if (!currentRole.Contains(Role.SuperAdmin.ToString()))
            {
                if (RankParticipant.IsDeleted)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RankParticipant = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(id);
            var prevPath = RankParticipant.Rank.Slug;
            AppUser currentUser = await _userManager.GetUserAsync(User);
            var currentRole = await _userManager.GetRolesAsync(currentUser);

            if (currentRole.Contains(Role.SuperAdmin.ToString()))
            {
                if (RankParticipant != null)
                {
                    await _rankRepository.DeleteRankParticipantAsync(RankParticipant);
                }
            }
            else
            {
                RankParticipant.IsDeleted = true;
            }
            return RedirectToPage($"/Rank/Index/{prevPath}/Man");
        }
    }
}

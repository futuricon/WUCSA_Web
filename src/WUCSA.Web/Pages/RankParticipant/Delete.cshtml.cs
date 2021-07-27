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
        private readonly IRankRepository _rankRepository;

        public DeleteModel(IRankRepository rankRepository)
        {
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

            if (!User.IsInRole("SuperAdmin"))
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
            var slug = RankParticipant.Rank.Slug;
            var location = RankParticipant.Rank.RankLocation.ToString();

            if (User.IsInRole("SuperAdmin"))
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
            return RedirectToPage($"/Rank/Index", new {location, slug });
        }
    }
}

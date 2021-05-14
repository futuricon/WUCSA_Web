using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.RankParticipant
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;
        public EditModel(UserManager<AppUser> userManager, IRankRepository rankRepository)
        {
            _userManager = userManager;
            _rankRepository = rankRepository;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.RankParticipant RankParticipant { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public Core.Entities.RankModel.Rank Rank { get; set; }

        public int PositionNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int Weight { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionUz { get; set; }

        public async Task<IActionResult> OnGetAsync(string rankId, string rankPartId)
        {
            if (rankId == null || rankPartId == null)
            {
                return NotFound();
            }

            Rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(rankId);

            AppUser currentUser = await _userManager.GetUserAsync(User);
            var currentRole = await _userManager.GetRolesAsync(currentUser);

            if (!currentRole.Contains(Role.SuperAdmin.ToString()))
            {
                if (Rank.IsDeleted)
                {
                    return NotFound();
                }
            }

            Input = new InputModel()
            {
                RankParticipant = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(rankPartId)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Participant")] List<Participant> model, string rankId, string rankPartId)
        {
            if (!ModelState.IsValid || model.Count == 0)
            {
                return Page();
            }
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(rankId);
            Input.RankParticipant.Rank = Rank;
            await _rankRepository.UpdateParticipantAsync(Input.RankParticipant, true, model.ToArray());
            return RedirectToPage("/Rank/Index", new { slug = Rank.Slug, gender = Input.RankParticipant.Gender.ToString(), Weight = Input.RankParticipant.Weight.ToString() });
        }
    }
}

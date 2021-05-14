using System;
using System.Collections;
using System.Collections.Generic;
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
        }

        [BindProperty]
        public InputModel Input { get; set; }

        private Core.Entities.RankModel.Rank Rank { get; set; }

        public int PositionNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int Weight { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionUz { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id != null)
            {
                Rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Model")] List<Participant> model, string id)
        {
            if (!ModelState.IsValid || model.Count == 0)
            {
                return Page();
            }
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);
            AppUser currentUser = await _userManager.GetUserAsync(User);
            Input.RankParticipant.Author = currentUser;
            Input.RankParticipant.Rank = Rank;
            await _rankRepository.AddRankParticipantAsync(Input.RankParticipant);
            await _rankRepository.UpdateParticipantAsync(Input.RankParticipant, true, model.ToArray());
            return RedirectToPage("/Rank/Index", new { slug = Rank.Slug, gender = Input.RankParticipant.Gender.ToString(), Weight = Input.RankParticipant.Weight.ToString()});
        }
    }
}

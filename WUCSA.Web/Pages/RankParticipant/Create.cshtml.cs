using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            _userManager = userManager;
            _rankRepository = rankRepository;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.RankParticipant RankParticipant { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public int PositionNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int Weight { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string DescriptionRu { get; set; }
        public string DescriptionUz { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select type of sport")]
        public string SelectedSTypeId { set; get; }
        public List<SelectListItem> OptionsRank { set; get; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);
            if (rank == null)
            {
                return NotFound();
            }
            ViewData["RankId"] = rank.Id;
            await GetOptionAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Model")] List<Participant> model, string rankId)
        {
            if (!ModelState.IsValid || model.Count == 0 || rankId == null)
            {
                await GetOptionAsync();
                return Page();
            }
            var sType = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.SportType>(SelectedSTypeId);
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(rankId);
            AppUser currentUser = await _userManager.GetUserAsync(User);
            Input.RankParticipant.Author = currentUser;
            Input.RankParticipant.SportType = sType;
            Input.RankParticipant.Rank = rank;
            await _rankRepository.AddRankParticipantAsync(Input.RankParticipant);
            await _rankRepository.UpdateParticipantAsync(Input.RankParticipant, true, model.ToArray());
            return RedirectToPage("/Rank/Index", new {location = rank.RankLocation.ToString(), slug = rank.Slug });
        }

        private async Task GetOptionAsync()
        {
            var sportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>(i => i.IsDeleted == false);

            OptionsRank = sportTypes.Select(a => new SelectListItem
            {
                Value = a.Id,
                Text = a.Name
            }).ToList();
        }
    }
}

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
    public class EditModel : PageModel
    {
        private readonly IRankRepository _rankRepository;
        public EditModel(IRankRepository rankRepository)
        {
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

        public async Task<IActionResult> OnGetAsync(string rankId, string rankPartId)
        {
            if (rankId == null || rankPartId == null)
            {
                return NotFound();
            }

            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(rankId);

            if (!User.IsInRole("SuperAdmin"))
            {
                if (rank.IsDeleted)
                {
                    return NotFound();
                }
            }

            await GetOptionAsync();
            ViewData["RankId"] = rank.Id;
            var rankPart = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(rankPartId);
            SelectedSTypeId = rankPart.SportType.Id;
            Input = new InputModel()
            {
                RankParticipant = rankPart
            };
            return Page();
        }

        /// <summary>
        /// Fix Editing !!!
        /// </summary>
        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Model")] List<Participant> model, string rankId)
        {
            if (!ModelState.IsValid || model.Count == 0)
            {
                return Page();
            }
            var rankPart = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(Input.RankParticipant.Id);
            var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(rankId);
            var sType = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.SportType>(SelectedSTypeId);
            rankPart.Rank = rank;
            rankPart.SportType = sType;
            rankPart.Weight = Input.RankParticipant.Weight;
            rankPart.Gender = Input.RankParticipant.Gender;

            await _rankRepository.UpdateParticipantAsync(rankPart, true, model.ToArray());
            return RedirectToPage("/Rank/Index", new { Location = rank.RankLocation.ToString(), slug = rank.Slug });
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

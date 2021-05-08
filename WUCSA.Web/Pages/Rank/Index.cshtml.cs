using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Rank
{
    public class IndexModel : PageModel
    {
        private readonly IRankRepository _rankRepository;

        public IndexModel(IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.RankParticipant RankParticipant { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select weight")]
        public string SelectedWeightId { set; get; }
        public Core.Entities.RankModel.RankParticipant Participant { get; set; }
        public string RCName { get; set; }
        public string PrevPath { get; set; }
        public List<SelectListItem> WieghtOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug, string gender, string weight)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            //PrevPath = $"/Rank/SubList/{basePath}";
            var Participants = await _rankRepository.GetListAsync<Core.Entities.RankModel.RankParticipant>(i=> i.IsDeleted != true && i.Rank.Slug == slug);
            if (Participants != null)
            {
                Participant = Participants.Where(i => i.Gender == Core.Entities.RankModel.Gender.Man).OrderByDescending(x => x.Weight).FirstOrDefault();
            
                WieghtOptions = Participants.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Weight.ToString()
                }).ToList();
            }

            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        WieghtOptions = Participants.Select(a => new SelectListItem
        //        {
        //            Value = a.Id.ToString(),
        //            Text = a.Weight.ToString()
        //        }).ToList();
        //        return Page();
        //    }
        //    var sportType = await _rankRepository.GetAsync<Core.Entities.RankModel.SportType>(i => i.Id == SelectedSTypeId);
        //    if (sportType == null)
        //    {
        //        await GetOptionAsync();
        //        return Page();
        //    }
        //    Input.Rank.SportType = sportType;

        //    await _rankRepository.AddRankAsync(Input.Rank);
        //    return RedirectToPage("/Rank/SubList", new { loc = Input.Rank.RankLocation.ToString().ToLower(), stype = Input.Rank.SportType.Name.ToLower() });
        //}
    }
}

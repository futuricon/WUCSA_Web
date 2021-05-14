using System;
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

        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Please select weight")]
        public string SelectedWeightId { set; get; }
        public List<SelectListItem> WeightOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Please select gender")]
        public string SelectedGenderId { set; get; }
        public List<SelectListItem> GenderOptions { get; set; }

        public Core.Entities.RankModel.RankParticipant RankParticipant { get; set; }
        public Core.Entities.RankModel.Rank Rank { get; set; }
        public string RCName { get; set; }
        public string PrevPath { get; set; }
        private Core.Entities.RankModel.Gender Gender { get; set; }
        private List<Core.Entities.RankModel.RankParticipant> RankParticipants { get; set; }


        public async Task<IActionResult> OnGetAsync(string slug, string gender, string weight)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            GetGenderOptions(gender);

            Rank = await _rankRepository.GetAsync<Core.Entities.RankModel.Rank>(i => i.Slug == slug && i.IsDeleted == false);
            if (Rank == null)
            {
                return NotFound();
            }
            PrevPath = $"/Rank/SubList/{Rank.RankLocation}/{Rank.SportType.Name}";
            RankParticipants = Rank.RankParticipants.Where(i => i.IsDeleted == false && i.Gender == Gender).ToList();

            if (RankParticipants.Count > 0)
            {
                if (weight != null)
                {
                    RankParticipant = RankParticipants.Where(i => i.Weight == Int16.Parse(weight)).FirstOrDefault();
                    if (RankParticipant == null){ return NotFound(); }
                }
                else
                {
                    RankParticipant = RankParticipants.OrderByDescending(x => x.Weight).FirstOrDefault();
                }

                GetWeightOptions(weight);
            }
            if (Rank.RankPartsFilePath != null)
            {
                ViewData["PDFFilePath"] = Rank.RankPartsFilePath;
            }
            return Page();
        }

        private void GetGenderOptions(string gender)
        {
            Gender = Core.Entities.RankModel.Gender.Man;
            if (Core.Entities.RankModel.Gender.Woman.ToString().ToLower() == gender.ToLower())
            {
                Gender = Core.Entities.RankModel.Gender.Woman;
            }
            switch (RCName)
            {
                case "ru":
                    GenderOptions = new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Man",  Text = "Мужчины"},
                        new SelectListItem { Value = "Woman", Text = "Женщины" }
                    };
                    break;
                case "uz":
                    GenderOptions = new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Man",  Text = "Erkaklar"},
                        new SelectListItem { Value = "Woman", Text = "Ayollar" }
                    };
                    break;
                default:
                    GenderOptions = new List<SelectListItem>
                    {
                        new SelectListItem{ Value = "Man",  Text = "Men"},
                        new SelectListItem { Value = "Woman", Text = "Women" }
                    };
                    break;
            }

            SelectedGenderId = Gender.ToString();
        }

        private void GetWeightOptions(string weight)
        {

            WeightOptions = RankParticipants.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Weight.ToString()
            }).ToList();
            SelectedWeightId = RankParticipant.Id;
        }

    }
}

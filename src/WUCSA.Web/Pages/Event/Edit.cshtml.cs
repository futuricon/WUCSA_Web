using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Event
{
    public class EditModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEventRepository _eventRepository;
        private readonly IRankRepository _rankRepository;
        private readonly PDFFileHelper _pdfFileHelper;
        private readonly ImageHelper _imageHelper;

        public EditModel(UserManager<AppUser> userManager, ImageHelper imageHelper,
            IEventRepository eventRepository, IRankRepository rankRepository, PDFFileHelper pdfFileHelper)
        {
            _userManager = userManager;
            _imageHelper = imageHelper;
            _eventRepository = eventRepository;
            _rankRepository = rankRepository;
            _pdfFileHelper = pdfFileHelper;
        }

        public class InputModel
        {
            public Core.Entities.EventModel.Event Event { get; set; }
            public IFormFile UploadPdfRules { get; set; }
            public IFormFile UploadPdfParts { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string SelectedRankId { set; get; }
        public List<SelectListItem> OptionsRank { set; get; }

        [BindProperty]
        [Required(ErrorMessage = "Please select Sport Types")]
        public string[] SelectedStypesId { get; set; }
        public List<Complex> SportTypes { get; set; }

        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            await GetOptionAsync();
            var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(id);

            if (!User.IsInRole("SuperAdmin"))
            {
                if (myEvent.IsDeleted)
                {
                    return NotFound();
                }
            }

            Input = new InputModel()
            {
                Event = myEvent
            };

            SelectedStypesId = myEvent.EventSportTypes.Where(i=>i.SportType.IsDeleted == false).Select(i=>i.SportType.Id).ToArray();
            
            if (myEvent.Rank != null)
            {
                SelectedRankId = myEvent.Rank.Id;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            AppUser currentUser = await _userManager.GetUserAsync(User);
            var tempSlug = $"{Input.Event.EventLocation}-{Input.Event.Title}-{Input.Event.EventDate:yyyy-MM-dd}";

            var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(Input.Event.Id);
            var myRank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(SelectedRankId);

            if (myEvent == null)
            {
                return NotFound();
            }
           
            myEvent.Title = Input.Event.Title;
            myEvent.TitleRu = Input.Event.TitleRu;
            myEvent.TitleUz = Input.Event.TitleUz;
            myEvent.Description = Input.Event.Description;
            myEvent.DescriptionRu = Input.Event.DescriptionRu;
            myEvent.DescriptionUz = Input.Event.DescriptionUz;
            myEvent.EventLocation = Input.Event.EventLocation;
            myEvent.EventDate = Input.Event.EventDate;
            myEvent.EventEndDate = Input.Event.EventEndDate;
            myEvent.Location = Input.Event.Location;
            myEvent.EventPromoVideo = Input.Event.EventPromoVideo;
            myEvent.Author = currentUser;
            myEvent.Slug = tempSlug.Slugify();

            if (myRank != null)
            {
                myEvent.Rank = myRank;
            }
            
            if (Input.UploadCoverPhoto != null)
            {
                myEvent.CoverPhotoPath = _imageHelper.UploadCoverImage(Input.UploadCoverPhoto, Input.Event.Id, "event_imgs");
            }
            //if (Input.UploadPdfRules != null)
            //{
            //    myEvent.RulesFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdfRules, $"{Input.Event.Slug}-Rules", "events");
            //}
            //if (Input.UploadPdfParts != null)
            //{
            //    myEvent.EventPartsFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdfParts, $"{Input.Event.Slug}-Parts", "events");
            //}

            await _eventRepository.UpdateSportTypesAsync(myEvent, true, SelectedStypesId);
            await _eventRepository.UpdateEventAsync(myEvent);
            return RedirectToPage($"/Event/List", new { location = Input.Event.EventLocation });
        }

        private async Task GetOptionAsync()
        {
            var sportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>(i => i.IsDeleted == false);
            var Rank = await _rankRepository.GetListAsync<Core.Entities.RankModel.Rank>(i => i.IsDeleted == false);
            SportTypes = new Complex().GetData(sportTypes, RCName);
            OptionsRank = RCName switch
            {
                "ru" => Rank.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.TitleRu
                }).ToList(),
                "uz" => Rank.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.TitleUz
                }).ToList(),
                _ => Rank.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Title
                }).ToList(),
            };
        }

        public class Complex
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<Complex> GetData(List<Core.Entities.RankModel.SportType> SportTypes, string rcName)
            {
                List<Complex> data = new List<Complex>();
                switch (rcName)
                {
                    case "ru":
                        foreach (var sportType in SportTypes)
                        {
                            data.Add(new Complex() { Id = sportType.Id, Name = sportType.NameRu });
                        }
                        break;
                    case "uz":
                        foreach (var sportType in SportTypes)
                        {
                            data.Add(new Complex() { Id = sportType.Id, Name = sportType.NameUz });
                        }
                        break;
                    default:
                        foreach (var sportType in SportTypes)
                        {
                            data.Add(new Complex() { Id = sportType.Id, Name = sportType.Name });
                        }
                        break;
                }

                return data;
            }
        }
    }
}

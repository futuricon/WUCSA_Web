using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Event
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEventRepository _eventRepository;
        private readonly IRankRepository _rankRepository;
        private readonly PDFFileHelper _pdfFileHelper;
        private readonly ImageHelper _imageHelper;

        public CreateModel(UserManager<AppUser> userManager, ImageHelper imageHelper,
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

        public string TempCoverImagePath { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select Types of Sport List")]
        public string[] SelectedStypesId { get; set; }
        public List<Complex> SportTypes { get; set; }

        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            await GetOptionAsync();
            TempCoverImagePath = _imageHelper.GenerateImage(DateTime.Now.ToString("yyyy-MM-dd"), "event_imgs", "big");
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
            Input.Event.Slug = tempSlug.Slugify();

            if (Input.UploadCoverPhoto != null)
            {
                Input.Event.CoverPhotoPath = _imageHelper.UploadCoverImage(Input.UploadCoverPhoto, Input.Event.Id, "event_imgs");
            }
            //if (Input.UploadPdfRules != null)
            //{
            //    Input.Event.RulesFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdfRules, $"{Input.Event.Slug}-Rules", "events");
            //}
            //if (Input.UploadPdfParts != null)
            //{
            //    Input.Event.EventPartsFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdfParts, $"{Input.Event.Slug}-Parts", "events");
            //}

            Input.Event.Author = currentUser;
            await _eventRepository.UpdateSportTypesAsync(Input.Event, false, SelectedStypesId);
            await _eventRepository.AddEventAsync(Input.Event);
            return RedirectToPage($"/Event/Index", new { slug = Input.Event.Slug});
        }

        private async Task GetOptionAsync()
        {
            var sportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>(i => i.IsDeleted == false);
            SportTypes = new Complex().GetData(sportTypes, RCName);
        }

        public class Complex {
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

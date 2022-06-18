using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

namespace WUCSA.Web.Pages.EventFile
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public EditModel(IEventRepository eventRepository, PDFFileHelper pdfFileHelper)
        {
            _eventRepository = eventRepository;
            _pdfFileHelper = pdfFileHelper;
        }

        public class InputModel
        {
            public Core.Entities.EventModel.EventRelatedFile EventFile { get; set; }
            public string EventId { get; set; }
            public IFormFile UploadPdfParts { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;

            var myFile = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.EventRelatedFile>(id);

            if (myFile == null)
            {
                return NotFound();
            }

            Input = new InputModel()
            {
                EventFile = myFile,
                EventId = myFile.Event.Id
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(Input.EventId);
            if (myEvent == null) { return NotFound(); }

            var myFile = myEvent.EventRelatedFiles.Where(x=>x.Id == Input.EventFile.Id).FirstOrDefault();
            if (myFile == null) { return NotFound(); }

            myFile.Title = Input.EventFile.Title;
            myFile.TitleRu = Input.EventFile.TitleRu;
            myFile.TitleUz = Input.EventFile.TitleUz;
            myFile.Path = Input.EventFile.Path;
            myFile.OrderNumber = Input.EventFile.OrderNumber;

            if (Input.UploadPdfParts != null)
            {
                _pdfFileHelper.DeleteFile(Input.EventFile.Path, "events");
                myFile.Path = await _pdfFileHelper.SaveFile(Input.UploadPdfParts, $"{Input.EventFile.Event.Id}_{DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss")}", "events");
            }

            await _eventRepository.UpdateEventFileAsync(myEvent, myFile);

            return RedirectToPage("/EventFile/List", new { id = Input.EventId });
        }
    }
}

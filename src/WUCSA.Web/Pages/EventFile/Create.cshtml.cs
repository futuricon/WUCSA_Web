using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.EventFile
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public CreateModel(IEventRepository eventRepository, PDFFileHelper pdfFileHelper)
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


        public int OrderNumb { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(id);

            if (myEvent == null)
            {
                return NotFound();
            }

            Input = new InputModel()
            {
                EventId = myEvent.Id
            };

            var myEventFile = myEvent.EventRelatedFiles.OrderBy(x => x.OrderNumber).FirstOrDefault();
            
            OrderNumb = myEventFile != null ? myEventFile.OrderNumber + 1 : 1;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(Input.EventId);

            if (!ModelState.IsValid || myEvent == null)
            {
                return Page();
            }

            if (Input.UploadPdfParts != null)
            {
                Input.EventFile.Path = await _pdfFileHelper.SaveFile(Input.UploadPdfParts, $"{Input.EventId}_{DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss")}", "events");
            }

            await _eventRepository.UpdateEventFileAsync(myEvent, Input.EventFile);

            return RedirectToPage("/EventFile/List", new { id = Input.EventId });
        }
    }
}

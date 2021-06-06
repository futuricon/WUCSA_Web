using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Event
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly PDFFileHelper _pdfFileHelper;
        private readonly IEventRepository _eventRepository;

        public DeleteModel(ImageHelper imageHelper, PDFFileHelper pdfFileHelper, 
            IEventRepository eventRepository)
        {
            _imageHelper = imageHelper;
            _pdfFileHelper = pdfFileHelper;
            _eventRepository = eventRepository;
        }

        [BindProperty]
        public Core.Entities.EventModel.Event Event { get; set; }

        public string Tags { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(id);

            if (Event == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("SuperAdmin"))
            {
                if (Event.IsDeleted)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Event = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(id);

            if (User.IsInRole("SuperAdmin"))
            {
                if (Event != null)
                {
                    if (Event.CoverPhotoPath != null)
                    {
                        _imageHelper.RemoveImage(Event.CoverPhotoPath, "event_imgs");
                    }
                    if (Event.RulesFilePath != null)
                    {
                        _pdfFileHelper.DeleteFile(Event.RulesFilePath, "events");
                    }
                    if (Event.EventPartsFilePath != null)
                    {
                        _pdfFileHelper.DeleteFile(Event.EventPartsFilePath, "events");
                    }
                    await _eventRepository.DeleteEventAsync(Event);
                }
            }
            else
            {
                Event.IsDeleted = true;
            }
            return RedirectToPage("/Event/List", new { location = Event.EventLocation.ToString() });
        }
    }
}

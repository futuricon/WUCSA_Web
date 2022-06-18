using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.EventFile
{
    public class ListModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        public ListModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Core.Entities.EventModel.Event Event { get; set; }
        public List<Core.Entities.EventModel.EventRelatedFile> EventFiles { get; set; }
        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;

            Event = await _eventRepository.GetAsync<Core.Entities.EventModel.Event>(i => i.Id == id && i.IsDeleted == false);
            if (Event == null)
            {
                return NotFound();
            }

            EventFiles = Event.EventRelatedFiles.OrderBy(x => x.OrderNumber).ToList();

            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(string id)
        {
            var myFile = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.EventRelatedFile>(id);
            var myEvent = await _eventRepository.GetAsync<Core.Entities.EventModel.Event>(i => i.Id == myFile.Event.Id && i.IsDeleted == false);

            if (myFile != null && myEvent != null)
            {
                await _eventRepository.UpdateEventFileAsync(myEvent, myFile, true);
            }

            return RedirectToPage("/EventFile/List", new { id = myFile.Event.Id });
        }
    }
}

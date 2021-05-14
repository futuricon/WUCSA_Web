using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Event
{
    public class IndexModel : PageModel
    {
        private readonly IEventRepository _eventRepository;

        public IndexModel(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Core.Entities.EventModel.Event Event { get; set; }
        public List<Core.Entities.EventModel.Event> LatestEvents { get; set; }
        public string RCName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            var eventSlug = RouteData.Values["slug"].ToString();
            Event = await _eventRepository.GetAsync<Core.Entities.EventModel.Event>(i => i.Slug == eventSlug && i.IsDeleted == false);
            var latestEvents = await _eventRepository.GetListAsync<Core.Entities.EventModel.Event>(i => i.IsDeleted == false);
            LatestEvents = latestEvents.OrderByDescending(i => i.PostedDate).Take(5).ToList();
            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

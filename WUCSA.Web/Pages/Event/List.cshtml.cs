using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Event
{
    public class ListModel : PageModel
    {
        private readonly IEventRepository _eventRepository;
        private readonly IRankRepository _rankRepository;
        public ListModel(IEventRepository eventRepository, IRankRepository rankRepository)
        {
            _eventRepository = eventRepository;
            _rankRepository = rankRepository;
        }

        public PaginatedList<Core.Entities.EventModel.Event> Events { get; set; }
        public string RCName { get; set; }
        public string SetClass { get; set; }
        public List<Core.Entities.RankModel.SportType> SportTypes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(string location, string sTypeId, int pageIndex = 1)
        {
            SetClass = location.ToLower();
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            SportTypes = await _rankRepository.GetListAsync<Core.Entities.RankModel.SportType>(i => i.IsDeleted == false);
            var sortedEvents = (await _eventRepository.GetListAsync<Core.Entities.EventModel.Event>(i => i.IsDeleted == false)).Where(z => z.EventLocation.ToString().ToLower() == location.ToLower());

            if (sTypeId != null)
            {
                sortedEvents = sortedEvents.SelectMany(i => i.EventSportTypes).Where(z => z.SportType.Id == sTypeId).Select(i=>i.Event).ToList();
                Events = PaginatedList<Core.Entities.EventModel.Event>.Create(sortedEvents.OrderByDescending(i => i.EventDate), pageIndex, 6);
            }
            else
            {
                Events = PaginatedList<Core.Entities.EventModel.Event>.Create(sortedEvents.OrderByDescending(i => i.EventDate), pageIndex, 6);
            }
            return Page();
        }
    }
}

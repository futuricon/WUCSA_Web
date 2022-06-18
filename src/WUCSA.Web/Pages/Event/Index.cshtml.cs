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
        public List<Core.Entities.EventModel.EventRelatedFile> EventFiles { get; set; }
        public List<Core.Entities.EventModel.Event> LatestEvents { get; set; }
        public List<string> PromoVideoUrls { get; set; }
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

            EventFiles = Event.EventRelatedFiles.OrderBy(x=>x.OrderNumber).ToList();

            ViewData["KeyWords"] = string.Join(",", Event.EventSportTypes.Select(i=>i.SportType.Name).ToArray());

            switch (RCName.ToLower())
            {
                case "ru":
                    ViewData["EventTitle"] = Event.TitleRu;
                    ViewData["EventDescription"] = Core.Entities.EventModel.Event.GetShortContent(Event.DescriptionRu, 200);
                    break;
                case "uz":
                    ViewData["EventTitle"] = Event.TitleUz;
                    ViewData["EventDescription"] = Core.Entities.EventModel.Event.GetShortContent(Event.DescriptionUz, 200);
                    break;
                default:
                    ViewData["EventTitle"] = Event.Title;
                    ViewData["EventDescription"] = Core.Entities.EventModel.Event.GetShortContent(Event.Description, 200);
                    break;
            }
            if (Event.EventPromoVideo != null)
            {
                PromoVideoUrls = Event.EventPromoVideo.Split(',').ToList();
            }

            return Page();
        }
    }
}


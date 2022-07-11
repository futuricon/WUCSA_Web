using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBlogRepository _blogRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IStaffRepository _staffRepository;

        public IndexModel(ILogger<IndexModel> logger, IBlogRepository blogRepository,
            IEventRepository eventRepository, IStaffRepository staffRepository)
        {
            _logger = logger;
            _blogRepository = blogRepository;
            _eventRepository = eventRepository;
            _staffRepository = staffRepository;
        }

        public string RCName { get; set; }
        public List<Core.Entities.BlogModel.Blog> Blogs { get; set; }
        public Core.Entities.BlogModel.Blog History { get; set; }
        public List<Core.Entities.EventModel.Event> Events { get; set; }
        public List<Core.Entities.StaffModel.Staff> Staffs { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;

            History = await _blogRepository.GetAsync<Core.Entities.BlogModel.Blog>(i => i.IsDeleted == true && i.StaticPage == StaticPage.History);
            
            var blogs = _blogRepository.GetAll<Core.Entities.BlogModel.Blog>().Where(i => i.IsDeleted == false);
            Blogs = blogs.OrderByDescending(i => i.PostedDate).Take(3).ToList();

            var events = await _eventRepository.GetListAsync<Core.Entities.EventModel.Event>(i => i.IsDeleted == false);
            Events = events.OrderByDescending(i => i.EventDate).Take(3).ToList();

            var staffs = (await _staffRepository.GetListAsync<Core.Entities.StaffModel.Staff>()).Where(i => i.IsDeleted == false && i.IsMember == false);
            Staffs = staffs.OrderBy(i => i.OrderNumber).ToList();

            return Page();
        }
    }
}

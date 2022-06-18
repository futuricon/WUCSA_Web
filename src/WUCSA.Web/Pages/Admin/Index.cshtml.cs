using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly PDFFileHelper _pdfFileHelper;
        private readonly IBlogRepository _blogRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IGalleryRepository _galleryRepository;
        private readonly IRankRepository _rankRepository;
        private readonly IStaffRepository _staffRepository;

        public IndexModel(PDFFileHelper pdfFileHelper, ImageHelper imageHelper,
            IBlogRepository blogRepository, IEventRepository eventRepository,
            IGalleryRepository galleryRepository, IRankRepository rankRepository,
            IStaffRepository staffRepository)
        {
            _pdfFileHelper = pdfFileHelper;
            _imageHelper = imageHelper;
            _blogRepository = blogRepository;
            _eventRepository = eventRepository;
            _galleryRepository = galleryRepository;
            _rankRepository = rankRepository;
            _staffRepository = staffRepository;
        }

        public PaginatedList<Core.Entities.BlogModel.Blog> Blogs { get; set; }
        public PaginatedList<Core.Entities.BlogModel.Blog> StaticPages { get; set; }
        public PaginatedList<Core.Entities.EventModel.Event> Events { get; set; }
        public PaginatedList<Core.Entities.GalleryModel.Media> Medias { get; set; }
        public PaginatedList<Core.Entities.RankModel.Rank> Ranks { get; set; }
        public PaginatedList<Core.Entities.RankModel.RankParticipant> RankParts { get; set; }
        public PaginatedList<Core.Entities.RankModel.SportType> SportTypes { get; set; }
        public PaginatedList<Core.Entities.StaffModel.Staff> Staffs { get; set; }
        public string RCName { get; set; }

        public IActionResult OnGet(string mode, int pageIndex = 1)
        {
            RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;
            switch (mode.ToLower())
            {
                case "event":
                    var events = _eventRepository.GetAll<Core.Entities.EventModel.Event>().Where(i => i.IsDeleted == true);
                    Events = PaginatedList<Core.Entities.EventModel.Event>.Create(events.OrderByDescending(i => i.PostedDate), pageIndex, 50);
                    break;
                case "media":
                    var medias = _galleryRepository.GetAll<Core.Entities.GalleryModel.Media>().Where(i => i.IsDeleted == true);
                    Medias = PaginatedList<Core.Entities.GalleryModel.Media>.Create(medias.OrderByDescending(i => i.PostedDate), pageIndex, 50);
                    break;
                case "rank":
                    var ranks = _rankRepository.GetAll<Core.Entities.RankModel.Rank>().Where(i => i.IsDeleted == true);
                    Ranks = PaginatedList<Core.Entities.RankModel.Rank>.Create(ranks.OrderByDescending(i => i.RankDate), pageIndex, 50);
                    break;
                case "rank_part":
                    var rank_parts = _rankRepository.GetAll<Core.Entities.RankModel.RankParticipant>().Where(i => i.IsDeleted == true);
                    RankParts = PaginatedList<Core.Entities.RankModel.RankParticipant>.Create(rank_parts.OrderByDescending(i => i.Rank.RankDate), pageIndex, 50);
                    break;
                case "sport_type":
                    var sport_types = _rankRepository.GetAll<Core.Entities.RankModel.SportType>().Where(i => i.IsDeleted == true);
                    SportTypes = PaginatedList<Core.Entities.RankModel.SportType>.Create(sport_types.OrderByDescending(i => i.Name), pageIndex, 50);
                    break;
                case "staff":
                    var staffs = _staffRepository.GetAll<Core.Entities.StaffModel.Staff>().Where(i => i.IsDeleted == true);
                    Staffs = PaginatedList<Core.Entities.StaffModel.Staff>.Create(staffs.OrderByDescending(i => i.FirstName), pageIndex, 50);
                    break;
                case "static_page":
                    var staticPages = _blogRepository.GetAll<Core.Entities.BlogModel.Blog>().Where(i => i.IsDeleted == true && i.StaticPage > 0);
                    StaticPages = PaginatedList<Core.Entities.BlogModel.Blog>.Create(staticPages.OrderByDescending(i => i.PostedDate), pageIndex, 50);
                    break;
                default:
                    var blogs = _blogRepository.GetAll<Core.Entities.BlogModel.Blog>().Where(i => i.IsDeleted == true);
                    Blogs = PaginatedList<Core.Entities.BlogModel.Blog>.Create(blogs.OrderByDescending(i => i.PostedDate), pageIndex, 50);
                    break;
            }

            ViewData["mode"] = mode.ToLower();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteModeAsync(string id, string mode)
        {
            switch (mode.ToLower())
            {
                case "event":
                    var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(id);
                    if (myEvent != null)
                    {
                        if (myEvent.CoverPhotoPath != null)
                        {
                            _imageHelper.RemoveImage(myEvent.CoverPhotoPath, "event_imgs");
                        }
                        //if (myEvent.RulesFilePath != null)
                        //{
                        //    _pdfFileHelper.DeleteFile(myEvent.RulesFilePath, "events");
                        //}
                        //if (myEvent.EventPartsFilePath != null)
                        //{
                        //    _pdfFileHelper.DeleteFile(myEvent.EventPartsFilePath, "events");
                        //}
                        await _eventRepository.DeleteEventAsync(myEvent);
                    }
                    break;
                case "media":
                    var media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(id);
                    if (media != null)
                    {
                        _imageHelper.DeleteFile(media.MediaPath);
                        await _galleryRepository.DeleteMediaAsync(media);
                    }
                    break;
                case "rank":
                    var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);
                    if (rank != null)
                    {
                        if (rank.RankPartsFilePath != null)
                        {
                            _pdfFileHelper.DeleteFile(rank.RankPartsFilePath, "ranks");
                        }
                        await _rankRepository.DeleteRankAsync(rank);
                    }
                    break;
                case "rank_part":
                    var rank_part = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(id);
                    if (rank_part != null)
                    {
                        await _rankRepository.DeleteRankParticipantAsync(rank_part);
                    }
                    break;
                case "sport_type":
                    var sport_type = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.SportType>(id);
                    if (sport_type != null)
                    {
                        if (sport_type.RulesFilePath != null)
                        {
                            _pdfFileHelper.DeleteFile(sport_type.RulesFilePath, "sportTypes");
                        }
                        await _rankRepository.DeleteSportTypeAsync(sport_type);
                    }
                    break;
                case "staff":
                    var staff = await _staffRepository.GetByIdAsync<Core.Entities.StaffModel.Staff>(id);
                    if (staff != null)
                    {
                        _imageHelper.RemoveImage(staff.ProfilePhotoPath, "staff_imgs");
                        await _staffRepository.DeleteStaffAsync(staff);
                    }
                    break;
                default:
                    var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);
                    if (blog != null)
                    {
                        _imageHelper.RemoveImage(blog.CoverPhotoPath, "post_imgs");
                        await _blogRepository.DeleteBlogAsync(blog);
                    }
                    break;
            }

            return new OkObjectResult(id);
        }

        public async Task<IActionResult> OnPostRestoreModeAsync(string id, string mode)
        {
            switch (mode.ToLower())
            {
                case "event":
                    var myEvent = await _eventRepository.GetByIdAsync<Core.Entities.EventModel.Event>(id);
                    if (myEvent != null)
                    {
                        myEvent.IsDeleted = false;
                        await _eventRepository.UpdateEventAsync(myEvent);
                    }
                    break;
                case "media":
                    var media = await _galleryRepository.GetByIdAsync<Core.Entities.GalleryModel.Media>(id);
                    if (media != null)
                    {
                        media.IsDeleted = false;
                        await _galleryRepository.UpdateMediaAsync(media);
                    }
                    break;
                case "rank":
                    var rank = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.Rank>(id);
                    if (rank != null)
                    {
                        rank.IsDeleted = false;
                        await _rankRepository.UpdateRankAsync(rank);
                    }
                    break;
                case "rank_part":
                    var rank_part = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.RankParticipant>(id);
                    if (rank_part != null)
                    {
                        rank_part.IsDeleted = false;
                        await _rankRepository.UpdateRankParticipantAsync(rank_part);
                    }
                    break;
                case "sport_type":
                    var sport_type = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.SportType>(id);
                    if (sport_type != null)
                    {
                        sport_type.IsDeleted = false;
                        await _rankRepository.UpdateSportTypeAsync(sport_type);
                    }
                    break;
                case "staff":
                    var staff = await _staffRepository.GetByIdAsync<Core.Entities.StaffModel.Staff>(id);
                    if (staff != null)
                    {
                        staff.IsDeleted = false;
                        await _staffRepository.UpdateStaffAsync(staff);
                    }
                    break;
                default:
                    var blog = await _blogRepository.GetByIdAsync<Core.Entities.BlogModel.Blog>(id);
                    if (blog != null)
                    {
                        blog.IsDeleted = false;
                        await _blogRepository.UpdateBlogAsync(blog);
                    }
                    break;
            }

            return new OkObjectResult(id);
        }
    }
}

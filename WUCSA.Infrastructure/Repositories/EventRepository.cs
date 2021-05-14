using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class EventRepository : Repository, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddEventAsync(Event myEvent)
        {
            myEvent.Slug = GetVerifiedBlogSlug(myEvent);
            return AddAsync(myEvent);
        }

        public Task UpdateEventAsync(Event myEvent)
        {
            myEvent.Slug = GetVerifiedBlogSlug(myEvent);
            return UpdateAsync(myEvent);
        }

        public async Task DeleteEventAsync(Event myEvent)
        {
            await DeleteAsync(myEvent);
        }

        public async Task UpdateSportTypesAsync(Event myEvent, bool saveChanges = true, params string[] sportTypesId)
        {
            List<SportType> sportTypes = new List<SportType>();
            foreach (var sportType in sportTypesId)
            {
                var originSType = await GetAsync<SportType>(i => i.Id == sportType);
                if (originSType!=null)
                {
                    sportTypes.Add(originSType);
                }
                
            }
            foreach (var eventSportType in myEvent.EventSportTypes)
            {
                if (sportTypes.Contains(eventSportType.SportType))
                {
                    sportTypes.RemoveAll(x => x.Id == eventSportType.SportTypeId);
                }
                else
                {
                    _context.Set<EventSportType>().Remove(eventSportType);
                }
            }
            foreach (var sportType in sportTypes)
            {
                myEvent.EventSportTypes.Add(new EventSportType
                {
                    SportType = sportType
                });
            }

            if (saveChanges)
            {
                await UpdateAsync(myEvent);
            }
        }

        private string GetVerifiedBlogSlug(Event slugifiedEntity)
        {
            var slug = slugifiedEntity.Slug;
            var verifiedSlug = slug;
            var hasSameSlug = _context.Set<Event>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = slug.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Event>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }

    }
}

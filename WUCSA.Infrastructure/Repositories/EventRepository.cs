using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.StaffModel;
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
            throw new System.NotImplementedException();
        }

        public Task AddEventParticipantAsync(EventParticipant eventParticipant, params Participant[] participants)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteEventAsync(Event myEvent)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteEventParticipantAsync(EventParticipant eventParticipant)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateEventAsync(Event myEvent)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateEventParticipantAsync(EventParticipant eventParticipant, params Participant[] participants)
        {
            throw new System.NotImplementedException();
        }
    }
}

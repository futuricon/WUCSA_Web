using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.ParticipantModel;
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

        public Task AddEventAsync(Event myEevent, IEnumerable<Participant> participants)
        {
            if (participants != null)
            {
                foreach (var participant in participants)
                {
                    myEevent.EventParticipants.Add(new EventParticipant
                    {
                        Participant = participant
                    });
                }
            }

            return AddAsync(myEevent);
        }

        public async Task DeleteEventAsync(Event myEevent)
        {
            //Test this
            await DeleteAsync(myEevent);
        }

        public async Task UpdateEventAsync(Event myEevent, IEnumerable<Participant> participants)
        {
            //Test this
            myEevent.EventParticipants.Clear();

            foreach (var participantId in participants)
            {
                var participant = await GetAsync<Participant>(i => i.Id == participantId.Id);

                if (participant != null)
                {
                    myEevent.EventParticipants.Add(new EventParticipant()
                    {
                        Participant = participant
                    });
                }
            }

            await UpdateAsync(myEevent);
        }
    }
}

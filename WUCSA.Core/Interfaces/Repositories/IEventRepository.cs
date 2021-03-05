using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.ParticipantModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IEventRepository : IRepository
    {
        Task AddEventAsync(Event myEevent, IEnumerable<Participant> participants);
        Task UpdateEventAsync(Event myEevent, IEnumerable<Participant> participants);
        Task DeleteEventAsync(Event myEevent);
    }
}

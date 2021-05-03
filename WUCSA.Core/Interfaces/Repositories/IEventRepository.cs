using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.StaffModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IEventRepository : IRepository
    {
        Task AddEventAsync(Event myEvent);
        Task UpdateEventAsync(Event myEvent);
        Task DeleteEventAsync(Event myEvent);

        Task AddEventParticipantAsync(EventParticipant eventParticipant, params Participant[] participants);
        Task UpdateEventParticipantAsync(EventParticipant eventParticipant, params Participant[] participants);
        Task DeleteEventParticipantAsync(EventParticipant eventParticipant);
    }
}

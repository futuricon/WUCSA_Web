using System.Collections.Generic;
using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IEventRepository : IRepository
    {
        Task AddEventAsync(Event myEvent);
        Task UpdateEventAsync(Event myEvent);
        Task DeleteEventAsync(Event myEvent);
        Task UpdateEventFileAsync(Event myEvent, EventRelatedFile myFile, bool deleted = false);
        Task UpdateSportTypesAsync(Event myEvent, bool saveChanges = true, params string[] sportTypesId);
    }
}

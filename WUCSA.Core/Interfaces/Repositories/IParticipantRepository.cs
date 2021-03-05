using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.ParticipantModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IParticipantRepository : IRepository
    {
        Task AddParticipantAsync(Participant participant, IEnumerable<Certificate> certificates);
        Task UpdateParticipantAsync(Participant participant, IEnumerable<Certificate> certificates);
        Task DeleteParticipantAsync(Participant participant);
    }
}

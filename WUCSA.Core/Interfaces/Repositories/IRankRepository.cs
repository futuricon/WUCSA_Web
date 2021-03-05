using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.ParticipantModel;
using WUCSA.Core.Entities.RankModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IRankRepository : IRepository
    {
        Task AddRankAsync(Rank rank, IEnumerable<Participant> participants);
        Task UpdateRankAsync(Rank rank, IEnumerable<Participant> participants);
        Task DeleteRankAsync(Rank rank);
    }
}

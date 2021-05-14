using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.StaffModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IRankRepository : IRepository
    {
        Task AddRankAsync(Rank rank);
        Task UpdateRankAsync(Rank rank);
        Task DeleteRankAsync(Rank rank);
        
        Task AddSportTypeAsync(SportType sportType);
        Task UpdateSportTypeAsync(SportType sportType);
        Task DeleteSportTypeAsync(SportType sportType);

        Task AddRankParticipantAsync(RankParticipant rankParticipant);
        Task UpdateRankParticipantAsync(RankParticipant rankParticipant);
        Task DeleteRankParticipantAsync(RankParticipant rankParticipant, bool saveChanges = true);

        Task UpdateParticipantAsync(RankParticipant rankParticipant, bool saveChanges = true, params Participant[] participant);
    }
}

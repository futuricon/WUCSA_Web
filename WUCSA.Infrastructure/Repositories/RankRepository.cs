using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class RankRepository : Repository, IRankRepository
    {
        public readonly ApplicationDbContext _context;

        public RankRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //////////////// Rank ////////////////

        public Task AddRankAsync(Rank rank)
        {
            return AddAsync(rank);
        }

        public Task UpdateRankAsync(Rank rank)
        {
            return UpdateAsync(rank);
        }

        public async Task DeleteRankAsync(Rank rank)
        {
            foreach (var rankParticipant in rank.RankParticipants)
            {
                await DeleteRankParticipantAsync(rankParticipant, false);
            }

            await DeleteAsync(rank);
        }


        //////////////// RankParticipant ////////////////

        public Task AddRankParticipantAsync(RankParticipant rankParticipant)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRankParticipantAsync(RankParticipant rankParticipant, bool saveChanges = true, params Participant[] participants)
        {
            
            foreach (var participant in participants)
            {
                
            }

            if (saveChanges)
            {
                //await UpdateAsync(rankParticipant);
            }
        }

        public async Task DeleteRankParticipantAsync(RankParticipant rankParticipant, bool saveChanges = true)
        {
            foreach (var participant in rankParticipant.Participants)
            {
                _context.Remove(participant);
            }

            _context.Remove(rankParticipant);

            if (saveChanges)
            {
                await _context.SaveChangesAsync();
            }
        }



        //////////////// SportType ////////////////

        public Task AddSportTypeAsync(SportType sportType)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSportTypeAsync(SportType sportType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSportTypeAsync(SportType sportType)
        {
            throw new NotImplementedException();
        }        
    }
}

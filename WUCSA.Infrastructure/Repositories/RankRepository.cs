using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.ParticipantModel;
using WUCSA.Core.Entities.RankModel;
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

        public Task AddRankAsync(Rank rank, IEnumerable<Participant> participants)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRankAsync(Rank rank)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRankAsync(Rank rank, IEnumerable<Participant> participants)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class SportTypeRepository : Repository, ISportTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public SportTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Task AddSportTypeAsync(SportType sportType)
        {
            return AddAsync(sportType);
        }

        public Task DeleteSportTypeAsync(SportType sportType)
        {
            return DeleteAsync(sportType);
        }

        public Task UpdateSportTypeAsync(SportType sportType)
        {
            return UpdateAsync(sportType);
            
        }
    }
}

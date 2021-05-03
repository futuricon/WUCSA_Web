using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface ISportTypeRepository : IRepository
    {
        Task AddSportTypeAsync(SportType sportType);
        Task UpdateSportTypeAsync(SportType sportType);
        Task DeleteSportTypeAsync(SportType sportType);
    }
}

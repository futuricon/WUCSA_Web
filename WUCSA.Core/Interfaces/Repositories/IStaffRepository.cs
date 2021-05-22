using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.StaffModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IStaffRepository : IRepository
    {
        Task AddStaffAsync(Staff staff);
        Task UpdateStaffAsync(Staff staff);
        Task DeleteStaffAsync(Staff staff);

        Task UpdateCertificatesAsync(Staff staff, bool saveChanges = true, params Certificate[] certificates);
    }
}

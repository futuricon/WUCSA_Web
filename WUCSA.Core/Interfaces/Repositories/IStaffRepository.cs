using System.Threading.Tasks;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.StaffModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IStaffRepository : IRepository
    {
        Task AddStaffAsync(Staff staff, Certificate certificate);
        Task UpdateStaffAsync(Staff staff, Certificate certificate);
        Task DeleteStaffAsync(Staff staff);
    }
}

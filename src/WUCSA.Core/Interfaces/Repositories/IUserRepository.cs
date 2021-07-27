using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task UpdateUserRolesAsync(AppUser user, IEnumerable<string> roles);
        Task DeleteDeeplyAsync(AppUser user);
    }
}

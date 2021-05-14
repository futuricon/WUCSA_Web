using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class StaffRepository : Repository, IStaffRepository
    {
        public readonly ApplicationDbContext _context;
        public StaffRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        //////////////// Staff ////////////////
        
        public Task AddStaffAsync(Staff staff, Certificate certificate)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStaffAsync(Staff staff, Certificate certificate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStaffAsync(Staff staff)
        {
            throw new NotImplementedException();
        }

    }
}

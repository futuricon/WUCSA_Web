using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public Task AddStaffAsync(Staff staff)
        {
            staff.Slug = GetVerifiedStaffSlug(staff);
            return AddAsync(staff);
        }

        public Task UpdateStaffAsync(Staff staff)
        {
            staff.Slug = GetVerifiedStaffSlug(staff);
            return UpdateAsync(staff);
        }

        public async Task DeleteStaffAsync(Staff staff)
        {
            foreach (var certificate in staff.Certificates)
            {
                await DeleteAsync(certificate);
            }
            await DeleteAsync(staff);
        }

        private string GetVerifiedStaffSlug(Staff slugifiedEntity)
        {
            var staff = slugifiedEntity.Slug;
            var verifiedSlug = staff;
            var hasSameSlug = _context.Set<Staff>().Where(x => x.Id != slugifiedEntity.Id).Any(i => i.Slug == verifiedSlug);

            var count = 0;
            while (hasSameSlug)
            {
                verifiedSlug = staff.Insert(0, $"{++count}-");
                hasSameSlug = _context.Set<Staff>().Any(i => i.Slug == verifiedSlug);
            }

            return verifiedSlug;
        }


        public async Task UpdateCertificatesAsync(Staff staff, bool saveChanges = true, params Certificate[] certificates)
        {
            List<Certificate> oldCertificates = staff.Certificates.ToList();
            var updateCertificates = new List<Certificate>();

            foreach (var oldCertificate in oldCertificates)
            {
                if (certificates.Any(i=>i.Id == oldCertificate.Id && i.CertPath == null))
                {
                    staff.Certificates.ToList().RemoveAll(i=>i.Id == oldCertificate.Id);
                }
            }
            foreach (var certificate in certificates)
            {
                var originCertificate = await GetAsync<Certificate>(i => i.Id == certificate.Id);

                if (originCertificate == null)
                {
                    if (certificate.CertPath == null)
                    {
                        continue;
                    }
                    originCertificate = certificate;
                    await _context.Set<Certificate>().AddAsync(originCertificate);

                }
                
                if (originCertificate != null)
                {
                    if (certificate.CertPath == null)
                    {
                        _context.Set<Certificate>().Remove(originCertificate);
                    }
                    else if (originCertificate.CertPath != certificate.CertPath)
                    {
                        updateCertificates.Add(originCertificate);
                    }
                }
                
                if (staff.Certificates.Any(i=> i.Id == originCertificate.Id))
                {
                    continue;
                }

                staff.Certificates.Add(originCertificate);
            }

            foreach (var updateCertificate in updateCertificates)
            {
                await UpdateAsync(updateCertificate);
            }

            if (saveChanges)
            {
                await UpdateAsync(staff);
            }
        }
    }
}

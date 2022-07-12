using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;
using System.Linq;
using SuxrobGM.Sdk.Extensions;

namespace WUCSA.Web.Pages.Staff
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IStaffRepository _staffRepository;

        public EditModel(ImageHelper imageHelper, IStaffRepository staffRepository)
        {
            _imageHelper = imageHelper;
            _staffRepository = staffRepository;
        }

        public class InputModel
        {
            public Core.Entities.StaffModel.Staff Staff { get; set; }
            public IFormFile UploadCoverPhoto { get; set; }
            public bool IsCoverPhotoDeleted { get; set; } = false;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IFormFile UploadFiles { get; set; }
        public string CertName { get; set; }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _staffRepository.GetByIdAsync<Core.Entities.StaffModel.Staff>(id);

            if (!User.IsInRole("SuperAdmin"))
            {
                if (staff.IsDeleted)
                {
                    return NotFound();
                }
            }

            Input = new InputModel()
            {
                Staff = staff
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Model")] Certificate[] Certificates, IFormFile[] UploadFiles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Certificates.Length > 0)
            {
                int counter = 0;
                foreach (var certificate in Certificates)
                {
                    if (certificate.CertName.Contains("null"))
                    {
                        if (certificate.CertPath == null)
                        {
                            continue;
                        }
                        _imageHelper.DeleteFile(certificate.CertPath);
                        certificate.CertPath = null;
                    }
                    else if (UploadFiles.Length == 0)
                    {
                        continue;
                    }
                    else if (certificate.CertName.Contains("new") && UploadFiles[counter] != null)
                    {
                        if (certificate.CertPath != null)
                        {
                            _imageHelper.DeleteFile(certificate.CertPath);
                        }
                        certificate.CertPath = await _imageHelper.UploadSatffImage(UploadFiles[counter], $"{certificate.Id}_certificate", "certificate_imgs");
                        certificate.CertName = "old-" + certificate.Id;
                        counter++;
                    }
                }
            }

            var staff = await _staffRepository.GetByIdAsync<Core.Entities.StaffModel.Staff>(Input.Staff.Id);
            staff.FirstName = Input.Staff.FirstName;
            staff.LastName = Input.Staff.LastName;
            staff.Position = Input.Staff.Position;
            staff.PositionRu = Input.Staff.PositionRu;
            staff.PositionUz = Input.Staff.PositionUz;
            staff.BirthDate = Input.Staff.BirthDate;
            staff.CountryCode = Input.Staff.CountryCode;
            staff.Description = Input.Staff.Description;
            staff.DescriptionRu = Input.Staff.DescriptionRu;
            staff.DescriptionUz = Input.Staff.DescriptionUz;
            staff.Location = Input.Staff.Location;
            staff.IsMember = Input.Staff.IsMember;
            staff.FacebookUrl = Input.Staff.FacebookUrl;
            staff.InstagramUrl = Input.Staff.InstagramUrl;
            staff.TwitterUrl = Input.Staff.TwitterUrl;
            staff.TelegramUrl = Input.Staff.TelegramUrl;
            staff.PhoneNumber = Input.Staff.PhoneNumber;
            staff.Email = Input.Staff.Email;
            staff.OrderNumber = Input.Staff.OrderNumber;
            var slugTitle = $"{Input.Staff.FirstName}_{Input.Staff.LastName}";
            staff.Slug = slugTitle.Slugify();

            if (Input.UploadCoverPhoto != null)
            {
                staff.ProfilePhotoPath = await _imageHelper.UploadSatffImage(Input.UploadCoverPhoto, $"{Input.Staff.Id}_staff_cover", "staff_imgs");
            }
            if (Input.IsCoverPhotoDeleted) 
            {
                Input.Staff.ProfilePhotoPath = _imageHelper.GenerateImage($"{Input.Staff.Id}_staff_cover", "staff_imgs");
            }

            await _staffRepository.UpdateCertificatesAsync(staff, false, Certificates);
            await _staffRepository.UpdateStaffAsync(staff);
            return RedirectToPage("/Staff/List");
        }
    }
}

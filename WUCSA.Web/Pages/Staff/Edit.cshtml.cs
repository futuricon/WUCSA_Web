using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

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

        public async Task<IActionResult> OnPostAsync([Bind(Prefix = "Model")] List<Certificate> Certificates, IFormFile[] UploadFiles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Certificates.Count > 0)
            {
                int counter = 0;
                foreach (var certificate in Certificates)
                {
                    if (certificate.CertName.Contains("null") && certificate.CertPath != null)
                    {
                        _imageHelper.DeleteFile(certificate.CertPath);
                    }
                    else if (UploadFiles[counter] != null && certificate.CertName.Contains("new"))
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

                //if (UploadFiles.Length > 0)
                //{
                    
                //}
                //else
                //{
                //    foreach (var certificate in Certificates)
                //    {
                //        if (certificate.CertName.Contains("null") && certificate.CertPath != null)
                //        {
                //            _imageHelper.DeleteFile(certificate.CertPath);
                //        }
                //    }
                //}
            }

            if (Input.UploadCoverPhoto != null)
            {
                Input.Staff.ProfilePhotoPath = await _imageHelper.UploadSatffImage(Input.UploadCoverPhoto, $"{Input.Staff.Id}_staff_cover", "staff_imgs");
            }
            else
            {
                Input.Staff.ProfilePhotoPath = _imageHelper.GenerateImage($"{Input.Staff.Id}_staff_cover", "staff_imgs");
            }

            await _staffRepository.UpdateCertificatesAsync(Input.Staff, false, Certificates.ToArray());
            await _staffRepository.AddStaffAsync(Input.Staff);
            return RedirectToPage("/Staff/Index");
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Staff
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ImageHelper _imageHelper;
        private readonly IStaffRepository _staffRepository;

        public DeleteModel(ImageHelper imageHelper, IStaffRepository staffRepository)
        {
            _imageHelper = imageHelper;
            _staffRepository = staffRepository;
        }

        [BindProperty]
        public Core.Entities.StaffModel.Staff Staff { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Staff = await _staffRepository.GetByIdAsync<Core.Entities.StaffModel.Staff>(id);

            if (Staff == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("SuperAdmin"))
            {
                if (Staff.IsDeleted)
                {
                    return NotFound();
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Staff = await _staffRepository.GetByIdAsync<Core.Entities.StaffModel.Staff>(id);

            foreach (var certificate in Staff.Certificates)
            {
                _imageHelper.DeleteFile(certificate.CertPath);
            }

            if (User.IsInRole("SuperAdmin"))
            {
                if (Staff != null)
                {
                    await _staffRepository.DeleteStaffAsync(Staff);
                    _imageHelper.RemoveImage(Staff.ProfilePhotoPath, "staff_imgs");
                }
            }
            else
            {
                Staff.IsDeleted = true;
            }
            return RedirectToPage("/Staff/Index");
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.Rank
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly PDFFileHelper _pdfFileHelper;
        private readonly IRankRepository _rankRepositor;

        public DeleteModel(UserManager<AppUser> userManager,
            PDFFileHelper pdfFileHelper, IRankRepository rankRepositor)
        {
            _userManager = userManager;
            _pdfFileHelper = pdfFileHelper;
            _rankRepositor = rankRepositor;
        }

        [BindProperty]
        public Core.Entities.RankModel.Rank Rank { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rank = await _rankRepositor.GetByIdAsync<Core.Entities.RankModel.Rank>(id);

            if (Rank == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("SuperAdmin"))
            {
                if (Rank.IsDeleted)
                {
                    return NotFound();
                }
            }

            if (Rank.RankParticipants.Count > 0)
            {
                ViewData.Add("ErrorMsg", "This Rank is associated with Participants");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rank = await _rankRepositor.GetByIdAsync<Core.Entities.RankModel.Rank>(id);

            if (User.IsInRole("SuperAdmin"))
            {
                if (Rank != null)
                {
                    if (Rank.RankPartsFilePath != null)
                    {
                        _pdfFileHelper.DeleteFile(Rank.RankPartsFilePath, "ranks");
                    }
                    await _rankRepositor.DeleteRankAsync(Rank);
                }
            }
            else
            {
                Rank.IsDeleted = true;
            }

            return RedirectToPage("/Rank/List", new { location = "World"});
        }
    }
}

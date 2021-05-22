using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.SportType
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly PDFFileHelper _pdfFileHelper;
        private readonly IRankRepository _rankRepositor;

        public DeleteModel(PDFFileHelper pdfFileHelper, IRankRepository rankRepositor)
        {
            _pdfFileHelper = pdfFileHelper;
            _rankRepositor = rankRepositor;
        }

        [BindProperty]
        public Core.Entities.RankModel.SportType SportType { get; set; }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SportType = await _rankRepositor.GetByIdAsync<Core.Entities.RankModel.SportType>(id);

            if (SportType == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("SuperAdmin"))
            {
                if (SportType.IsDeleted)
                {
                    return NotFound();
                }
            }

            if (SportType.EventSportTypes.Count > 0 || SportType.Ranks.Count > 0)
            {
                ViewData.Add("ErrorMsg", "This Type of Sport is associated with an Event and/or a Rank. Please break these associations before deleting");
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SportType = await _rankRepositor.GetByIdAsync<Core.Entities.RankModel.SportType>(id);

            if (User.IsInRole("SuperAdmin"))
            {
                if (SportType != null)
                {
                    if (SportType.RulesFilePath != null)
                    {
                        _pdfFileHelper.DeleteFile(SportType.RulesFilePath, "sportTypes");
                    }
                    await _rankRepositor.DeleteSportTypeAsync(SportType);
                }
            }
            else
            {
                SportType.IsDeleted = true;
            }
            
            return RedirectToPage("/SportType/List");
        }
    }
}

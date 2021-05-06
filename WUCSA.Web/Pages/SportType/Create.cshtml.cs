using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.SportType
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly IRankRepository _rabkRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public CreateModel(IRankRepository rabkRepository, PDFFileHelper pdfFileHelper)
        {
            _rabkRepository = rabkRepository;
            _pdfFileHelper = pdfFileHelper;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.SportType SportType { get; set; }
            public IFormFile UploadPdf { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Input.SportType.Slug = Input.SportType.Name.Slugify();

            if (Input.UploadPdf.Length > 0)
            {
                Input.SportType.RulesFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdf, Input.SportType.Slug, "sportTypes");
            }
            
            await _rabkRepository.AddSportTypeAsync(Input.SportType);
            return RedirectToPage("/SportType/Index", new { slug = Input.SportType.Slug });
        }
    }
}

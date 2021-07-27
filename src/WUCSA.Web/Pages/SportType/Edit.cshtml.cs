using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuxrobGM.Sdk.Extensions;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;
using WUCSA.Web.Utils;

namespace WUCSA.Web.Pages.SportType
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class EditModel : PageModel
    {
        private readonly IRankRepository _rankRepository;
        private readonly PDFFileHelper _pdfFileHelper;

        public EditModel(IRankRepository rankRepository, PDFFileHelper pdfFileHelper)
        {
            _rankRepository = rankRepository;
            _pdfFileHelper = pdfFileHelper;
        }

        public class InputModel
        {
            public Core.Entities.RankModel.SportType SportType { get; set; }
            public IFormFile UploadPdf { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sportType = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.SportType>(id);
            if (sportType == null)
            {
                return NotFound();
            }
            
            if (!User.IsInRole("SuperAdmin"))
            {
                if (sportType.IsDeleted)
                {
                    return NotFound();
                }
            }
            if (sportType.RulesFilePath != null)
            {
                ViewData["PDFFilePath"] = sportType.RulesFilePath;
            }
            Input = new InputModel()
            {
                SportType = sportType
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var sportType = await _rankRepository.GetByIdAsync<Core.Entities.RankModel.SportType>(Input.SportType.Id);
            sportType.Name = Input.SportType.Name;
            sportType.NameRu = Input.SportType.NameRu;
            sportType.NameUz = Input.SportType.NameUz;
            sportType.Description = Input.SportType.Description;
            sportType.DescriptionRu = Input.SportType.DescriptionRu;
            sportType.DescriptionUz = Input.SportType.DescriptionUz;
            sportType.Slug = Input.SportType.Name.Slugify();

            if (Input.UploadPdf != null)
            {
                if (sportType.RulesFilePath != null)
                {
                    _pdfFileHelper.DeleteFile(sportType.RulesFilePath, "sportTypes");
                }
                sportType.RulesFilePath = await _pdfFileHelper.SaveFile(Input.UploadPdf, sportType.Slug, "sportTypes");
            }

            await _rankRepository.UpdateSportTypeAsync(sportType);
            return RedirectToPage("/SportType/Index", new { slug = sportType.Slug });
        }
    }
}

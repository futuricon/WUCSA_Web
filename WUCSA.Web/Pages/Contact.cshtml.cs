using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Web.Pages
{
    public class ContactModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        public ContactModel(UserManager<AppUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [Required]
        [BindProperty]
        public string MsgContent { get; set; }

        [Required]
        [BindProperty]
        public string MsgSubject { get; set; }

        [Required]
        [BindProperty]
        public string AuthorName { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        public string AuthorEmail { get; set; }

        [BindProperty]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string AuthorPhoneNum { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
           
            if (string.IsNullOrWhiteSpace(MsgContent))
            {
                ModelState.AddModelError("Content", "Empty content");
                return Page();
            }

            string TGMsg = $"Hi. There is new message from {AuthorName}.\nE-mail:   {AuthorEmail}\nPhone number:   {AuthorPhoneNum}\nSubject: {MsgSubject}\nContent:   {MsgContent}";

            await _emailService.SendToAllTGAsync(TGMsg);

            return Page();
        }
    }
}

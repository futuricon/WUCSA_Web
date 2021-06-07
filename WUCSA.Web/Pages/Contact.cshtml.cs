using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
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

        public async Task<IActionResult> OnPostMessageAsync(string authorName, string authorEmail, string authorPhone, string msgSubject, string msgContent)
        {
            var user = await _userManager.GetUserAsync(User);
            if (string.IsNullOrWhiteSpace(msgContent) || string.IsNullOrWhiteSpace(authorEmail))
            {
                return BadRequest($"Something went wrong!");
            }

            var RCName = HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.UICulture.Name;

            if (user != null)
            {
                authorName = user.UserName + " (wucsa.com User)";
            }

            string TGMsg = $"Hi. There is new message from {authorName}.\nE-mail:   {authorEmail}\nPhone number:   {authorPhone}\nSubject: {msgSubject}\nContent:   {msgContent}";

            await _emailService.SendToAllTGAsync(TGMsg);

            var responce = RCName switch
            {
                "ru" => "Ваше письмо успешно отправлено!",
                "uz" => "Sizning xatingiz muvaffaqiyatli yuborildi !",
                _ => "Your email has been successfully sent!",
            };

            return new OkObjectResult(responce);
        }
    }
}

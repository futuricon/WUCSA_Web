using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces.Repositories;

namespace WUCSA.Web.Pages.Rank
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRankRepository _rankRepository;

        public IndexModel(UserManager<AppUser> userManager, IRankRepository rankRepository)
        {
            _rankRepository = rankRepository;
            _userManager = userManager;
        }

        public Core.Entities.RankModel.Rank Rank { get; set; }

        public async Task OnGetAsync()
        {
            var rankSlug = RouteData.Values["slug"].ToString();
            Rank = await _rankRepository.GetAsync<Core.Entities.RankModel.Rank>(i => i.Slug == rankSlug);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager, ApplicationDbContext context) : base(context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task UpdateUserRolesAsync(AppUser user, IEnumerable<string> roles)
        {
            var actualRoles = roles.ToList();
            var previousRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, previousRoles);

            foreach (var role in actualRoles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        public async Task DeleteDeeplyAsync(AppUser user)
        {
            if (user == null)
            {
                return;
            }

            var deletedUserAccount = await _userManager.FindByNameAsync("DELETED_USER");
            var articles = _context.Set<Blog>().Where(i => i.Author.Id == user.Id);
            var comments = _context.Set<Comment>().Where(i => i.Author.Id == user.Id);

            foreach (var article in articles)
            {
                article.Author = deletedUserAccount;
            }

            foreach (var comment in comments)
            {
                comment.Author = deletedUserAccount;
            }

            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
        }
    }
}

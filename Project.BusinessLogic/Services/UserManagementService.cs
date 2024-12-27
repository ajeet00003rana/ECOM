using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Project.DataAccess.Services
{
    public interface IUserManagementService
    {
        Task<IEnumerable<IdentityUser>> GetAllUsersAsync();
        Task<IdentityUser> GetUserByIdAsync(string id);
        Task<IdentityResult> UpdateUserAsync(string id, string userName, string email);
        Task<IdentityResult> DeleteUserAsync(string id);
    }

    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagementService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityUser> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> UpdateUserAsync(string id, string userName, string email)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            user.UserName = userName;
            user.Email = email;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await _userManager.DeleteAsync(user);
        }
    }
}

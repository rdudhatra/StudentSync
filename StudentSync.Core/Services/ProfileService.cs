using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.Models;
using StudentSync.Data.ViewModels;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly StudentSyncDbContext _context;

        public ProfileService(StudentSyncDbContext context)
        {
            _context = context;

        }

        public async Task<ProfileViewModel> GetProfileAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            return new ProfileViewModel
            {
                Email = user.Email,
                Username = user.Username,
                Password = user.Password,
                ProfileImage = user.ProfileImage

            };
        }
      
        public async Task UpdateProfileAsync(ProfileViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user != null)
            {
                user.Email = model.Email;
                user.Username = model.Username;
                user.Password = model.Password; // Ensure proper hashing and security here
                user.ProfileImage = model.ProfileImage;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}

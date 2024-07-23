using Microsoft.EntityFrameworkCore;
using StudentSync.Core.Services.Interface;
using StudentSync.Data.Data;
using StudentSync.Data.ViewModels;
using System.Threading.Tasks;

namespace StudentSync.Core.Services
{
    public class ProfileService : IProfileService
    {
        // Assume a database context or similar is injected here
        private readonly StudentSyncDbContext _context;
        private readonly string _imagePath;

        public ProfileService(StudentSyncDbContext context)
        {
            _context = context;
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        }

        public async Task<ProfileViewModel> GetProfileAsync(string username)
        {
            // Fetch the profile from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            return new ProfileViewModel
            {
                Email = user.Email,
                Username = user.Username,
                Password = user.Password, // Ideally, don't expose passwords in this way
                 //ProfileImageUrl = user.ProfileImageUrl // Fetch the URL from the user model

            };
        }

        public async Task UpdateProfileAsync(ProfileViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user != null)
            {
                user.Email = model.Email;
                user.Password = model.Password; // Ensure proper hashing and security here

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}

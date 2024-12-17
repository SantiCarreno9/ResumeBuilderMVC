using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using ResumeBuilder.Models.ViewModels;
using ResumeBuilder.Repositories.Contracts;

namespace ResumeBuilder.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region PERSONAL INFO

        public async Task<PersonalInfo?> GetPersonalInfo(string userId)
        {
            var personalInfo = await _context.PersonalInfo.FindAsync(userId);
            if (personalInfo == null)
                return null;
            return personalInfo;
        }

        public async Task<PersonalInfo?> AddPersonalInfo(PersonalInfo personalInfo)
        {
            _context.Add(personalInfo);
            await _context.SaveChangesAsync();
            return personalInfo;
        }

        public async Task<PersonalInfo?> UpdatePersonalInfo(string userId, PersonalInfo personalInfo)
        {
            var existingInfo = await _context.PersonalInfo.FindAsync(userId);
            if (existingInfo == null)
                return null;
            
            existingInfo.FirstName = personalInfo.FirstName;
            existingInfo.LastName = personalInfo.LastName;
            existingInfo.Address = personalInfo.Address;
            existingInfo.Email = personalInfo.Email;
            existingInfo.AdditionalContactInfo = personalInfo.AdditionalContactInfo;
            existingInfo.PhoneNumber = personalInfo.PhoneNumber;
                        
            await _context.SaveChangesAsync();
            return existingInfo;
        }

        public async Task DeletePersonalInfo(string userId)
        {
            var userInfo = await _context.PersonalInfo.FindAsync(userId);
            if (userInfo != null)
            {
                _context.PersonalInfo.Remove(userInfo);
            }

            await _context.SaveChangesAsync();
            return;
        }

        #endregion

        #region PROFILE ENTRY

        public async Task<IEnumerable<ProfileEntry>?> GetProfileEntries(string userId)
        {
            var profileEntries = _context.ProfileEntry.Where(pe => pe.UserId!.Equals(userId)).AsNoTracking();
            if (profileEntries == null)
                return null;
            return profileEntries.OrderByDescending(x => x.StartDate);
        }

        public async Task<IEnumerable<ProfileEntry>?> GetProfileEntriesByCategory(string userId, EntryCategory entryCategory)
        {
            var profileEntries = _context.ProfileEntry.Where(pe => pe.UserId!.Equals(userId) && pe.Category == entryCategory).AsNoTracking();
            if (profileEntries == null)
                return null;
            return profileEntries.OrderByDescending(x => x.StartDate);
        }

        public async Task<ProfileEntry?> GetProfileEntry(string userId, string id)
        {
            var profileEntries = await _context.ProfileEntry.FindAsync(id);
            if (profileEntries == null || !profileEntries.UserId.Equals(userId))
                return null;
            return profileEntries;
        }

        public async Task<ProfileEntry?> AddProfileEntry(ProfileEntry profileEntry)
        {
            _context.Add(profileEntry);
            await _context.SaveChangesAsync();
            return profileEntry;
        }

        public async Task<ProfileEntry?> UpdateProfileEntry(string userId, string id, ProfileEntry profileEntry)
        {
            var existingInfo = await _context.ProfileEntry.FindAsync(userId);
            if (existingInfo == null || !existingInfo.UserId.Equals(userId))
                return null;

            _context.Update(profileEntry);
            await _context.SaveChangesAsync();
            return profileEntry;
        }

        public async Task DeleteProfileEntry(string userId, string id)
        {
            var profileEntry = await _context.ProfileEntry.FindAsync(userId);
            if (profileEntry != null && profileEntry.UserId.Equals(userId))
                _context.ProfileEntry.Remove(profileEntry);

            await _context.SaveChangesAsync();
            return;
        }

        #endregion


        public void Dispose()
        {

        }

        
    }
}

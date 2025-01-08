using ResumeBuilder.Models;

namespace ResumeBuilder.Repositories.Contracts
{
    public interface IUserRepository : IDisposable
    {
        Task<PersonalInfo?> GetPersonalInfo(string userId);
        Task<PersonalInfo?> AddPersonalInfo(PersonalInfo personalInfo);
        Task<PersonalInfo?> UpdatePersonalInfo(string userId, PersonalInfo personalInfo);
        Task DeletePersonalInfo(string userId);

        Task<ProfileEntry?> GetProfileEntry(string userId, string id);
        Task<IEnumerable<ProfileEntry>?> GetProfileEntries(string userId);
        Task<IEnumerable<ProfileEntry>?> GetProfileEntriesByCategory(string userId, string entryCategory);
        Task<ProfileEntry?> AddProfileEntry(ProfileEntry profileEntry);
        Task<ProfileEntry?> UpdateProfileEntry(string userId, string id, ProfileEntry profileEntry);
        Task DeleteProfileEntry(string userId, string id);
    }
}

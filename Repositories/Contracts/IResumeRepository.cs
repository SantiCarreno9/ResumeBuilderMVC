using ResumeBuilder.Models;

namespace ResumeBuilder.Repositories.Contracts
{
    public interface IResumeRepository : IDisposable
    {
        Task<IEnumerable<Resume>?> GetResumes(string userId);
        Task<Resume?> GetResume(string userId, string resumeId);

        Task<string?> GetResumeName(string userId, string resumeId);
        Task<ResumeBasicInfo?> GetResumeBasicInfo(string userId, string resumeId);
        Task<PersonalInfo?> GetResumePersonalInfo(string userId, string resumeId);
        Task<ProfileEntry?> GetResumeProfileEntry(string userId, string resumeId, string id);
        Task<IEnumerable<ProfileEntry>?> GetResumeProfileEntries(string userId, string resumeId);
        Task<IEnumerable<ProfileEntry>?> GetResumeProfileEntriesByCategory(string userId, string resumeId, EntryCategory category);

        Task<Resume?> AddResume(Resume resume);

        Task<Resume?> UpdateResume(string userId, string resumeId, Resume resume);
        Task<string?> UpdateResumeName(string userId, string resumeId, string resumeName);
        Task<ResumeBasicInfo?> UpdateResumeBasicInfo(string userId, string resumeId, ResumeBasicInfo basicInfo);
        Task<PersonalInfo?> UpdateResumePersonalInfo(string userId, string resumeId, PersonalInfo personalInfo);
        Task<ProfileEntry?> UpdateResumeProfileEntry(string userId, string resumeId, ProfileEntry profileEntry);
        Task<List<ProfileEntry>?> UpdateResumeProfileEntries(string userId, string resumeId, List<ProfileEntry> profileEntries);

        Task DeleteResume(string userId, string id);
        Task DeleteResumeProfileEntry(string userId, string resumeId, string id);
    }
}

using Newtonsoft.Json;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using ResumeBuilder.Models.ViewModels;
using ResumeBuilder.Repositories.Contracts;

namespace ResumeBuilder.Repositories.Implementations
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resume>?> GetResumes(string userId)
        {
            var resumes = _context.Resumes.Where(r => r.UserId!.Equals(userId));
            if (resumes == null)
                return null;
            return resumes;
        }

        public async Task<Resume?> GetResume(string userId, string id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null || !resume.UserId.Equals(userId))
                return null;
            return resume;
        }

        public async Task<Resume?> AddResume(Resume resume)
        {
            _context.Add(resume);
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task<Resume?> UpdateResume(string userId, string id, Resume resume)
        {
            var existingResume = await _context.Resumes.FindAsync(id);
            if (existingResume == null || !existingResume.UserId.Equals(userId))
                return null;

            _context.Update(resume);
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task DeleteResume(string userId, string id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null && resume.UserId.Equals(userId))
            {
                _context.Resumes.Remove(resume);
            }

            await _context.SaveChangesAsync();
            return;
        }

        public async Task<string?> GetResumeName(string userId, string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId))
                return null;

            return resume.Name;
        }

        public async Task<string?> UpdateResumeName(string userId, string resumeId, string resumeName)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId))
                return null;

            resume.Name = resumeName;
            await _context.SaveChangesAsync();
            return resumeName;
        }

        public async Task<ResumeBasicInfo?> GetResumeBasicInfo(string userId, string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ResumeInfo == null)
                return null;

            var basicInfo = JsonConvert.DeserializeObject<ResumeBasicInfo>(resume.ResumeInfo);
            return basicInfo;
        }

        public async Task<ResumeBasicInfo?> UpdateResumeBasicInfo(string userId, string resumeId, ResumeBasicInfo basicInfo)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ResumeInfo == null)
                return null;

            resume.ResumeInfo = JsonConvert.SerializeObject(basicInfo);
            await _context.SaveChangesAsync();
            return basicInfo;
        }

        public async Task<PersonalInfo?> GetResumePersonalInfo(string userId, string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.PersonalInfo == null)
                return null;

            var personalInfo = JsonConvert.DeserializeObject<PersonalInfo>(resume.PersonalInfo);
            return personalInfo;
        }

        public async Task<PersonalInfo?> UpdateResumePersonalInfo(string userId, string resumeId, PersonalInfo personalInfo)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.PersonalInfo == null)
                return null;

            resume.PersonalInfo = JsonConvert.SerializeObject(personalInfo);
            await _context.SaveChangesAsync();
            return personalInfo;
        }

        public async Task<IEnumerable<ProfileEntry>?> GetResumeProfileEntries(string userId, string resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ProfileEntries == null)
                return null;

            var profileEntries = JsonConvert.DeserializeObject<IEnumerable<ProfileEntry>>(resume.ProfileEntries);
            return profileEntries;
        }

        public async Task<IEnumerable<ProfileEntry>?> GetResumeProfileEntriesByCategory(string userId, string resumeId, EntryCategory category)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ProfileEntries == null)
                return null;

            var profileEntries = JsonConvert.DeserializeObject<List<ProfileEntry>>(resume.ProfileEntries);
            if (profileEntries == null)
                return null;

            return profileEntries.Where(pe => pe.Category == category).OrderByDescending(pe => pe.StartDate);
        }

        public async Task<ProfileEntry?> GetResumeProfileEntry(string userId, string resumeId, string id)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ProfileEntries == null)
                return null;

            var profileEntries = JsonConvert.DeserializeObject<List<ProfileEntry>>(resume.ProfileEntries);
            if (profileEntries == null)
                return null;

            return profileEntries.FirstOrDefault(pe => pe.Id.Equals(id));
        }

        public async Task<List<ProfileEntry>?> UpdateResumeProfileEntries(string userId, string resumeId, List<ProfileEntry> profileEntries)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ProfileEntries == null)
                return null;

            var existingProfileEntries = JsonConvert.DeserializeObject<List<ProfileEntry>>(resume.ProfileEntries);
            if (existingProfileEntries == null)
                return null;

            for (int i = 0; i < profileEntries.Count; i++)
            {
                var existingEntry = existingProfileEntries.FirstOrDefault(x => x.Id.Equals(profileEntries[i].Id));
                if (existingEntry == null)
                    existingProfileEntries.Add(profileEntries[i]);
                else
                {
                    int index = existingProfileEntries.IndexOf(existingEntry);
                    existingProfileEntries[index] = profileEntries[i];
                }
            }
            resume.ProfileEntries = JsonConvert.SerializeObject(existingProfileEntries);
            await _context.SaveChangesAsync();
            return existingProfileEntries;
        }

        public async Task<ProfileEntry?> UpdateResumeProfileEntry(string userId, string resumeId, ProfileEntry profileEntry)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ProfileEntries == null)
                return null;

            var existingProfileEntries = JsonConvert.DeserializeObject<List<ProfileEntry>>(resume.ProfileEntries);
            if (existingProfileEntries == null)
                return null;

            var existingEntry = existingProfileEntries.FirstOrDefault(x => x.Id.Equals(profileEntry.Id));
            if (existingEntry == null)
                existingProfileEntries.Add(profileEntry);
            else
            {
                int index = existingProfileEntries.IndexOf(existingEntry);
                existingProfileEntries[index] = profileEntry;
            }
            resume.ProfileEntries = JsonConvert.SerializeObject(existingProfileEntries);
            await _context.SaveChangesAsync();
            return profileEntry;
        }

        public async Task DeleteResumeProfileEntry(string userId, string resumeId, string id)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null || !resume.UserId.Equals(userId) || resume.ProfileEntries == null)
                return;

            var existingProfileEntries = JsonConvert.DeserializeObject<List<ProfileEntry>>(resume.ProfileEntries);
            if (existingProfileEntries == null)
                return;

            var existingEntry = existingProfileEntries.FirstOrDefault(x => x.Id.Equals(id));
            if (existingEntry == null)
                return;

            existingProfileEntries.Remove(existingEntry);
            resume.ProfileEntries = JsonConvert.SerializeObject(existingProfileEntries);
            await _context.SaveChangesAsync();
            return;
        }

        public void Dispose()
        {

        }

    }
}

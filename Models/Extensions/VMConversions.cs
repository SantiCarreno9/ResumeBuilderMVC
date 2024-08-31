using ResumeBuilder.Models.ViewModels;
using System.Text.Json;

namespace ResumeBuilder.Models.Extensions
{
    public static class VMConversions
    {
        public static PersonalInfo ConvertToEntity(this VMPersonalInfo vmPersonalInfo)
        {
            return new PersonalInfo
            {
                FirstName = vmPersonalInfo.FirstName,
                LastName = vmPersonalInfo.LastName,
                Address = vmPersonalInfo.Address,
                Email = vmPersonalInfo.Email,
                PostalCode = vmPersonalInfo.PostalCode,
                PhoneNumber = vmPersonalInfo.PhoneNumber,
                LinkedInURL = vmPersonalInfo.LinkedInURL,
                GitHubAccount = vmPersonalInfo.GitHubAccount,
                WebsiteURL = vmPersonalInfo.WebsiteURL
            };
        }

        public static VMPersonalInfo ConvertToViewModel(this PersonalInfo personalInfo)
        {
            return new VMPersonalInfo
            {
                FirstName = personalInfo.FirstName,
                LastName = personalInfo.LastName,
                Address = personalInfo.Address,
                Email = personalInfo.Email,
                PostalCode = personalInfo.PostalCode,
                PhoneNumber = personalInfo.PhoneNumber,
                LinkedInURL = personalInfo.LinkedInURL,
                GitHubAccount = personalInfo.GitHubAccount,
                WebsiteURL = personalInfo.WebsiteURL
            };
        }

        public static ProfileEntry ConvertToEntity(this VMProfileEntry vMProfileEntry) 
        {
            return new ProfileEntry
            {
                Id = vMProfileEntry.Id,
                Title = vMProfileEntry.Title,
                Organization = vMProfileEntry.Organization,
                StartDate = vMProfileEntry.StartDate,
                EndDate = vMProfileEntry.EndDate,
                IsCurrent = vMProfileEntry.IsCurrent,
                Category = vMProfileEntry.Category,
                Details = vMProfileEntry.Details,
                Location = vMProfileEntry.Location
            };
        }

        public static VMProfileEntry ConvertToViewModel(this ProfileEntry profileEntry)
        {
            return new VMProfileEntry
            {
                Id = profileEntry.Id,
                Title = profileEntry.Title,
                Organization = profileEntry.Organization,
                StartDate = profileEntry.StartDate,
                EndDate = profileEntry.EndDate,
                IsCurrent = profileEntry.IsCurrent,
                Category = profileEntry.Category,
                Details = profileEntry.Details,
                Location = profileEntry.Location
            };
        }

        public static Resume ConvertToEntity(this VMResume vmResume)
        {
            return new Resume
            {
                Id = vmResume.Id,
                ResumeInfo = JsonSerializer.Serialize(vmResume.ResumeInfo),
                PersonalInfo = JsonSerializer.Serialize(vmResume.PersonalInfo),
                ProfileEntries = JsonSerializer.Serialize(vmResume.ProfileEntries)
            };
        }

        public static VMResume ConvertToViewModel(this Resume resume)
        {            
            return new VMResume
            {
                Id = resume.Id,
                ResumeInfo = resume.ResumeInfo != null ? JsonSerializer.Deserialize<BasicResumeInfo>(resume.ResumeInfo) : new BasicResumeInfo(),
                PersonalInfo = resume.PersonalInfo != null ? JsonSerializer.Deserialize<PersonalInfo>(resume.PersonalInfo) : new PersonalInfo(),
                ProfileEntries = resume.ProfileEntries != null ? JsonSerializer.Deserialize<List<ProfileEntry>>(resume.ProfileEntries) : new List<ProfileEntry>(),
            };
        }

        //private Resume ConvertToResume(VMResume vmResume)
        //{
        //    Resume resume = new Resume
        //    {
        //        Description = vmResume.Description,
        //        Skills = vmResume.Skills,
        //        ResumeName = vmResume.ResumeName,
        //        JobTitle = vmResume.JobTitle,
        //    };
        //    string personalInfoJSON = JsonSerializer.Serialize(vmResume.PersonalInfo);
        //    var educationRecord = vmResume.ProfessionalRecord?.Where(x => x.Category == EntryCategory.Education);
        //    string educationRecordJSON = JsonSerializer.Serialize(educationRecord);
        //    var experienceRecord = vmResume.ProfessionalRecord?.Where(x => x.Category == EntryCategory.WorkExperience);
        //    string experienceRecordJSON = JsonSerializer.Serialize(experienceRecord);

        //    resume.PersonalInfo = personalInfoJSON;
        //    resume.EducationRecord = educationRecordJSON;
        //    resume.ExperienceRecord = experienceRecordJSON;
        //    return resume;
        //}

        //private VMResume ConvertToVMResume(Resume resume)
        //{
        //    VMResume vmResume = new VMResume
        //    {
        //        Description = resume.Description,
        //        Skills = resume.Skills,
        //        ResumeName = resume.ResumeName,
        //        JobTitle = resume.JobTitle,
        //    };
        //    var personalInfo = JsonSerializer.Deserialize<PersonalInfo>(resume.PersonalInfo);
        //    var educationRecord = JsonSerializer.Deserialize<ICollection<ProfileEntry>>(resume.EducationRecord);
        //    var experienceRecord = JsonSerializer.Deserialize<ICollection<ProfileEntry>>(resume.ExperienceRecord);

        //    vmResume.PersonalInfo = personalInfo;
        //    vmResume.ProfessionalRecord = new List<ProfileEntry>();
        //    if (educationRecord != null)
        //        vmResume.ProfessionalRecord.AddRange(educationRecord);
        //    if (experienceRecord != null)
        //        vmResume.ProfessionalRecord.AddRange(experienceRecord);
        //    return vmResume;
        //}
    }
}

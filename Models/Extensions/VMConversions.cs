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
                PhoneNumber = vmPersonalInfo.PhoneNumber,
                AdditionalContactInfo = vmPersonalInfo.AdditionalContactInfo
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
                PhoneNumber = personalInfo.PhoneNumber,
                AdditionalContactInfo = personalInfo.AdditionalContactInfo
            };
        }

        public static VMResumeBasicInfo ConvertToViewModel(this ResumeBasicInfo resumeBasicInfo)
        {
            return new VMResumeBasicInfo
            {
                JobTitle = resumeBasicInfo.JobTitle,
                Description = resumeBasicInfo.Description,
                Skills = resumeBasicInfo.Skills
            };
        }

        public static Resume ConvertToEntity(this VMResume vmResume)
        {
            return new Resume
            {
                Id = vmResume.Id,
                ResumeInfo = JsonSerializer.Serialize(vmResume.ResumeInfo),
                PersonalInfo = JsonSerializer.Serialize(vmResume.PersonalInfo),
                ProfileEntries = JsonSerializer.Serialize(vmResume.ProfileEntries),
                ModifiedAt = vmResume.ModifiedAt,
                OrderedCategories = JsonSerializer.Serialize(vmResume.ProfileEntries),
            };
        }

        public static VMResume ConvertToViewModel(this Resume resume)
        {
            return new VMResume
            {
                Id = resume.Id,
                ResumeInfo = resume.ResumeInfo != null ? JsonSerializer.Deserialize<VMResumeBasicInfo>(resume.ResumeInfo) : new VMResumeBasicInfo(),
                PersonalInfo = resume.PersonalInfo != null ? JsonSerializer.Deserialize<VMPersonalInfo>(resume.PersonalInfo) : new VMPersonalInfo(),
                ProfileEntries = resume.ProfileEntries != null ? JsonSerializer.Deserialize<List<ProfileEntry>>(resume.ProfileEntries) : new List<ProfileEntry>(),
                ModifiedAt = resume.ModifiedAt,
                OrderedCategories = resume.OrderedCategories != null ? JsonSerializer.Deserialize<EntryCategory[]>(resume.OrderedCategories) : Enum.GetValues<EntryCategory>(),
            };
        }

    }
}

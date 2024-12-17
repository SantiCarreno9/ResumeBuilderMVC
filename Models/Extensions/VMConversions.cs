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
                ResumeInfo = resume.ResumeInfo != null ? JsonSerializer.Deserialize<ResumeBasicInfo>(resume.ResumeInfo) : new ResumeBasicInfo(),
                PersonalInfo = resume.PersonalInfo != null ? JsonSerializer.Deserialize<PersonalInfo>(resume.PersonalInfo) : new PersonalInfo(),
                ProfileEntries = resume.ProfileEntries != null ? JsonSerializer.Deserialize<List<ProfileEntry>>(resume.ProfileEntries) : new List<ProfileEntry>(),
            };
        }
        
    }
}
